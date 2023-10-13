namespace Models.DataTransferObjects.Shared
{
    public class InventoryDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        //public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
        public string Status { get; set; }
    }
}
