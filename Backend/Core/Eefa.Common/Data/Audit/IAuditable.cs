using System.Collections.Generic;

namespace Eefa.Common.Data
{
    public interface IAuditable
    {
        List<AuditMapRule> Audit();
    }
}