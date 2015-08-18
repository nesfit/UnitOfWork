namespace Fakes
{
    using System;

    using BaseDataModel;

    public class Foo: IDataModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public override Boolean Equals(Object obj)
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

        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (Id.GetHashCode() * 397) ^ (Name?.GetHashCode() ?? 0);
            }
        }

        private Boolean Equals(Foo foo)
        {
            return Id.Equals(foo.Id) && String.Equals(Name, foo.Name);
        }
    }
}