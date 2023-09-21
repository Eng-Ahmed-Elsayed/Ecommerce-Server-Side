namespace Models.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImgPath { get; set; } = null!;
        public Guid? ProductId { get; set; }
    }
}
