var builder = DistributedApplication.CreateBuilder(args);


//pgsql cts
var publicPortalPostgresUser = builder.AddParameter("PublicPortalPostgresUser", "postgres");
var publicPortalPostgresPassword = builder.AddParameter("PublicPortalPostgresPassword", "walidozich");

var publicPortalPostgres =
  builder.AddPostgres("PublicPortalPostgres", userName: publicPortalPostgresUser, password: publicPortalPostgresPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();

var publicPortalDatabase = publicPortalPostgres.AddDatabase(name: "PublicPortalDatabase", databaseName: "PublicPortalDatabase");// name => la resource ! 


//pgsql server(s)
var ctsPostgresUser = builder.AddParameter("CtsPostgresUser", "postgres");
var ctsPostgresPassword = builder.AddParameter("CtsPostgresPassword", "hikarosubahiko");

var ctsPostgres =
  builder.AddPostgres("CtsPostgres1", userName: ctsPostgresUser, password: ctsPostgresPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();


var cts1Database = ctsPostgres.AddDatabase(name: "Cts1PortalDatabase", databaseName: "Cts1PortalDatabase");// name => la resource ! 


// kafka
var kafka = builder.AddKafka("kafka")
  .WithDataVolume(isReadOnly: false)// pour la persistence hors session
  .WithKafkaUI();// pour verifier



// email server

// api Portail Public

var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>("publicPortalApi")
  .WithEndpoint(
    name: "my-https",//dont use https
    port: 57679 + 10,
    scheme: "https",
    isExternal:true,
    isProxied: false
  )
  .WithEndpoint(
    name: "my-http",//dont use http
      port: 57678 + 10,
      scheme: "http",
      isExternal: true,
      isProxied: false
  )
  .WithUrlForEndpoint("my-http", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  .WithUrlForEndpoint("my-https", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  .WithReference(publicPortalDatabase)
  .WithReference(kafka)
  .WaitFor(publicPortalDatabase)
  .WaitFor(kafka);

publicPortalApi.WithHttpCommand(path: "/dbadmin/migrate","Migrate Database",commandOptions:new HttpCommandOptions(){IconName = "DatabaseArrowUp" });
publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed

// api cts 1
var cts1 = builder.AddProject<Projects.HSTS_Back>("cts1Api")
  .WithEndpoint(
    name: "my-https",//dont use https
    port: 57677 + 10,
    scheme: "https",
    isExternal: true,
    isProxied: false
  )
  .WithEndpoint(
    name: "my-http",//dont use http
    port: 57676 + 10,
    scheme: "http",
    isExternal: true,
    isProxied: false
  )
  .WithUrlForEndpoint("my-http", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  .WithUrlForEndpoint("my-https", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  .WithReference(cts1Database)
  .WithReference(kafka)
  .WaitFor(cts1Database)
  .WaitFor(kafka);

cts1.WithExternalHttpEndpoints();


// api cts 2


// api Central

// 

builder.Build().Run();
