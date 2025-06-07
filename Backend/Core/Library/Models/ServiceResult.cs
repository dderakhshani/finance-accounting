using Library.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Library.Models
{
    public class ServiceResult
    {
        public static ServiceResult Success(object data = null, string message = DefaultMessages.SuccessMessage) => new ServiceResult { ObjResult = data, Message = message, Succeed = true };
        public static ServiceResult Failure(object data = null, string message = DefaultMessages.FailureMessage, List<ApplicationErrorModel> errors = default) => new ServiceResult { ObjResult = data, Message = message, Succeed = false, Errors = errors };

        [JsonProperty(PropertyName = "objResult")]
        public object ObjResult { get; set; }

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
}