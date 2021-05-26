namespace UserManagement.Models
{
    public class CustomToken
    {
        public int Id { get; set; }
        public long Expiry { get; set; }
        public string Owner { get; set; }
    }
}