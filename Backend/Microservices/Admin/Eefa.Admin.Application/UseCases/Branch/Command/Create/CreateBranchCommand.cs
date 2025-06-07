using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;


namespace Eefa.Admin.Application.CommandQueries.Branch.Command.Create
{
    public class CreateBranchCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBranchCommand>, ICommand
    {
        /// <summary>
        /// 
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBranchCommand, Data.Databases.Entities.Branch>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateBranchCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var input = _mapper.Map<Data.Databases.Entities.Branch>(request);
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.Branch>(input));
            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
