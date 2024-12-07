namespace Practice.Service.EmployeeAPI.Model.Dto
{
    public class ResponseDto
    {
        public string Message { get; set; }
        public bool isSuccess { get; set; }
        public object? Result { get; set; }
    }
}
