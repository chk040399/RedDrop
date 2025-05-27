var builder = DistributedApplication.CreateBuilder(args);


//pgsql server(s)
var publicPortalPostgresUser = builder.AddParameter("PublicPortalPostgresUser", "postgres");
var publicPortalPostgresPassword = builder.AddParameter("PublicPortalPostgresPassword", "walidozich");

var publicPortalPostgres =
  builder.AddPostgres("PublicPortalPostgres",userName:publicPortalPostgresUser,password:publicPortalPostgresPassword,port: 5432)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();

publicPortalPostgres.AddDatabase(name:"PublicPortalDatabase",databaseName: "PublicPortalDatabase");// name => la resource ! 



var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>("publicPortalApi")
  .WithReference(publicPortalPostgres)
  .WaitFor(publicPortalPostgres);


publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed






builder.Build().Run();
