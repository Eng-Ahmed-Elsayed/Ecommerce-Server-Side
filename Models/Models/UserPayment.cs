namespace Models.Models
{
    public class UserPayment
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string PaymentType { get; set; }
        public string Provider { get; set; }
        public int AccountNo { get; set; }
        public DateTime Expiry { get; set; }
    }
}
