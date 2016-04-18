using AutoMapper;
using Shop.Common.Facade;
using Shop.Domain.Entities.Shop;
using Shop.Domain.Enums;
using Shop.ViewModel.Shop;
using IMapper = Shop.Common.Facade.IMapper;

namespace Shop.Mapping
{
    public class Mapper : IMapper
    {
        private readonly AutoMapper.IMapper mapper;

        public Mapper(IDependencyResolver resolver)
        {
            var config = new MapperConfiguration(x => Configure(x, resolver));
            mapper = config.CreateMapper();
        }

        private static void Configure(IMapperConfiguration config, IDependencyResolver resolver)
        {
            config.ConstructServicesUsing(resolver.Resolve);

            config.CreateMap<CartViewModel, Cart>()
                .ForMember(x => x.CustomerType, opt => opt.MapFrom(vm => vm.IsPersistent ? CustomerType.Registered : CustomerType.Anonymous));
            config.CreateMap<Cart, CartViewModel>()
                .ForMember(x => x.IsPersistent, opt => opt.MapFrom(vm => vm.CustomerType == CustomerType.Registered));

            config.CreateMap<CartItemViewModel, CartItem>();
            config.CreateMap<CartItem, CartItemViewModel>();

            config.CreateMap<ProductCategory, ProductCategoryViewModel>();
            config.CreateMap<ProductCategoryViewModel, ProductCategory>();

            config.CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.CategoryTitle, opt => opt.MapFrom(e => e.Category.Title));
            config.CreateMap<Product, CartItemViewModel>()
                .ForMember(x => x.ProductId, opt => opt.MapFrom(e => e.Id))
                .ForMember(x => x.Id, opt => opt.MapFrom(e => 0));
            config.CreateMap<ProductViewModel, Product>();

            config.CreateMap<Order, OrderViewModel>();
            config.CreateMap<OrderViewModel, Order>();
            config.CreateMap<CartItemViewModel, OrderItem>();
        }

        #region IMapper implementation
        public TDest Map<TDest>(object source)
        {
            return mapper.Map<TDest>(source);
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            return mapper.Map<TSource, TDest>(source);
        }

        public TDest Map<TSource, TDest>(TSource source, TDest dest)
        {
            return mapper.Map(source, dest);
        }
        #endregion
    }
}