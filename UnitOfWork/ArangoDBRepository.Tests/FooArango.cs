using System;
using ArangoDB.Client;
using UnitOfWork.Fakes;

namespace UnitOfWork.ArangoDBRepository.Tests
{
    public class FooArango : Foo
    {
        [DocumentProperty(Identifier = IdentifierType.Key)]
        public String Key => this.Id.ToString();
    }
}