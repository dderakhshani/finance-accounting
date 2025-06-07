using AutoMapper;
using Eefa.Common.CommandQuery;
using Eefa.Common;
using Eefa.Common.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Commands.MeasureUnit.Update
{
    public class UpdateMeasureUnitCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<Data.Entities.MeasureUnit>, ICommand
    {
        public int Id { get; set; }

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
            profile.CreateMap<UpdateMeasureUnitCommand, Eefa.Commodity.Data.Entities.MeasureUnit>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateMeasureUnitCommandHandler : IRequestHandler<UpdateMeasureUnitCommand, ServiceResult>
    {
        private readonly IRepository<Data.Entities.MeasureUnit> _repository;
        private readonly IMapper _mapper;

        public UpdateMeasureUnitCommandHandler(IRepository<Data.Entities.MeasureUnit> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(UpdateMeasureUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.Find(request.Id);

            _mapper.Map<UpdateMeasureUnitCommand, Data.Entities.MeasureUnit>(request, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
