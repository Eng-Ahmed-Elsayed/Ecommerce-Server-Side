namespace Models.DataTransferObjects.Customer
{
    public class CheckListDto
    {
        public Guid? Id { get; set; }
        public string? UserId { get; set; }
        public List<CheckListItemDto>? CheckListItems { get; set; }
    }
}
