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


// kafka
var kafka = builder.AddKafka("kafka")
  .WithDataVolume(isReadOnly: false)// pour la persistence hors session
  .WithKafkaUI();// pour verifier



// email server

// api Portail Public

var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>("publicPortalApi")
  .WithReference(publicPortalDatabase)
  .WithReference(kafka)
  .WaitFor(publicPortalDatabase)
  .WaitFor(kafka);


publicPortalApi.WithHttpCommand(path: "/dbadmin/migrate","Migrate Database",commandOptions:new HttpCommandOptions(){IconName = "DatabaseArrowUpRegular" });

publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed

// api cts


// api Central

// 

builder.Build().Run();
