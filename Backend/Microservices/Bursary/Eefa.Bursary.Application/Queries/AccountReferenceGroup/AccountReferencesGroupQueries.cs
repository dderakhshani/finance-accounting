using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.AccountReferenceGroup
{
    public class AccountReferencesGroupQueries : IAccountReferencesGroupQueries
    {


       private readonly IRepository<Domain.Entities.AccountReferencesGroups> _accountReferencesGroupRepository;
       private readonly IRepository<Domain.Entities.AccountHeadRelReferenceGroup> _accountRelReferencesGroupRepository;
       private readonly IRepository<Domain.Entities.AccountHead> _accountHeadRepository;
        private readonly IMapper _mapper;

       public AccountReferencesGroupQueries(IMapper mapper, IRepository<Domain.Entities.AccountReferencesGroups> accountReferencesGroupRepository, IRepository<AccountHeadRelReferenceGroup> accountRelReferencesGroupRepository, IRepository<Domain.Entities.AccountHead> accountHeadRepository)
       {
           _mapper = mapper;
           _accountReferencesGroupRepository = accountReferencesGroupRepository;
           _accountRelReferencesGroupRepository = accountRelReferencesGroupRepository;
           _accountHeadRepository = accountHeadRepository;
       }



       public async Task<PagedList<AccountReferencesGroupViewModel>> GetAll(PaginatedQueryModel query)
       {


            try { 
            var referencesGroup = _accountReferencesGroupRepository.GetAll()
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty)
                .ProjectTo<AccountReferencesGroupViewModel>(_mapper.ConfigurationProvider);

                var result = new PagedList<AccountReferencesGroupViewModel>()
                {
                    Data = await referencesGroup.Paginate(query.Paginator()).ToListAsync(),
                    TotalCount = query.PageIndex <= 1 ? await referencesGroup.CountAsync() : 0
                };

                return result;
            }
            catch
            {
                return new PagedList<AccountReferencesGroupViewModel>(); 
               // throw new ValidationError("مشکل پیش آمده ");
            }
            //var referencesGroup = (from ah in _accountHeadRepository.GetAll()
            //                       join arg in _accountRelReferencesGroupRepository.GetAll() on ah.Id equals arg.AccountHeadId
            //                       join rg in _accountReferencesGroupRepository.GetAll() on arg.ReferenceGroupId equals rg.Id
            //                       select new AccountReferencesGroupViewModel
            //                       {
            //                           AccountHeadId = ah.Id,
            //                           Code = rg.Code,
            //                           Id = rg.Id,
            //                           LevelCode = rg.LevelCode,
            //                           Title = rg.Title
            //                       })
            //    .FilterQuery(query.Conditions)
            //    .OrderByMultipleColumns(query.OrderByProperty);




       }

       public async Task<PagedList<AccountReferencesGroupViewModel>> GetReferenceGroupsBy(int id)
       {

           //var referencesGroup = _accountReferencesGroupRepository.GetAll()
           //        .Include(x => x.AccountHeadRelReferenceGroups.Where( x=>x.AccountHeadId == id))
           //        .ProjectTo<AccountReferencesGroupViewModel>(_mapper.ConfigurationProvider);


           var referenceGroups = (from ah in  _accountHeadRepository.GetAll()
               join arg in _accountRelReferencesGroupRepository.GetAll() on ah.Id equals arg.AccountHeadId
               join rg in _accountReferencesGroupRepository.GetAll() on arg.ReferenceGroupId equals  rg.Id
                                  where ah.Id == id
                                  select rg)
               .ProjectTo<AccountReferencesGroupViewModel>(_mapper.ConfigurationProvider);

            var result = new PagedList<AccountReferencesGroupViewModel>()
           {
               Data = await referenceGroups.ToListAsync(),
               TotalCount =   await referenceGroups.CountAsync() 
           };

           return result;
       }


    }
}
