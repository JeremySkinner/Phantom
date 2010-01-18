/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Microsoft Public License. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the  Microsoft Public License, please send an email to 
 * ironruby@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Microsoft Public License.
 *
 * You must not remove this notice, or any other, from this software.
 *
 *
 * ***************************************************************************/

// This file is from Microsoft's IronRuby project (http://www.ironruby.net)

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections;

namespace Phantom.Core {
	[ExcludeFromCoverage]
	internal static class Glob {
        // Duplicated constants from File.Constants
		[ExcludeFromCoverage]
        private static class Constants {
            public readonly static int FNM_CASEFOLD = 0x08;
            public readonly static int FNM_DOTMATCH = 0x04;
            public readonly static int FNM_NOESCAPE = 0x01;
            public readonly static int FNM_PATHNAME = 0x02;
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")] // TODO
            public readonly static int FNM_SYSCASE = 0x08;
        }

		[ExcludeFromCoverage]
        private class CharClass {
            private readonly StringBuilder/*!*/ _chars = new StringBuilder();

            internal void Add(char c) {
                if (c == ']' || c == '\\') {
                    _chars.Append('\\');
                }
                _chars.Append(c);
            }

            internal string MakeString() {
                if (_chars.Length == 0) {
                    return null;
                }
                if (_chars.Length == 1 && _chars[0] == '^') {
                    _chars.Insert(0, "\\");
                }
                _chars.Insert(0, "[");
                _chars.Append(']');
                return _chars.ToString();
            }
        }

        private static void AppendExplicitRegexChar(StringBuilder/*!*/ builder, char c) {
            builder.Append('[');
            if (c == '^' || c == '\\') {
                builder.Append('\\');
            }
            builder.Append(c);
            builder.Append(']');
        }

        internal static string/*!*/ PatternToRegex(string/*!*/ pattern, bool pathName, bool noEscape) {
            StringBuilder result = new StringBuilder(pattern.Length);
            result.Append("\\G");

            bool inEscape = false;
            CharClass charClass = null;

            foreach (char c in pattern) {
                if (c == '\\' && !inEscape && !noEscape) {
                    inEscape = true;
                    continue;
                }
                if (inEscape) {
                    if (charClass != null) {
                        charClass.Add(c);
                    } else {
                        AppendExplicitRegexChar(result, c);
                    }
                    inEscape = false;
                    continue;
                }
                if (charClass != null) {
                    if (c == ']') {
                        string set = charClass.MakeString();
                        if (set == null) {
                            // Ruby regex "[]" matches nothing
                            // CLR regex "[]" throws exception
                            return String.Empty;
                        }
                        result.Append(set);
                        charClass = null;
                    } else {
                        charClass.Add(c);
                    }
                    continue;
                }
                switch (c) {
                    case '*':
                        result.Append(pathName ? "[^/]*" : ".*");
                        break;

                    case '?':
                        result.Append('.');
                        break;

                    case '[':
                        charClass = new CharClass();
                        break;

                    default:
                        AppendExplicitRegexChar(result, c);
                        break;
                }
            }

            return (charClass == null) ? result.ToString() : String.Empty;
        }

        public static bool FnMatch(string/*!*/ pattern, string/*!*/ path, int flags) {
            if (pattern.Length == 0) {
                return path.Length == 0;
            }

            bool pathName = ((flags & Constants.FNM_PATHNAME) != 0);
            bool noEscape = ((flags & Constants.FNM_NOESCAPE) != 0);
            string regexPattern = PatternToRegex(pattern, pathName, noEscape);
            if (regexPattern.Length == 0) {
                return false;
            }

            if (((flags & Constants.FNM_DOTMATCH) == 0) && path.Length > 0 && path[0] == '.') {
                // Starting dot requires an explicit dot in the pattern
                if (regexPattern.Length < 4 || regexPattern[2] != '[' || regexPattern[3] != '.') {
                    return false;
                }
            }

            RegexOptions options = RegexOptions.None;
            if ((flags & Constants.FNM_CASEFOLD) != 0) {
                options |= RegexOptions.IgnoreCase;
            }
            Match match = Regex.Match(path, regexPattern, options);
            return match != null && match.Success && (match.Length == path.Length);
        }

		[ExcludeFromCoverage]
        private class GlobUngrouper {
			[ExcludeFromCoverage]
            internal abstract class GlobNode {
                internal readonly GlobNode/*!*/ _parent;
                protected GlobNode(GlobNode parentNode) {
                    _parent = parentNode ?? this;
                }
                abstract internal GlobNode/*!*/ AddChar(char c);
                abstract internal GlobNode/*!*/ StartLevel();
                abstract internal GlobNode/*!*/ AddGroup();
                abstract internal GlobNode/*!*/ FinishLevel();
                abstract internal List<StringBuilder>/*!*/ Flatten();
            }
			[ExcludeFromCoverage]
            internal class TextNode : GlobNode {
                private readonly StringBuilder/*!*/ _builder;

