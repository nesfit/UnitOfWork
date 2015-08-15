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
            return Equals((Foo)obj);
        }

        private bool Equals(Foo foo)
        {
            return foo != null && (Id == foo.Id && Name == foo.Name);
        }
    }
}