using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;
using System;

namespace Eefa.Inventory.Application
{

    public  class QuotaGroupModel : IMapFrom<QuotaGroup>
    {
        public void Mapping(Profile profile)
        {
            
            profile.CreateMap<QuotaGroup, QuotaGroupModel>().ForMember(src => src.Title,
                    opt => opt.MapFrom(dest => dest.QuotaGroupName));
        }
        public int Id { get; set; }
        public int ErpQuotaGroupId { get; set; }
        public string QuotaGroupName { get; set; }
        public string Title { get; set; }
        public string UnitsTitle { get; set; }
        public Nullable<int> UnitId { get; set; }

    }
   
}
