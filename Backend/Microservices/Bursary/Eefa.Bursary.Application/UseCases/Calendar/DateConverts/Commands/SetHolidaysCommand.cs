using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Calendar.DateConverts.Commands
{
    public class SetHolidaysCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int[] Ids { get; set; }
        public bool IsHoliday { get; set; }
    }
    public class SetHolidaysCommandHandler : IRequestHandler<SetHolidaysCommand, ServiceResult>
    {
        private readonly IBursaryUnitOfWork _uow;

        public SetHolidaysCommandHandler(IBursaryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ServiceResult> Handle(SetHolidaysCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Ids.Length == 0)
                    throw new ValidationError("اطلاعات ورودی ارسال نگردیده است");
                foreach (var item in request.Ids)
                {
                    var entity = await _uow.DateConversion.FirstOrDefaultAsync(w => w.Id == item);
                    if (entity != null)
                    {
                        entity.IsHoliday = request.IsHoliday;
                        _uow.DateConversion.Update(entity);
                    }

                    var value = await _uow.SaveChangesAsync(cancellationToken);
                    if (value <= 0)
                        throw new Exception("بروز خطا در ثبت اطلاعات تعطیلات");
                }
                return ServiceResult.Success();
            }
            catch(Exception ex)
            {

            }
            return null;
        }
    }
}