                internal TextNode(GlobNode/*!*/ parentNode)
                    : base(parentNode) {
                    _builder = new StringBuilder();
                }
                internal override GlobNode/*!*/ AddChar(char c) {
                    if (c != 0) {
                        _builder.Append(c);
                    }
                    return this;
                }
                internal override GlobNode/*!*/ StartLevel() {
                    return _parent.StartLevel();
                }
                internal override GlobNode/*!*/ AddGroup() {
                    return _parent.AddGroup();
                }
                internal override GlobNode/*!*/ FinishLevel() {
                    return _parent.FinishLevel();
                }
                internal override List<StringBuilder>/*!*/ Flatten() {
                    List<StringBuilder> result = new List<StringBuilder>(1);
                    result.Add(_builder);
                    return result;
                }
            }

			[ExcludeFromCoverage]
            internal class ChoiceNode : GlobNode {
                private readonly List<SequenceNode>/*!*/ _nodes;

                internal ChoiceNode(GlobNode/*!*/ parentNode)
                    : base(parentNode) {
                    _nodes = new List<SequenceNode>();
                }
                internal override GlobNode/*!*/ AddChar(char c) {
                    SequenceNode node = new SequenceNode(this);
                    _nodes.Add(node);
                    return node.AddChar(c);
                }
                internal override GlobNode/*!*/ StartLevel() {
                    SequenceNode node = new SequenceNode(this);
                    _nodes.Add(node);
                    return node.StartLevel();
                }
                internal override GlobNode/*!*/ AddGroup() {
                    AddChar('\0');
                    return this;
                }
                internal override GlobNode/*!*/ FinishLevel() {
                    AddChar('\0');
                    return _parent;
                }
                internal override List<StringBuilder>/*!*/ Flatten() {
                    List<StringBuilder> result = new List<StringBuilder>();
                    foreach (GlobNode node in _nodes) {
                        foreach (StringBuilder builder in node.Flatten()) {
                            result.Add(builder);
                        }
                    }
                    return result;
                }
            }

			[ExcludeFromCoverage]
            internal class SequenceNode : GlobNode {
                private readonly List<GlobNode>/*!*/ _nodes;

                internal SequenceNode(GlobNode parentNode)
                    : base(parentNode) {
                    _nodes = new List<GlobNode>();
                }

                internal override GlobNode/*!*/ AddChar(char c) {
                    TextNode node = new TextNode(this);
                    _nodes.Add(node);
                    return node.AddChar(c);
                }

                internal override GlobNode/*!*/ StartLevel() {
                    ChoiceNode node = new ChoiceNode(this);
                    _nodes.Add(node);
                    return node;
                }

                internal override GlobNode/*!*/ AddGroup() {
                    return _parent;
                }

                internal override GlobNode/*!*/ FinishLevel() {
                    return _parent._parent;
                }

                internal override List<StringBuilder>/*!*/ Flatten() {
                    List<StringBuilder> result = new List<StringBuilder>();
                    result.Add(new StringBuilder());
                    foreach (GlobNode node in _nodes) {
                        List<StringBuilder> tmp = new List<StringBuilder>();
                        foreach (StringBuilder builder in node.Flatten()) {
                            foreach (StringBuilder sb in result) {
                                StringBuilder newsb = new StringBuilder(sb.ToString());
                                newsb.Append(builder.ToString());
                                tmp.Add(newsb);
                            }
                        }
                        result = tmp;
                    }
                    return result;
                }
            }

            private readonly SequenceNode/*!*/ _rootNode;
            private GlobNode/*!*/ _currentNode;
            private int _level;

            internal GlobUngrouper(int patternLength) {
                _rootNode = new SequenceNode(null);
                _currentNode = _rootNode;
                _level = 0;
            }

            internal void AddChar(char c) {
                _currentNode = _currentNode.AddChar(c);
            }

            internal void StartLevel() {
                _currentNode = _currentNode.StartLevel();
                _level++;
            }

            internal void AddGroup() {
                _currentNode = _currentNode.AddGroup();
            }

            internal void FinishLevel() {
                _currentNode = _currentNode.FinishLevel();
                _level--;
            }
            internal int Level {
                get { return _level; }
            }
            internal string[]/*!*/ Flatten() {
                if (_level != 0) {
                    return new string[0];
                }
                List<StringBuilder> list = _rootNode.Flatten();
                string[] result = new string[list.Count];
                for (int i = 0; i < list.Count; i++) {
                    result[i] = list[i].ToString();
                }
                return result;
            }
        }

