using Library.Common;
using Library.Interfaces;
using Library.Mappings;
using Library.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.AccountHead.Command
{
    public class AddAccountReferenceGroupToAccountHeadCommand : CommandBase, IRequest<ServiceResult>, IMapFrom<AddAccountReferenceGroupToAccountHeadCommand>, ICommand
    {
        public int ReferenceGroupId { get; set; }
        public int ReferenceNo { get; set; }
    }
}
