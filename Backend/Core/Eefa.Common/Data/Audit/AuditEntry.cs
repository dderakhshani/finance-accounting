using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Eefa.Common.Data
{
    public class AuditEntry : BaseEntity
    {
        public EntityEntry Entry { get; set; }
        public int UserId { get; set; }
        public int MenueId { get; set; }
        public string TableName { get; set; }
        public SubSystem SubSystem { get; set; }
        public int PrimaryKey { get; set; }
        public IBaseEntity Primary { get; set; }
        public List<AuditChangeValue> Changes { get; set; } = new List<AuditChangeValue>();
        public AuditType AuditType { get; set; }
        public Guid TransactionId { get; set; }
        public Audit ToAudit()
        {
            var audit = new Audit
            {
                UserId = UserId,
                OwnerRoleId = OwnerRoleId,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedById = UserId,
                MenuId = MenueId,
                Type = AuditType.ToString(),
                TableName = TableName,
                DateTime = DateTime.Now,
                PrimaryId = PrimaryKey,
                TransactionId = TransactionId,
                ChangesValues = Changes.Count == 0 ? null : JsonConvert.SerializeObject(Changes)
            };
            return audit;
        }
    }
}