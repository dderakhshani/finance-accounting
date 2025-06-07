namespace Eefa.Audit.Service.AuditMonitoring
{
    public interface IMyProject
    {
        IMyProject EventTypes(bool eventType = true);
        IMyProject StartDates(bool startDate = true);
        IMyProject EndDates(bool endDate = true);
        IMyProject LoggedInUserIds(bool loggedInUserId = true);
        IMyProject LoggedInUsernames(bool loggedInUsername = true);
        IMyProject LoggedInUserRoleNames(bool loggedInUserRoleName = true);
        IMyProject LoggedInUserRoleIds(bool loggedInUserRoleId = true);
        IMyProject _ids(bool _id = true);
        IMyProject _ts(bool _t = true);
        IMyProject Environments(bool environment = true);
        IMyProject Actions(bool action = true);
        IMyProject HttpMethods(bool httpMethod = true);
        IMyProject ResponseStatusCodes(bool responseStatusCode = true);
        IMyProject ResponseStatuss(bool responseStatus = true);
    }
}