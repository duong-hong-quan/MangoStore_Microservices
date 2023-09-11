namespace Mango.Services.ProductAPI.Models.Dto
{
    public class ResponeDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

    }
}
