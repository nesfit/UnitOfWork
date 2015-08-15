namespace Fakes
{
    using System;

    using BaseDataModel;

    public class Foo: IDataModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((Foo)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Name?.GetHashCode() ?? 0);
            }
        }

        protected bool Equals(Foo foo)
        {
            return Id.Equals(foo.Id) && string.Equals(Name, foo.Name);
        }
    }
}