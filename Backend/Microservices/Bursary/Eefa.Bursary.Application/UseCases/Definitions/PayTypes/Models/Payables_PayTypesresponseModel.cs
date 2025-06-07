using AutoMapper;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Models
{
    public class Payables_PayTypesResponseModel:IMapFrom<Payables_PayTypes_View>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Payables_PayTypes_View, Payables_PayTypesResponseModel>();
        }

    }
}
