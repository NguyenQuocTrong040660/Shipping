namespace Files.Domain.Models
{
    public class Result
    {
        internal Result(bool succeeded, string error, dynamic data)
        {
            Succeeded = succeeded;
            Error = error;
            Data = data;
        }

        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public dynamic Data { get; set; }

        public static Result Success()
        {
            return new Result(true, string.Empty, null);
        }

        public static Result SuccessWithData(dynamic data)
        {
            return new Result(true, string.Empty, data);
        }

        public static Result Failure(string error)
        {
            return new Result(false, error, null);
        }
    }
}
