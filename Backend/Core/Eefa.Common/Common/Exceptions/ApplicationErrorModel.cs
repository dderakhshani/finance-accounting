using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Common.Common.Exceptions
{
    public class ApplicationErrorModel
    {

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
