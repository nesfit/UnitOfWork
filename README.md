# UnitOfWork
C# Generic Unit Of Work And Repository Patterns
* Experimental implementation of Repository and UnitOfWork for **Cassandra** and **ArrangoDB**
* Uses: 
  * [**DataStax C# Driver**](https://github.com/datastax/csharp-driver) for **Cassandra**
  * DataStax's [LINQ](https://docs.datastax.com/en/developer/csharp-driver/3.4/features/components/linq/) integrations.
  * [**ArangoDB .Net Client**](http://www.arangoclient.net/) for **ArangoDB**
  * DmitryPavliv's [UnitOfWork](https://github.com/dmitryPavliv/UnitOfWork)

# Run Cassandra with Docker
* Look at [docker-compose-cassandra-cluster](/UnitOfWork/docker-compose-cassandra-cluster.yml)

# Run ArangoDB with Docker
* Look at [docker-compose-arangodb-cluster](/UnitOfWork/docker-compose-arangodb-cluster.yml)