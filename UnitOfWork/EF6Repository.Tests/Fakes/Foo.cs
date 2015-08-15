namespace EF6Repository.Tests.Fakes
{
    using System;

    using BaseDataModel;

    public class Foo: IDataModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }
    }
}