namespace Phantom.Core.Language {
    // TResult should generally be equal to the type that is implementing the interface
    // but is not limited to this.
    public interface IRunnable<TResult> {
        TResult Run();
    }
}


