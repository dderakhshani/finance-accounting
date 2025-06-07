using AutoMapper;
using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Update
{
    public class UpdateMonthlyForecastCommand : CommandBase, IRequest<ServiceResult<MonthlyForecast>>, IMapFrom<UpdateMonthlyForecastCommand>, ICommand
    {
        public int Id { get; set; }
        public int YM { get; set; }
        public int RexpId { get; set; }
        public long Amount { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateMonthlyForecastCommand, MonthlyForecast>().IgnoreAllNonExisting();
        }
    }

    public class UpdateMonthlyForecastCommandHandler : IRequestHandler<UpdateMonthlyForecastCommand, ServiceResult<MonthlyForecast>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public UpdateMonthlyForecastCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<MonthlyForecast>> Handle(UpdateMonthlyForecastCommand request, CancellationToken cancellationToken)
        {
            var mf = await _uow.MonthlyForecast.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (mf == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }
            _mapper.Map(request, mf);

            _uow.MonthlyForecast.Update(mf);
            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات بانک");
            return ServiceResult<MonthlyForecast>.Success(mf);
        }

    }
}
