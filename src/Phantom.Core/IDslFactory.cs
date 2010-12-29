namespace Phantom.Core {
	using System;

	public interface IDslFactory {
		bool CanExecute(string path);
		ScriptModel BuildModel(string path);
	}
}