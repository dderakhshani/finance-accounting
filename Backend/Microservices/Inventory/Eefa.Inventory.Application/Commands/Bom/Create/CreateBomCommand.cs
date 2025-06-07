using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class CreateBomCommand : CommandBase, IRequest<ServiceResult<Bom>>, IMapFrom<CreateBomCommand>, ICommand
    {
        /// <summary>
        /// کد والد
        /// </summary>
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;
        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int CommodityCategoryId { get; set; } = default!;

        public bool IsActive { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBomCommand, Bom>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBomCommandHandler : IRequestHandler<CreateBomCommand, ServiceResult<Bom>>
    {
        private readonly IRepository<Bom> _repository;
        private readonly IMapper _mapper;

        public CreateBomCommandHandler(IRepository<Bom> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult<Bom>> Handle(CreateBomCommand request, CancellationToken cancellationToken)
        {
            var bom = _mapper.Map<Bom>(request);

            var entity = _repository.Insert(bom);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult<Bom>.Success(_mapper.Map<Bom>(bom));
        }
    }
}
