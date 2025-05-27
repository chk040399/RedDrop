var builder = DistributedApplication.CreateBuilder(args);


//pgsql server(s)
var publicPortalPostgresUser = builder.AddParameter("PublicPortalPostgresUser", "postgres");
var publicPortalPostgresPassword = builder.AddParameter("PublicPortalPostgresPassword", "walidozich");

var publicPortalPostgres =
  builder.AddPostgres("PublicPortalPostgres",userName:publicPortalPostgresUser,password:publicPortalPostgresPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();

var publicPortalDatabase = publicPortalPostgres.AddDatabase(name:"PublicPortalDatabase",databaseName: "PublicPortalDatabase");// name => la resource ! 


var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>("publicPortalApi")
  .WithReference(publicPortalDatabase)
  .WaitFor(publicPortalDatabase);

publicPortalApi.WithHttpCommand(path: "/dbadmin/migrate","Migrate Database",commandOptions:new HttpCommandOptions(){IconName = "HttpCommandOptions" });

publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed

builder.Build().Run();
