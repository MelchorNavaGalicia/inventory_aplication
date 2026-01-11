namespace inventory_aplication.Application.Features.Common.Results
{
    public class Result<T>
    {
        public bool Success { get; init; }          // true/false
        public T? Data { get; init; }               // resultado útil
        public string? Error { get; init; }         // mensaje para usuario
        public string? Code { get; init; }          // código técnico

        // Ok
        public static Result<T> Ok(T data) =>
            new Result<T> { Success = true, Data = data };

        // Fail
        public static Result<T> Fail(string error, string code) =>
            new Result<T> { Success = false, Error = error, Code = code };
    }
    public class Result
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public string? Code { get; init; }

        public static Result Ok() => new Result { Success = true };
        public static Result Fail(string error, string code) =>
            new Result { Success = false, Error = error, Code = code };
    }
}
