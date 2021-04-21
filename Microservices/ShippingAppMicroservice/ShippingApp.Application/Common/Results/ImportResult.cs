using System.Collections.Generic;

namespace ShippingApp.Application.Common.Results
{
    public class ImportResult
    {
        internal ImportResult(bool isValid, List<string> errors, string dataKey, dynamic data)
        {
            IsValid = isValid;
            Errors = errors;
            Data = data;
            DataKey = dataKey;
        }

        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public string DataKey { get; set; }
        public dynamic Data { get; set; }

        public static ImportResult Success()
        {
            return new ImportResult(true, new List<string> { }, string.Empty, null);
        }

        public static ImportResult Success(string dataKey, dynamic data)
        {
            return new ImportResult(true, new List<string> { }, dataKey, data);
        }

        public static ImportResult Failure(List<string> errors)
        {
            return new ImportResult(false, errors, null, null);
        }

        public static ImportResult Failure(List<string> errors, string dataKey, dynamic data)
        {
            return new ImportResult(false, errors, dataKey, data);
        }
    }
}
