using System;
using ArangoDB.Client;
using Fakes;

namespace ArangoDBRepository.Tests
{
    public class FooArango : Foo
    {
        [DocumentProperty(Identifier = IdentifierType.Key)]
        public String Key => this.Id.ToString();
    }
}