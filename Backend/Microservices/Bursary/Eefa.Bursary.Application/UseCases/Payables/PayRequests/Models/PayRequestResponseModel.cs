using AutoMapper;
using Eefa.Bursary.Application.UseCases.Payables.PayOrders.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using System;

namespace Eefa.Bursary.Application.UseCases.Payables.PayRequests.Models
{
    public class PayRequestResponseModel : IMapFrom<Payables_PayRequests_View>
    {
        public int Id { get; set; }
        public int PayOrderId { get; set; }
        public DateTime PayRequestDate { get; set; }
        public string PayRequestNo { get; set; }
        public long PayRequestAmount { get; set; }
        public string PayRequestDescp { get; set; }
        public int PayRequestAccountId { get; set; }
        public string PayRequestAccountName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_PayRequests_View, PayRequestResponseModel>().IgnoreAllNonExisting();
        }

    }
}
