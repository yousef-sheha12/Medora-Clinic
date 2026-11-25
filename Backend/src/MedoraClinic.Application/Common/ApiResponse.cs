namespace MedoraClinic.Application.Common
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; init; }
        public bool Succeeded { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
        public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();

        public static ApiResponse<T> Success(
            T? data = default,
            string message = "تمت العملية بنجاح.",
            int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Succeeded = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Failure(
            string message,
            IEnumerable<string>? errors = null,
            int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Succeeded = false,
                Message = message,
                Errors = errors?.ToArray() ?? Array.Empty<string>()
            };
        }
    }
}