        private static string[] UngroupGlobs(string/*!*/ pattern, bool noEscape) {
            if (pattern.IndexOf('{') < 0) {
                if (pattern.IndexOf('}') < 0) {
                    return new string[1] { pattern };
                } else {
                    return new string[0];
                }
            }

            GlobUngrouper ungrouper = new GlobUngrouper(pattern.Length);

            bool inEscape = false;
            foreach (char c in pattern) {
                if (c == '\\' && !inEscape && !noEscape) {
                    inEscape = true;
                    continue;
                }
                if (inEscape) {
                    if (c != ',' && c != '{' && c != '{') {
                        ungrouper.AddChar('\\');
                    }
                    ungrouper.AddChar(c);
                    inEscape = false;
                    continue;
                }
                switch (c) {
                    case '{':
                        ungrouper.StartLevel();
                        break;

                    case ',':
                        if (ungrouper.Level < 1) {
                            ungrouper.AddChar(c);
                        } else {
                            ungrouper.AddGroup();
                        }
                        break;

                    case '}':
                        if (ungrouper.Level < 1) {
                            // Unbalanced closing bracket matches nothing
                            return new string[0];
                        }
                        ungrouper.FinishLevel();
                        break;

                    default:
                        ungrouper.AddChar(c);
                        break;
                }
            }
            return ungrouper.Flatten();
        }

		[ExcludeFromCoverage]
        private sealed class GlobMatcher {
            readonly PlatformAdaptationLayer/*!*/ _pal;
            readonly string/*!*/ _pattern;
            readonly int _flags;
            readonly bool _dirOnly;
            readonly List<string>/*!*/ _result;
            bool _stripTwo;

			public static int LastCharacter(string str) {
				if (str.Length != 0) {
					return str[str.Length - 1];
				}
				return -1;
			}


            internal GlobMatcher(PlatformAdaptationLayer/*!*/ pal, string/*!*/ pattern, int flags) {
                _pal = pal;
                _pattern = (pattern == "**") ? "*" : pattern;
                _flags = flags | Constants.FNM_CASEFOLD;
                _result = new List<string>();
                _dirOnly = LastCharacter(pattern) == '/';
                _stripTwo = false;
            }

            internal int FindNextSeparator(int position, bool allowWildcard, out bool containsWildcard) {
                int lastSlash = -1;
                bool inEscape = false;
                containsWildcard = false;
                for (int i = position; i < _pattern.Length; i++) {
                    if (inEscape) {
                        inEscape = false;
                        continue;
                    }
                    char c = _pattern[i];
                    if (c == '\\') {
                        inEscape = true;
                        continue;
                    } else if (c == '*' || c == '?' || c == '[') {
                        if (!allowWildcard) {
                            return lastSlash + 1;
                        } else if (lastSlash >= 0) {
                            return lastSlash;
                        }
                        containsWildcard = true;
                    } else if (c == '/' || c == ':') {
                        if (containsWildcard) {
                            return i;
                        }
                        lastSlash = i;
                    }
                }
                return _pattern.Length;
            }

            private void TestPath(string path, int patternEnd, bool isLastPathSegment) {
                if (!isLastPathSegment) {
                    DoGlob(path, patternEnd, false);
                    return;
                }
                string pathName = path.Replace('\\', '/');
                if (_stripTwo) {
                    pathName = pathName.Substring(2);
                }
                if (_pal.DirectoryExists(pathName)) {
                    _result.Add(pathName);
                } else if (!_dirOnly && _pal.FileExists(pathName)) {
                    _result.Add(pathName);
                }
            }

            internal IList<string>/*!*/ DoGlob() {
                if (_pattern.Length == 0) {
                    return new string[0];
                }

                int pos = 0;
                string baseDirectory = ".";
                if (_pattern[0] == '/' || _pattern.IndexOf(':') >= 0) {
                    bool containsWildcard;
                    pos = FindNextSeparator(0, false, out containsWildcard);
                    if (pos == _pattern.Length) {
                        TestPath(_pattern, pos, true);
                        return _result;
                    }
                    if (pos > 0 || _pattern[0] == '/') {
                        baseDirectory = _pattern.Substring(0, pos);
                    }
                }

                _stripTwo = (baseDirectory == ".");

                DoGlob(baseDirectory, pos, false);
                return _result;
            }

