using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.MeasureUnit.Create
{
    public class CreateMeasureUnitCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateMeasureUnitCommand>, ICommand
    {
        public int? ParentId { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMeasureUnitCommand, Data.Entities.MeasureUnit>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateMeasureUnitCommandHandler : IRequestHandler<CreateMeasureUnitCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.MeasureUnit> _repository;
        private readonly IMapper _mapper;

        public CreateMeasureUnitCommandHandler(IRepository<Data.Entities.MeasureUnit> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateMeasureUnitCommand request, CancellationToken cancellationToken)
        {
            var measureUnit = _mapper.Map<Data.Entities.MeasureUnit>(request);

            var entity = _repository.Insert(measureUnit);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
