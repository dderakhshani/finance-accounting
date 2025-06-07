using Newtonsoft.Json;
using System.Collections.Generic;

public class ServiceResult
{
    public static ServiceResult<T> Success<T>(T data = default, string message = DefaultMessages.SuccessMessage) => new ServiceResult<T>(data, true, message);
    public static ServiceResult<T> Failure<T>(T data = default, string message = DefaultMessages.FailureMessage, List<ApplicationErrorModel> errors = default) => new ServiceResult<T>(data, true, message, errors);
}
public class ServiceResult<T>
{
    public ServiceResult(T data, bool succeed, string message, List<ApplicationErrorModel> errors = default)
    {
        this.ObjResult = data;
        this.Message = message;
        this.Errors = errors;
        this.Succeed = succeed;
    }
    [JsonProperty(PropertyName = "objResult")]
    public T ObjResult { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    [JsonProperty(PropertyName = "errors")]
    public List<ApplicationErrorModel> Errors { get; set; }

    [JsonProperty(PropertyName = "succeed")]
    public bool Succeed { get; set; } = false;
}

public class DefaultMessages
{
    public const string SuccessMessage = "عملیات موفقیت آمیز بود";
    public const string FailureMessage = "خطا در انجام عملیات";
}