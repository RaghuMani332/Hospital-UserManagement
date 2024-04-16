namespace UserManagement.CustomException
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException() { }
        public DuplicateEntryException(string message) : base(message) { }
    }
}
