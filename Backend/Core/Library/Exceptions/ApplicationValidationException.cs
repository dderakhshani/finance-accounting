using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Exceptions
{
    public class ApplicationValidationException : Exception
    {
        public List<ApplicationErrorModel> Errors { get; set; } = new List<ApplicationErrorModel>();
        public ApplicationValidationException(List<ApplicationErrorModel> errors)
        {
            this.Errors = errors;
        }
        public ApplicationValidationException(ApplicationErrorModel error)
        {
            this.Errors.Add(error);
        }
    }
}
