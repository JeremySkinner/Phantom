namespace Phantom.Core {
	public interface IDslFactory {
		bool CanGenerateDsl(string path);
		ScriptModel GenerateScriptModel(string path);
	}
}