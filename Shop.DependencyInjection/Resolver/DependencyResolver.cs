using System;
using Shop.Common.Facade;
using SimpleInjector;

namespace Shop.DependencyInjection.Resolver
{
    internal class DependencyResolver : IDependencyResolver
    {
        private readonly Container container;

        public DependencyResolver(Container container)
        {
            this.container = container;
        }

        public T Resolve<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        public object Resolve(Type t)
        {
            return container.GetInstance(t);
        }
    }
}