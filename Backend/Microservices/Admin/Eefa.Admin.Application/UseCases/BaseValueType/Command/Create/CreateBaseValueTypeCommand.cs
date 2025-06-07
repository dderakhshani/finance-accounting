using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;


namespace Eefa.Admin.Application.CommandQueries.BaseValueType.Command.Create
{
    public class CreateBaseValueTypeCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateBaseValueTypeCommand>, ICommand
    {
        public int? ParentId { get; set; }

        /// <summary>
        /// ?????
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// ??? ???????
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// ??? ????
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// ??? ??? ???? ?????? ????
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        /// <summary>
        /// ??? ?????
        /// </summary>
        public string? SubSystem { get; set; }

        public string LevelCode { get; set;} = "0";

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBaseValueTypeCommand, Data.Databases.Entities.BaseValueType>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBaseValueTypeCommandHandler : IRequestHandler<CreateBaseValueTypeCommand, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public CreateBaseValueTypeCommandHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateBaseValueTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = _repository.Insert(_mapper.Map<Data.Databases.Entities.BaseValueType>(request));
          

            return await request.Save(_repository, entity.Entity, cancellationToken);

        }
    }
}
