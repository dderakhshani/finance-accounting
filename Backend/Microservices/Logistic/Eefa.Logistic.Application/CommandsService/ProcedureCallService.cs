using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using Eefa.Logistic.Infrastructure.Context;

namespace Eefa.Logistic.Application
{
    public class ProcedureCallService : IProcedureCallService
    {

        private readonly IMapper _mapper;
        private readonly LogisticContext _context;
       
        public ProcedureCallService(
              IMapper mapper
            , LogisticContext context
            


            )
        {
            _mapper = mapper;
            _context = context;
            

        }

        public Task ModifyDocumentAttachments(List<int> attachmentIds, int DocumentHeadId)
        {
            throw new System.NotImplementedException();
        }

        

    }
}
