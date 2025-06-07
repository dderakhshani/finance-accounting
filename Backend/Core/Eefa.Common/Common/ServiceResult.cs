using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eefa.Common
{

    public class DefaultMessages
    {
        public const string SuccessMessage = "عملیات موفقیت آمیز بود";
        public const string FailureMessage = "خطا در انجام عملیات";
    }


    public class ServiceResult : ServiceResult<dynamic>
    {
        private ServiceResult(bool succeed, dynamic data = default, IDictionary<string, List<string>> exceptions = null)
        {
            this.ObjResult = data;
            this.Succeed = succeed;
            this.Exceptions = exceptions;
        }
        public static ServiceResult Success() => new(true);
        public static ServiceResult Failed(IDictionary<string, List<string>> exceptions = default) => new(false, default, exceptions);
    }

    public class ServiceResult<T>
    {
 
        public static ServiceResult<T> Success(T data) => new(true, data);
        public static ServiceResult<T> Failed() => new(false, default);

        private T _objResult;
        public bool Succeed { get; set; } = false;
        public IDictionary<string, List<string>> Exceptions { get; set; }

        public T ObjResult
        {
            get => _objResult;
            set
            {
                if (value is not null) Succeed = true;
                _objResult = value;
            }
        }

        private protected ServiceResult(bool succeed, T data = default, IDictionary<string, List<string>> exceptions = null)
        {
            Succeed = succeed;
            ObjResult = data;
            Exceptions = exceptions;
        }

        private protected ServiceResult()
        {
            Succeed = false;
            ObjResult = default;
            Exceptions = new Dictionary<string, List<string>>();
        }

        
    }
}