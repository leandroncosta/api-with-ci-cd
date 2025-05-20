namespace RelatoX.Application.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; } = default;
        public List<string>? Errors { get; set; } = null;

        public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data, Errors = null };

        public static ApiResponse<T> Fail(params string[] errors) => new() { Success = false, Errors = errors.ToList(), Data = default };
    }
}