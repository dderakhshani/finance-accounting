using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities.Calendar;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Calendar.DateConverts.Queries
{
    public class GetDateQuery : IRequest<ServiceResult<DateConversion>>, IQuery
    {
        public int? Id { get; set; }
        public int? PersianDate { get; set; }
        public DateTime? GreDate { get; set; }
    }
    public class GetDateQueryHandler : IRequestHandler<GetDateQuery, ServiceResult<DateConversion>>
    {
        private readonly IBursaryUnitOfWork _uow;

        public GetDateQueryHandler(IBursaryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ServiceResult<DateConversion>> Handle(GetDateQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == null && request.PersianDate == null && request.GreDate == null)
                throw new ValidationError("فیلتر ورودی ارسال نگردیده است");

            var qry = _uow.DateConversion.AsQueryable().AsNoTracking();
            if (request.Id != null && request.Id > 0)
                qry = qry.Where(x => x.Id == request.Id);
            if (request.PersianDate != null && request.PersianDate > 0)
                qry = qry.Where(x => x.PersianDate == request.PersianDate);
            if (request.GreDate != null && request.GreDate > new DateTime(2020, 1, 1))
            {
                var d = request.GreDate.Value;
                var d1 = d.Year.ToString() + d.Month.ToString("D2") + d.Day.ToString("D2");
                qry = qry.Where(x => x.GreDate == Convert.ToUInt32(d1));
            }
            var r = await qry.FirstOrDefaultAsync();
            return ServiceResult<DateConversion>.Success(r);
        }
    }

}
