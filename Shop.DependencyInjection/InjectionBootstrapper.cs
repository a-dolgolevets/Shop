using Shop.Common.Facade;
using Shop.DependencyInjection.Config;
using Shop.DependencyInjection.SimpleInjector;
using SimpleInjector;

namespace Shop.DependencyInjection
{
    public static class InjectionBootstrapper
    {
        public static void Configure(InjectionConfig config)
        {
            ContainerProvider.Configure(config);
        }

        public static Container GetContainer()
        {
            return ContainerProvider.GetContainer();
        }
    }
}