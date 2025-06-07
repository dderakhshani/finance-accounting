using AutoMapper;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using System;

namespace Eefa.Bursary.Application.UseCases.Payables.PayOrders.Models
{
    public class PayOrderResponseModel : IMapFrom<Payables_PayOrders_View>
    {
        public int Id { get; set; }
        public DateTime PayOrderDate { get; set; }
        public string PayOrderNo { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
        public long PayOrderAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_PayOrders_View, PayOrderResponseModel>().IgnoreAllNonExisting();
        }

    }
}
