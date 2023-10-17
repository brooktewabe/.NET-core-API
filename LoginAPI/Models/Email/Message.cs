using MimeKit;

namespace LoginAPI.Models.Email
{
    public class Message
    {
        public MailboxAddress To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(string to, string subject, string content)
        {
            To = new MailboxAddress("receiver", to);
            Subject = subject;
            Content = content;
        }
    }
}
