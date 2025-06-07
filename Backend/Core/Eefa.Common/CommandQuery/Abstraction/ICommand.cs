using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.Validation.Resources;
using Microsoft.AspNetCore.Hosting;

namespace Eefa.Common.CommandQuery
{
    public interface ICommand
    {
        Task<Dictionary<string, List<string>>> Validate<T>(T command,ICurrentUserAccessor currentUserAccessor,
            IWebHostEnvironment hostingEnvironment, IValidationFactory validationFactory ) where T : ICommand;

    }
}