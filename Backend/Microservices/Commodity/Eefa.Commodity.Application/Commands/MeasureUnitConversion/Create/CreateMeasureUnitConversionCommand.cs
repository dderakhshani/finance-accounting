using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.MeasureUnitConversion.Create
{
    public class CreateMeasureUnitConversionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<CreateMeasureUnitConversionCommand>, ICommand
    {

        /// <summary>
        /// واحد اندازه گیری اولیه
        /// </summary>
        public int SourceMeasureUnitId { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری ثانویه
        /// </summary>
        public int DestinationMeasureUnitId { get; set; } = default!;

        /// <summary>
        /// ضریب تبدیل
        /// </summary>
        public double? Ratio { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMeasureUnitConversionCommand, Data.Entities.MeasureUnitConversion>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateMeasureUnitConversionCommandHandler : IRequestHandler<CreateMeasureUnitConversionCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.MeasureUnitConversion> _repository;
        private readonly IMapper _mapper;

        public CreateMeasureUnitConversionCommandHandler(IRepository<Data.Entities.MeasureUnitConversion> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        public async Task<ServiceResult> Handle(CreateMeasureUnitConversionCommand request, CancellationToken cancellationToken)
        {
            var measureUnitConversion = _mapper.Map<Data.Entities.MeasureUnitConversion>(request);

            var entity = _repository.Insert(measureUnitConversion);

            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
