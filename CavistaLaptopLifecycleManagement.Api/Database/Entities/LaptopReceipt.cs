namespace CavistaLaptopLifecycleManagement.Api.Database.Entities
{
    public class LaptopReceipt : BaseEntity
    {
        public Guid LaptopId { get; set; }
        public byte[] Receipt { get; set; }
    }
}
