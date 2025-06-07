using Microsoft.AspNetCore.Http;

namespace Eefa.Sale.Application.Common.ImportModels
{
    public class ImportPayload
    {
        public IFormFile File { get; set; }
    }
}
