namespace UserManagement.Dto
{
    public class ResponceBody<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public String Message { get; set; }

    }
}
