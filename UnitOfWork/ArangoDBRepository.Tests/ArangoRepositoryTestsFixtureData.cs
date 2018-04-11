using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArangoDB.Client;
using ArangoDB.Client.Data;

namespace UnitOfWork.ArangoDBRepository.Tests
{
    public class ArangoRepositoryTestsFixtureData
    {
        private static readonly Lazy<DatabaseSharedSetting> SharedSetting = new Lazy<DatabaseSharedSetting>(() =>
        {
#if NET46
// Uncomment if you want your requests goes through proxy
            ArangoDatabase.ClientSetting.Proxy = new System.Net.WebProxy("http://localhost:19777");
#endif

            var sharedSetting = new DatabaseSharedSetting
            {
                Database = "ExampleDB",
                Url = "http://localhost:8000"
            };

            var credential = GetCredential();

            sharedSetting.SystemDatabaseCredential = new NetworkCredential(
                credential.UserName,
                credential.Password);
            sharedSetting.Credential = new NetworkCredential(
                credential.UserName,
                credential.Password);

            sharedSetting.Collection.ChangeIdentifierDefaultName(IdentifierType.Key, "Key");

            using (var db = new ArangoDatabase(sharedSetting))
            {
                if (!db.ListDatabases().Contains(sharedSetting.Database))
                    db.CreateDatabase(sharedSetting.Database, new List<DatabaseUser>
                    {
                        new DatabaseUser
                        {
                            Username = credential.UserName,
                            Passwd = credential.Password
                        }
                    });

                var collections = db.ListCollections().Select(c => c.Name).ToArray();
                var collectionsToCreate = new[]
                {
                    new Tuple<String, CollectionType>("FooArango", CollectionType.Document)
                };

                foreach (var c in collectionsToCreate)
                    if (collections.Contains(c.Item1) == false)
                        db.CreateCollection(c.Item1, type: c.Item2);
            }

            return sharedSetting;
        });

        public ArangoRepositoryTestsFixtureData()
        {
            this.UnitOfWork = new ArangoDBUnitOfWork.ArangoDBUnitOfWork(SharedSetting.Value);
        }

        public ArangoDBUnitOfWork.ArangoDBUnitOfWork UnitOfWork { get; set; }

        private static NetworkCredential GetCredential()
        {
            return new NetworkCredential("root", "123456");
        }
    }
}