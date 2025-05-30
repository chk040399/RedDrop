var builder = DistributedApplication.CreateBuilder(args);


//pgsql public portal user
var publicPortalPostgresUser = builder.AddParameter("PublicPortalPostgresUser", "postgres");
var publicPortalPostgresPassword = builder.AddParameter("PublicPortalPostgresPassword", "walidozich");



var publicPortalPostgres =
  builder.AddPostgres("PublicPortalPostgres", userName: publicPortalPostgresUser, password: publicPortalPostgresPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();




//pgsql server(s)
var ctsPostgresUser = builder.AddParameter("CtsPostgresUser", "postgres");
var ctsPostgresPassword = builder.AddParameter("CtsPostgresPassword", "hikarosubahiko");

var ctsPostgres =
  builder.AddPostgres("CtsPostgres1", userName: ctsPostgresUser, password: ctsPostgresPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin()
    .WithPgWeb();

// kafka
var kafka = builder.AddKafka("kafka")
  .WithDataVolume(isReadOnly: false)// pour la persistence hors session
  .WithKafkaUI();// pour verifier

// email server


// api Portail Public
var publicPortalDatabase = publicPortalPostgres.AddDatabase(name: "PublicPortalDatabase", databaseName: "PublicPortalDatabase");// name => la resource ! 

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
var cts1Database = ctsPostgres.AddDatabase(name: "Cts1Database", databaseName: "Cts1Database");// name => la resource ! 

var cts1Api = builder.AddProject<Projects.HSTS_Back>("cts1Api")
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

  cts1Api.WithExternalHttpEndpoints();
  cts1Api.WithEnvironment("DatabaseName", "Cts1Database");

//TODO : Pb connection string
/*
// api cts 2
var cts2Database = ctsPostgres.AddDatabase(name: "Cts2Database", databaseName: "Cts2Database");// name => la resource ! 

var cts2Api = builder.AddProject<Projects.HSTS_Back>("cts2Api")
  .WithEndpoint(
    name: "my-https",//dont use https
    port: 57675 + 10,
    scheme: "https",
    isExternal: true,
    isProxied: false
  )
  .WithEndpoint(
    name: "my-http",//dont use http
    port: 57674 + 10,
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
  .WithReference(cts2Database)
  .WithReference(kafka)
  .WaitFor(cts2Database)
  .WaitFor(kafka);

cts2Api.WithExternalHttpEndpoints();
*/


// api Central

// 

builder.Build().Run();
