using System;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core
{
    public abstract class BaseEntity : Eefa.Common.Data.BaseEntity
    {
        //public TKey Id { get; private set; }
        public override bool Equals(object obj)
        {
            var entity = obj as BaseEntity;
            return entity != null &&
            GetType() == entity.GetType() &&
            EqualityComparer<int>.Default.Equals(Id, entity.Id);
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }
    }
}
