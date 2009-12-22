namespace Phantom.Core.Integration {
    public interface IConstructionAware<T> {
        T Constructed();
    }
}
