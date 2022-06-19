using ZDToolbox.Helpers;

namespace ZDToolbox.Classes
{
    public class ApiResult
    {
        public ApiResult()
        {
            _errors = new List<string>();

            _successes = new List<string>();
        }

        public bool IsFailed { get; set; }

        public bool IsSuccess { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<string> _errors;

        public IReadOnlyList<string> Errors => _errors;

        [System.Text.Json.Serialization.JsonIgnore]
        private readonly List<string> _successes;

        public IReadOnlyList<string> Successes => _successes;

        public void AddErrorMessage(string message)
        {
            message = message.Fix();

            if (message == null)
            {
                return;
            }

            if (_errors.Contains(message))
            {
                return;
            }

            _errors.Add(message);
        }

        public void AddSuccessMessage(string message)
        {
            message =
                message.Fix();

            if (message == null)
            {
                return;
            }

            if (_successes.Contains(message))
            {
                return;
            }

            _successes.Add(message);
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T Value { get; set; }
    }

    public class PagedApiResult<T> : ApiResult
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }

    //public static class ResultExtensions
    //{
    //    static ResultExtensions()
    //    {
    //    }

    //    public static Result ConvertToDtxResult(this FluentResults.Result result)
    //    {
    //        Result dtxResult = new()
    //        {
    //            IsFailed = result.IsFailed,
    //            IsSuccess = result.IsSuccess
    //        };

    //        if (result.Errors != null)
    //        {
    //            foreach (var item in result.Errors)
    //            {
    //                dtxResult.AddErrorMessage(message: item.Message);
    //            }
    //        }

    //        if (result.Successes != null)
    //        {
    //            foreach (var item in result.Successes)
    //            {
    //                dtxResult.AddSuccessMessage(message: item.Message);
    //            }
    //        }

    //        return dtxResult;
    //    }

    //    public static Result<T> ConvertToDtxResult<T>(this FluentResults.Result<T> result)
    //    {
    //        Result<T> dtxResult = new()
    //        {
    //            IsFailed = result.IsFailed,
    //            IsSuccess = result.IsSuccess
    //        };

    //        if (result.IsSuccess)
    //        {
    //            dtxResult.Value = result.Value;
    //        }

    //        if (result.Errors != null)
    //        {
    //            foreach (var item in result.Errors)
    //            {
    //                dtxResult.AddErrorMessage(message: item.Message);
    //            }
    //        }

    //        if (result.Successes != null)
    //        {
    //            foreach (var item in result.Successes)
    //            {
    //                dtxResult.AddSuccessMessage(message: item.Message);
    //            }
    //        }

    //        return dtxResult;
    //    }
    //}



    public static class ApiResultFactory
    {
        public static ApiResult<T> CreateSuccessResult<T>(T t, string message = "", string field = "")
        {
            var result = new ApiResult<T> { Value = t, IsSuccess = true };
            result.AddSuccessMessage(MakeMessage(Resources.Messages.Successes.SuccessGeneral, message, field));
            return result;
        }

        public static ApiResult<T> CreateErrorResult<T>(IEnumerable<string> messages)
        {
            var result = new ApiResult<T>() { IsFailed = true };
            foreach (var message in messages)
                result.AddErrorMessage(message);
            return result;
        }

        public static ApiResult<T> CreateErrorResult<T>(string message, string field = "")
        {
            var result = new ApiResult<T>() { IsFailed = true };
            result.AddErrorMessage(MakeMessage(Resources.Messages.Errors.UnexpectedError, message, field));
            return result;
        }

        public static PagedResult<T> CreateSuccessPagedResult<T>(IEnumerable<T> items, int count, int pageNumber, int pageSize, string message = "", string field = "")
        {
            var result = new PagedResult<T>(items, count, pageNumber, pageSize) { IsSuccess = true };
            result.AddSuccessMessage(MakeMessage(Resources.Messages.Successes.SuccessGeneral, message, field));
            return result;
        }

        public static PagedResult<T> CreateErrorPagedResult<T>(int pageNumber, int pageSize, string message, string field = "")
        {
            var result = new PagedResult<T>(null, 0, pageNumber, pageSize) { IsFailed = true };
            result.AddErrorMessage(MakeMessage(Resources.Messages.Errors.UnexpectedError, message, field));
            return result;
        }

        private static string MakeMessage(string defaultMessage, string message, string field)
        {
            try
            {
                return string.IsNullOrWhiteSpace(message) switch
                {
                    false when !string.IsNullOrWhiteSpace(field) => string.Format(message, field),
                    false when string.IsNullOrWhiteSpace(field) => message,
                    _ => defaultMessage
                };
            }
            catch
            {
                return defaultMessage;
            }
        }

    }
}