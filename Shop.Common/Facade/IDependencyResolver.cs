using System;

namespace Shop.Common.Facade
{
    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;

        object Resolve(Type t);
    }
}