namespace Eefa.Audit.Service.AuditMonitoring
{
    public class MyProjectConfigurator : IMyProject
    {
        public bool EventType { get; set; } = false;
        public bool StartDate { get; set; } = false;
        public bool EndDate { get; set; } = false;
        public bool LoggedInUserId { get; set; } = false;
        public bool LoggedInUsername { get; set; } = false;
        public bool LoggedInUserRoleName { get; set; } = false;
        public bool LoggedInUserRoleId { get; set; } = false;
        public bool _id { get; set; } = false;
        public bool _t { get; set; } = false;
        public bool Environment { get; set; } = false;
        public bool Action { get; set; } = false;
        public bool HttpMethod { get; set; } = false;
        public bool ResponseStatusCode { get; set; } = false;
        public bool ResponseStatus { get; set; } = false;
        public bool UserId { get; set; } = false;

        public IMyProject EventTypes(bool eventType = true)
        {
            EventType = eventType;
            return this;
        }

        public IMyProject StartDates(bool startDate = true)
        {
            StartDate = startDate;
            return this;
        }

        public IMyProject EndDates(bool endDate = true)
        {
            EndDate = endDate;
            return this;
        }

        public IMyProject LoggedInUserIds(bool loggedInUserId = true)
        {
            LoggedInUserId = loggedInUserId;
            return this;
        }

        public IMyProject LoggedInUsernames(bool loggedInUsername = true)
        {
            LoggedInUsername = loggedInUsername;
            return this;
        }

        public IMyProject LoggedInUserRoleNames(bool loggedInUserRoleName = true)
        {
            LoggedInUserRoleName = loggedInUserRoleName;
            return this;
        }

        public IMyProject LoggedInUserRoleIds(bool loggedInUserRoleId = true)
        {
            LoggedInUserRoleId = loggedInUserRoleId;
            return this;
        }

        public IMyProject _ids(bool _id = true)
        {
            this._id = _id;
            return this;
        }

        public IMyProject _ts(bool _t = true)
        {
            this._t = _t;
            return this;
        }

        public IMyProject Environments(bool environment = true)
        {
            Environment = environment;
            return this;
        }

        public IMyProject Actions(bool action = true)
        {
            Action = action;
            return this;
        }

        public IMyProject HttpMethods(bool httpMethod = true)
        {
            HttpMethod = httpMethod;
            return this;
        }

        public IMyProject ResponseStatusCodes(bool responseStatusCode = true)
        {
            ResponseStatusCode = responseStatusCode;
            return this;
        }

        public IMyProject ResponseStatuss(bool responseStatus = true)
        {
            ResponseStatus = responseStatus;
            return this;
        }
    }
}