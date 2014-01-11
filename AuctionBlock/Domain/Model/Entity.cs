using System;

namespace AuctionBlock.Domain.Model
{
    public class Entity : Entity<Guid>
    {
    }

    // Didn't take time to refactor to rule of 4s as it's
    // just some web infrastructure code from here
    // http://stackoverflow.com/questions/11785505/nhibernate-and-entity-base-class
    public abstract class Entity<TId>
    {

        public virtual TId Id { get; protected set; }
        protected virtual int Version { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TId>);
        }

        private static bool IsTransient(Entity<TId> obj)
        {
            return obj != null &&
                   Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(TId)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }
    }
}