            internal void DoGlob(string/*!*/ baseDirectory, int position, bool isPreviousDoubleStar) {
                if (!_pal.DirectoryExists(baseDirectory)) {
                    return;
                }

                bool containsWildcard;
                int patternEnd = FindNextSeparator(position, true, out containsWildcard);
                bool isLastPathSegment = (patternEnd == _pattern.Length);
                string dirSegment = _pattern.Substring(position, patternEnd - position);

                if (!isLastPathSegment) {
                    patternEnd++;
                }

                if (!containsWildcard) {
                    string path = baseDirectory + "/" + dirSegment;
                    TestPath(path, patternEnd, isLastPathSegment);
                    return;
                }

                bool doubleStar = dirSegment.Equals("**");
                if (doubleStar && !isPreviousDoubleStar) {
                    DoGlob(baseDirectory, patternEnd, true);
                }

                foreach (string file in _pal.GetDirectories(baseDirectory, "*").Concat(_pal.GetFiles(baseDirectory, "*"))) {
                    string objectName = _pal.GetFileName(file);
                    if (FnMatch(dirSegment, objectName, _flags)) {
                        TestPath(file, patternEnd, isLastPathSegment);
                        if (doubleStar) {
                            DoGlob(file, position, true);
                        }
                    }
                }
                if (isLastPathSegment && (_flags & Constants.FNM_DOTMATCH) != 0 || dirSegment[0] == '.') {
                    if (FnMatch(dirSegment, ".", _flags)) {
                        string directory = baseDirectory + "/.";
                        if (_dirOnly) {
                            directory += '/';
                        }
                        TestPath(directory, patternEnd, true);
                    }
                    if (FnMatch(dirSegment, "..", _flags)) {
                        string directory = baseDirectory + "/..";
                        if (_dirOnly) {
                            directory += '/';
                        }
                        TestPath(directory, patternEnd, true);
                    }
                }
            }
        }

        public static IEnumerable<string>/*!*/ GlobResults(PlatformAdaptationLayer/*!*/ pal, string/*!*/ pattern, int flags) {
            if (pattern.Length == 0) {
                yield break;
            }
            bool noEscape = ((flags & Constants.FNM_NOESCAPE) != 0);
            string[] groups = UngroupGlobs(pattern, noEscape);
            if (groups.Length == 0) {
                yield break;
            }

            foreach (string group in groups) {
                GlobMatcher matcher = new GlobMatcher(pal, group, flags);
                foreach (string filename in matcher.DoGlob()) {                 
                    yield return filename;
                }
            }
        }

		public static IEnumerable<string> GlobResults(string  pattern) {
			return GlobResults(new PlatformAdaptationLayer(), pattern, 0);
		}
    }

	[Serializable, ExcludeFromCoverage]
	internal class PlatformAdaptationLayer {
		// Fields
		public static readonly PlatformAdaptationLayer Default = new PlatformAdaptationLayer();

		// Methods
		public virtual bool DirectoryExists(string path) {
			return Directory.Exists(path);
		}

		public virtual bool FileExists(string path) {
			return File.Exists(path);
		}

		public virtual string[] GetDirectories(string path, string searchPattern) {
			return Directory.GetDirectories(path, searchPattern);
		}

		public virtual string GetEnvironmentVariable(string key) {
			return Environment.GetEnvironmentVariable(key);
		}

		public virtual IDictionary GetEnvironmentVariables() {
			return Environment.GetEnvironmentVariables();
		}

		public virtual string GetFileName(string file) {
			return Path.GetFileName(file);
		}

		public virtual string[] GetFiles(string path, string searchPattern) {
			return Directory.GetFiles(path, searchPattern);
		}

		public virtual string GetFullPath(string path) {
			string fullPath;
			try {
				fullPath = Path.GetFullPath(path);
			}
			catch (Exception) {
				throw new Exception("Invalid path");
			}
			return fullPath;
		}

		public virtual bool IsAbsolutePath(string path) {
			return (((Environment.OSVersion.Platform != PlatformID.Unix) && Path.GetPathRoot(path).EndsWith(@":\")) ||
					((Environment.OSVersion.Platform == PlatformID.Unix) && Path.IsPathRooted(path)));
		}

		public virtual Assembly LoadAssembly(string name) {
			return Assembly.Load(name);
		}

		public virtual Assembly LoadAssemblyFromPath(string path) {
			return Assembly.LoadFile(path);
		}

		public virtual Stream OpenInputFileStream(string path) {
			return new FileStream(path, FileMode.Open, FileAccess.Read);
		}

		public virtual Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share) {
			return new FileStream(path, mode, access, share);
		}

		public virtual Stream OpenInputFileStream(string path, FileMode mode, FileAccess access, FileShare share,
												  int bufferSize) {
			return new FileStream(path, mode, access, share, bufferSize);
		}

		public virtual Stream OpenOutputFileStream(string path) {
			return new FileStream(path, FileMode.Create, FileAccess.Write);
		}

		public virtual void SetEnvironmentVariable(string key, string value) {
			Environment.SetEnvironmentVariable(key, value);
		}

		public virtual void TerminateScriptExecution(int exitCode) {
			Environment.Exit(exitCode);
		}

		// Properties
		public virtual string CurrentDirectory {
			get { return Environment.CurrentDirectory; }
		}

		public virtual StringComparer PathComparer {
			get { return StringComparer.Ordinal; }
		}
	}
}