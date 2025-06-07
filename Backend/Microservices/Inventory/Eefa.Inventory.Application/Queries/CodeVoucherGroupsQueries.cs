using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class CodeVoucherGroupsQueries : ICodeVoucherGroupsQueries
    {

        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _context;
        private readonly IRepository<Receipt> _receiptRepository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        public CodeVoucherGroupsQueries(
              IMapper mapper
            , IInvertoryUnitOfWork context
            , IRepository<Receipt> receiptRepository
            , ICurrentUserAccessor currentUserAccessor

            )
        {
            _mapper = mapper;
            _context = context;
            _receiptRepository = receiptRepository;
            _currentUserAccessor = currentUserAccessor;
        }

        /// <summary>
        /// ورود و خروج کالا از انبار
        /// </summary>
        
        public async Task<PagedList<ReceiptALLStatusModel>> GetReceiptALLVoucherGroup(string Code)
        {
            List<int> AccessCodeVoucherGroup = AccessCodeVoucherGroups();
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.Code.Substring(0, 1) == Code || String.IsNullOrEmpty(Code)
                                                                            && AccessCodeVoucherGroup.Contains(a.Id))
            .ProjectTo<ReceiptALLStatusModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

            var result = new PagedList<ReceiptALLStatusModel>()
            {
                Data = codeVoucherGroup.OrderBy(a=>a.UniqueName),
                TotalCount = 0
            };
            return result;

        }
        public async Task<PagedList<ReceiptALLStatusModel>> GetALL()
        {
            var codeVoucherGroup = await _context.CodeVoucherGroups.Where(a => a.SchemaName == ConstantValues.CodeVoucherGroupValues.SchemaName)                                                              
            .ProjectTo<ReceiptALLStatusModel>(_mapper.ConfigurationProvider).OrderBy(a=>a.ViewId).OrderBy(a=>a.Title)
            .ToListAsync();

            var result = new PagedList<ReceiptALLStatusModel>()
            {
                Data = codeVoucherGroup.OrderBy(a => a.UniqueName),
                TotalCount = 0
            };
            return result;

        }
       
        private List<int> AccessCodeVoucherGroups()
        {
            return _context.AccessToWarehouse.Where(a => a.TableName == ConstantValues.AccessToWarehouseEnam.CodeVoucherGroups && a.UserId == _currentUserAccessor.GetId() && !a.IsDeleted).Select(a => a.WarehouseId).ToList();
        }

    }
    }
