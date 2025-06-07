using AutoMapper;
using Infrastructure.Mappings;

namespace $rootnamespace$.$fileinputname$.Model
{
    public class $safeitemname$ : IMapFrom<Data.Entities.$fileinputname$>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.$fileinputname$, $safeitemname$>();
        }
    }
    
}
