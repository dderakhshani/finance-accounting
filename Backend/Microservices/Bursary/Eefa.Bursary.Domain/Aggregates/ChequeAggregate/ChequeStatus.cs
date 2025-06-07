using System;
using System.Collections.Generic;
using System.Linq;
 

namespace Eefa.Bursary.Domain.Aggregates.ChequeAggregate
{
   public class ChequeStatus:Enumeration
    {
        public static ChequeStatus Ended = new ChequeStatus(1, nameof(Ended).ToLowerInvariant());

        public static ChequeStatus Wasted = new ChequeStatus(2, nameof(Wasted).ToLowerInvariant());

        public static ChequeStatus InUse = new ChequeStatus(3, nameof(InUse).ToLowerInvariant());

        public static ChequeStatus Created = new ChequeStatus(4, nameof(Created).ToLowerInvariant());

        public ChequeStatus(int id, string name):base(id,name)
        {
        }
        public static IEnumerable<ChequeStatus> List() =>
            new[] { Ended, Wasted };

        public static ChequeStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
              //  throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static ChequeStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
               // throw new OrderingDomainException($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
