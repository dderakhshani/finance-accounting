using AutoMapper;
using Infrastructure.Data.Entities;
using Infrastructure.Mappings;

namespace $rootnamespace$.$fileinputname$
{
    public class $fileinputname$ServiceModel : BaseEntity , IMapFrom<Identity.Data.Entities.$fileinputname$>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Identity.Data.Entities.$fileinputname$, $fileinputname$ServiceModel>();
        }
    }
}