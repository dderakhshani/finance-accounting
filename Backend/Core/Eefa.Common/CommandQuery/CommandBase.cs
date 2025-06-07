using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.Validation;
using Eefa.Common.Validation.Resources;
using Microsoft.AspNetCore.Hosting;

namespace Eefa.Common.CommandQuery
{
    public abstract class CommandBase : ICommand
    {
        public CommandState State { get; set; }
        public async Task<Dictionary<string, List<string>>> Validate<T>(T command,
            ICurrentUserAccessor currentUserAccessor, 
            IWebHostEnvironment hostingEnvironment,
            IValidationFactory validationFactory
        ) where T : ICommand
        {

            var exceptions = await Utility.Validate<T>(command, currentUserAccessor, hostingEnvironment, validationFactory);
            return exceptions;
        }

       
    }


    public enum CommandState
    {
        Unchanged,
        Inserted,
        Modified,
        Deleted
    }
}