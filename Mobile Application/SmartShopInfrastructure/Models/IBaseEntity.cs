namespace SmartShopInfrastructure.Models
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        int Id { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}