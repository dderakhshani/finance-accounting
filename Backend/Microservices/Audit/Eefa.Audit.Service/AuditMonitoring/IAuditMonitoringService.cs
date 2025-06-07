using Library.Interfaces;
using Library.Models;

namespace Eefa.Audit.Service.AuditMonitoring
{
    public interface IAuditMonitoringService : IService
    {
        public ServiceResult GetAll(AuditMonitoringModel auditMonitoringModel,
            AuditMonitoringProjectionModel auditMonitoringProjection, Pagination pagination);

    }
}