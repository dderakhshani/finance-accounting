//using System.Linq;
//using Audit.Core;
//using Library.Interfaces;
//using Library.Models;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Newtonsoft.Json;

//namespace Eefa.Audit.Service.AuditMonitoring
//{
//    public class AuditMonitoringService : IAuditMonitoringService
//    {
//        private readonly IAuditMontiroringRepository _auditMontiroringRepository;
//        private readonly ICurrentUserAccessor _currentUserAccessor;
//        public AuditMonitoringService(IAuditMontiroringRepository auditMontiroringRepository, ICurrentUserAccessor currentUserAccessor)
//        {
//            _auditMontiroringRepository = auditMontiroringRepository;
//            _currentUserAccessor = currentUserAccessor;
//        }


//        public ServiceResult GetAll(AuditMonitoringModel auditMonitoringModel, AuditMonitoringProjectionModel auditMonitoringProjection, Pagination pagination)
//        {
//            //auditMonitoringModel.ResponseStatusCode = "401";
//            auditMonitoringModel.HttpMethod = "GET";

//            var filter = FilterCreator(auditMonitoringModel);
//            var founded = _auditMontiroringRepository.Table().Find(filter)
//                .MyProject(project => project
//                    .EventTypes(auditMonitoringProjection.EventType)
//                    .EndDates(auditMonitoringProjection.EndDate)
//                    .LoggedInUserIds(auditMonitoringProjection.UserId)
//                    .LoggedInUserRoleIds(auditMonitoringProjection.RoleId)
//                    .LoggedInUserRoleNames(auditMonitoringProjection.RoleName)
//                    .LoggedInUsernames(auditMonitoringProjection.Username)
//                    .StartDates(auditMonitoringProjection.StartDate)
//                    .Actions(auditMonitoringProjection.Action)
//                    .Environments(auditMonitoringProjection.Environment)
//                    .HttpMethods(auditMonitoringProjection.HttpMethod)
//                    .ResponseStatusCodes(auditMonitoringProjection.ResponseStatusCode)
//                    .ResponseStatuss(auditMonitoringProjection.ResponseStatus)
//                    ._ids(auditMonitoringProjection._id)
//                    ._ts(auditMonitoringProjection._t)
//                )
//                .Skip(pagination.Skip).Limit(pagination.Take);


//            return ServiceResult.Set(founded// دریافت تمام رکورد ها از دیتابیس به صورت کوئری
//                .ToList()//تبدیل به لیست
//                .ConvertAll(BsonTypeMapper.MapToDotNetValue)// تبدیل بی سون به جیسون قابل فهم برای دات نت
//                .Select(JsonConvert.SerializeObject)// تبدیل به جیسون
//                .Select(AuditEvent.FromJson)// تبدیل به آئودیت ایونت از جیسون
//                .ToList());
//        }

//        private FilterDefinition<BsonDocument> FilterCreator(AuditMonitoringModel auditMonitoringModel)
//        {
//            var filter = Builders<BsonDocument>.Filter.Gte("StartDate", auditMonitoringModel.StartDate);
//            filter &= Builders<BsonDocument>.Filter.Lte("EndDate", auditMonitoringModel.EndDate);

//            // var filter = Builders<BsonDocument>.Filter.Eq("UserId._v", _currentUserAccessor.GetId());

//            //   filter |= Builders<BsonDocument>.Filter.Where(x => x["RoleLevelCode._v"].ToString().StartsWith(_currentUserAccessor.GetRoleLevelCode()));


//            if (auditMonitoringModel.HttpMethod is not null)
//                filter &= Builders<BsonDocument>.Filter.Eq("Action.HttpMethod", auditMonitoringModel.HttpMethod);

//            if (auditMonitoringModel.ResponseStatus is not null)
//                filter &= Builders<BsonDocument>.Filter.Eq("Action.ResponseStatus", auditMonitoringModel.ResponseStatus);

//            if (auditMonitoringModel.ResponseStatusCode is not null)
//                filter &= Builders<BsonDocument>.Filter.Eq("Action.ResponseStatusCode", auditMonitoringModel.ResponseStatusCode);

//            return filter;
//        }
//    }
//}