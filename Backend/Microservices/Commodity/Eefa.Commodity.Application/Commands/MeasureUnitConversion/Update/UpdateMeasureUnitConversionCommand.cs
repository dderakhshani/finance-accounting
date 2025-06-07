using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.MeasureUnitConversion.Update
{
    public class UpdateMeasureUnitConversionCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Eefa.Commodity.Data.Entities.MeasureUnitConversion>, ICommand
    {
        public int Id { get; set; }

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
            profile.CreateMap<UpdateMeasureUnitConversionCommand, Data.Entities.MeasureUnitConversion>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateMeasureUnitConversionCommandHandler : IRequestHandler<UpdateMeasureUnitConversionCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.MeasureUnitConversion> _repository;
        private readonly IMapper _mapper;

        public UpdateMeasureUnitConversionCommandHandler(IRepository<Data.Entities.MeasureUnitConversion> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateMeasureUnitConversionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateMeasureUnitConversionCommand, Data.Entities.MeasureUnitConversion>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();

        }
    }
}
