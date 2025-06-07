using AutoMapper;
using Eefa.Bursary.Application.UseCases.Rexp.Models;
using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Add
{
    public class CreateMonthlyForecastCommand : CommandBase, IRequest<ServiceResult<MonthlyForecast>>, IMapFrom<CreateMonthlyForecastCommand>, ICommand
    {

        public int YM { get; set; }
        public List<RexpId_Amount_Model> RexpId_Amount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMonthlyForecastCommand, MonthlyForecast>().IgnoreAllNonExisting();
        }

    }

    public class CreateMonthlyForecastCommandHandler : IRequestHandler<CreateMonthlyForecastCommand, ServiceResult<MonthlyForecast>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public CreateMonthlyForecastCommandHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<MonthlyForecast>> Handle(CreateMonthlyForecastCommand request, CancellationToken cancellationToken)
        {
            MonthlyForecast mf = new MonthlyForecast();
            foreach (var itm in request.RexpId_Amount)
            {
                var item = new MonthlyForecast();
                item.YM = request.YM;
                item.RexpId = itm.RexpId;
                item.Amount = itm.Amount;
                _uow.MonthlyForecast.Add(item);
                mf = item;
            }

            var value = await _uow.SaveChangesAsync(cancellationToken);
            if (value <= 0)
                throw new Exception("بروز خطا در ثبت اطلاعات پیش بینی ماهانه منابع و مصارف");
            return ServiceResult<MonthlyForecast>.Success(mf);
        }
    }

}
