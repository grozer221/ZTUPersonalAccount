namespace ZTUPersonalAccount.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; } = Role.User;

        public string GroupName { get; set; }
        public int SubGroupNumber { get; set; } = 1;

        public bool Subscribed { get; set; } = true;
        public int MinutesBeforeLessonNotification { get; set; } = 10;
        public int MinutesBeforeLessonsNotification { get; set; } = 60;

        public int PersonalAccountId { get; set; }
        public virtual PersonalAccountModel PersonalAccount { get; set; }
    }
}
