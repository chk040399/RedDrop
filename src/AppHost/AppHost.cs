var builder = DistributedApplication.CreateBuilder(args);


//pgsql server(s)
var publicPortalPostgresUser = builder.AddParameter("PublicPortalPostgresUser", "postgres");
var publicPortalPostgresPassword = builder.AddParameter("PublicPortalPostgresPassword", "walidozich");

var publicPortalPostgres =
  builder.AddPostgres("PublicPortalPostgres",userName:publicPortalPostgresUser,password:publicPortalPostgresPassword,port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();

var publicPortalDatabase = publicPortalPostgres.AddDatabase(name:"PublicPortalDatabase",databaseName: "PublicPortalDatabase");// name => la resource ! 

var migrationService = builder.AddProject<Projects.BD_MigrationService>("PublicPortalDatabaseMigrationService")
  .WithReference(publicPortalDatabase)
  .WithParentRelationship(publicPortalPostgres)
  .WaitFor(publicPortalDatabase);




var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>("publicPortalApi")
  .WithReference(publicPortalDatabase)
  .WaitForCompletion(migrationService);


publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed






builder.Build().Run();
