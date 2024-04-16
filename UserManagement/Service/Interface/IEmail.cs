namespace UserManagement.Interface
{
    public interface IEmail
    {
        bool SendEmail(string to, string subject, string htmlMessage);

    }
}
