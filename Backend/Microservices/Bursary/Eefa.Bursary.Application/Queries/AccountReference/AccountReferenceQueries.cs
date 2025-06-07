using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eefa.Bursary.Domain.Entities;

namespace Eefa.Bursary.Application.Queries.AccountReference
{
    public class AccountReferenceQueries : IAccountReferenceQueries
    {

        private readonly IRepository<Domain.Entities.AccountReferences> _accountReferenceRepository;
        private readonly IRepository<Domain.Entities.AccountReferencesGroups> _accountReferenceGroupRepository;
        private readonly IRepository<Domain.Entities.AccountReferencesRelReferencesGroups> _accountReferenceRelReferenceGroupRepository;

        private readonly IMapper _mapper;

        public AccountReferenceQueries( IMapper mapper, IRepository<Domain.Entities.AccountReferences> accountReferenceRepository, IRepository<AccountReferencesGroups> accountReferenceGroupRepository, IRepository<AccountReferencesRelReferencesGroups> accountReferenceRelReferenceGroupRepository)
        {
            _mapper = mapper;
            _accountReferenceRepository = accountReferenceRepository;
            _accountReferenceGroupRepository = accountReferenceGroupRepository;
            _accountReferenceRelReferenceGroupRepository = accountReferenceRelReferenceGroupRepository;
        }

        public async Task<PagedList<AccountReferenceModel>> GetAll(PaginatedQueryModel query)
        {
            var accountReferences = _accountReferenceRepository.GetAll()
                .ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty);

            //var accountReferences = (from arg in _accountReferenceGroupRepository.GetAll()
            //    join arrg in _accountReferenceRelReferenceGroupRepository.GetAll() on arg.Id equals arrg.ReferenceGroupId
            //    join ar in _accountReferenceRepository.GetAll() on arrg.ReferenceId equals ar.Id
              
            //    select new AccountReferenceModel
            //    {
            //        Code = ar.Code,
            //        Id = ar.Id,
            //        Title = ar.Title,
            //        AccountGroupId = arg.Id
            //    })
            //     .FilterQuery(query.Conditions)
            //     .OrderByMultipleColumns(query.OrderByProperty);

             var result = new PagedList<AccountReferenceModel>()
            {
                Data = await accountReferences.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1 ? await accountReferences.CountAsync(): 0
            };

            return result;
        }

        public async Task<AccountReferenceModel> GetById(int id)
        {
            var accountReference = await _accountReferenceRepository.Find(id);
            return _mapper.Map<AccountReferenceModel>(accountReference);
        }

        public async Task<PagedList<AccountReferenceModel>> ReferenceAccountsByGroupId(int id)
        {

            var accountReferences = (from arg in _accountReferenceGroupRepository.GetAll()
                    join arrg in _accountReferenceRelReferenceGroupRepository.GetAll() on arg.Id equals arrg.ReferenceGroupId
                    join ar in _accountReferenceRepository.GetAll() on arrg.ReferenceId equals ar.Id
                    where arg.Id == id
                    select ar)
                .ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider);

            var result = new PagedList<AccountReferenceModel>()
            {
                Data = await accountReferences.ToListAsync(),
                TotalCount = await accountReferences.CountAsync()
            };

            return result;
        }


    }
}
