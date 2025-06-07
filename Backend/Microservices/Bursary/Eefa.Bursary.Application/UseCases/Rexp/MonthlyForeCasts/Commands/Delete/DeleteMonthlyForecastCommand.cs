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

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Commands.Delete
{
    public class DeleteMonthlyForecastCommand : CommandBase, IRequest<ServiceResult<MonthlyForecast>>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteMonthlyForecastCommandHandler : IRequestHandler<DeleteMonthlyForecastCommand, ServiceResult<MonthlyForecast>>
    {
        private readonly IBursaryUnitOfWork _uow;
        protected readonly ICurrentUserAccessor _currentUserAccessor;

        public async Task<ServiceResult<MonthlyForecast>> Handle(DeleteMonthlyForecastCommand request, CancellationToken cancellationToken)
        {
            var mfc = await _uow.MonthlyForecast.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (mfc == null)
            {
                throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
            }

            mfc.IsDeleted = true;
            mfc.ModifiedAt = DateTime.UtcNow;
            mfc.ModifiedById = _currentUserAccessor.GetId();
            _uow.MonthlyForecast.Update(mfc);

            var value = await _uow.SaveChangesAsync(cancellationToken);

            if (value <= 0)
                throw new Exception("بروز خطا در حذف اطلاعات پیش بینی ماه");
            return ServiceResult<MonthlyForecast>.Success(mfc);
        }
    }

}
