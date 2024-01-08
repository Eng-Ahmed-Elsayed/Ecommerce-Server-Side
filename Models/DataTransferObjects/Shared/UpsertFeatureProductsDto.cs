namespace Models.DataTransferObjects.Shared
{
    public class UpsertFeatureProductsDto
    {
        public IEnumerable<ProductDto>? FeatureProducts { get; set; }
        public IEnumerable<ProductDto>? OtherProducts { get; set; }
    }
}
