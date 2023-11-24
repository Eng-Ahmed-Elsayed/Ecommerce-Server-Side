using Models.DataTransferObjects.Shared;

namespace Models.DataTransferObjects.Customer
{
    public class CheckListItemDto
    {
        public Guid? Id { get; set; }
        public Guid? CheckListId { get; set; }
        public Guid? ProductId { get; set; }
        public ProductDto? Product { get; set; }
    }
}
