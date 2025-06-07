using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Library.Resources;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Library.Interfaces
{
    public interface ICommand
    {
        Task<Dictionary<string, List<string>>> Validate<T>(T command,IRepository repository,ICurrentUserAccessor currentUserAccessor, IMediator mediator, IWebHostEnvironment hostingEnvironment, IResourceFactory validationFactory ,IMapper mapper) where T : ICommand;


        public bool SaveChanges { get; set; }
        public int MenueId { get; set; }
    }

}