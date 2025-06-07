using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Model
{
    public class GetNoResultModel
    {
        public GetNoResultModel(int minNo,int maxNo)
        {
            MinNo = minNo;
            MaxNo = maxNo;
        }
        public int MinNo { get; set; }
        public int MaxNo { get; set; }
    }
}
