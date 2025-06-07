using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class ResultModel
    {
        public List<VoucherResult> objResult { get; set; }
        public string message { get; set; }
        public bool succeed { get; set; }
        public List<ErrorModel> error { get; set; }
    }

    public class ErrorModel
    { 
     public string propertyName { get; set; }
     public string message {  set; get; }
    }

}
