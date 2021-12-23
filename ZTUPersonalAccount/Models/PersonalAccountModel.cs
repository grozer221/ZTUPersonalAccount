namespace ZTUPersonalAccount.Models
{
    public class PersonalAccountModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Cookie { get; set; }

        public virtual ChatModel Chat { get; set; }
    }
}
