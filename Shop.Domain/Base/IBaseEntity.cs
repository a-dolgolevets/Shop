namespace Shop.Domain.Base
{
    // Interface is needed for Identity models
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
}