namespace UserManagement.CustomException
{
    public class PasswordMissMatchException : Exception
    {
        public PasswordMissMatchException() { }
        public PasswordMissMatchException(string message) : base(message) { }
    }
}
