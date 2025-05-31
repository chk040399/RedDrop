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
var publicPortalApiName = "publicPortalApi";
var publicPortalDatabaseName = "PublicPortalDatabase";
var publicPortalDatabase = publicPortalPostgres.AddDatabase(name: "PublicPortalDatabase", databaseName: publicPortalDatabaseName);// name => la resource ! 

var publicPortalApi = builder.AddProject<Projects.BD_PublicPortal_Api>(publicPortalApiName)
  .WithUrlForEndpoint("http", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  .WithUrlForEndpoint("https", url =>
  {
    url.Url = "/swagger";
    url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  })
  //.WithEndpoint(
  //  name: "my-https",//dont use https
  //  port: 57679 + 10,
  //  scheme: "https",
  //  isExternal:true,
  //  isProxied: false
  //)
  //.WithEndpoint(
  //  name: "my-http",//dont use http
  //    port: 57678 + 10,
  //    scheme: "http",
  //    isExternal: true,
  //    isProxied: false
  //)
  //.WithUrlForEndpoint("my-http", url =>
  //{
  //  url.Url = "/swagger";
  //  url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  //})
  //.WithUrlForEndpoint("my-https", url =>
  //{
  //  url.Url = "/swagger";
  //  url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
  //})
  .WithReference(publicPortalDatabase)
  .WithReference(kafka)
  .WaitFor(publicPortalDatabase)
  .WaitFor(kafka);

publicPortalApi.WithHttpCommand(path: "/dbadmin/migrate","Migrate Database",commandOptions:new HttpCommandOptions(){IconName = "DatabaseArrowUp" });
publicPortalApi.WithExternalHttpEndpoints();//TODO : disable if nno external acces is needed
publicPortalApi.WithEnvironment("DatabaseName", publicPortalDatabaseName);

// api Central

//cts
int ctsNr = 1;
for(int i = 1;i <= ctsNr; i++)
{
  var ctsApiName = $"cts{i}Api";
  var ctsDatabaseName = $"Cts{i}Database";
  var ctsKafkaBrokerPartionIdentifId = $"Cts{i}PartionId";
  //var ctsHttpsExposedPort = 57677 + 10 + i;
  //var ctsHttpExposedPort = 57676 + 10 + i;

  var ctsDatabase =
    ctsPostgres.AddDatabase(name: ctsDatabaseName, databaseName: ctsDatabaseName); // name => la resource ! 

  var ctsApi = builder.AddProject<Projects.HSTS_Back>(ctsApiName)
    .WithUrlForEndpoint("http", url =>
    {
      url.Url = "/swagger";
      url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
    })
    .WithUrlForEndpoint("https", url =>
    {
      url.Url = "/swagger";
      url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
    })
    //.WithEndpoint(
    //  name: "my-https", //dont use https
    //  port: ctsHttpsExposedPort,
    //  scheme: "https",
    //  isExternal: true,
    //  isProxied: false
    //)
    //.WithEndpoint(
    //  name: "my-http", //dont use http
    //  port: ctsHttpExposedPort,
    //  scheme: "http",
    //  isExternal: true,
    //  isProxied: false
    //)
    //.WithUrlForEndpoint("my-http", url =>
    //{
    //  url.Url = "/swagger";
    //  url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
    //})
    //.WithUrlForEndpoint("my-https", url =>
    //{
    //  url.Url = "/swagger";
    //  url.DisplayLocation = UrlDisplayLocation.SummaryAndDetails;
    //})
    .WithReference(ctsDatabase)
    .WithReference(kafka)
    .WaitFor(ctsDatabase)
    .WaitFor(kafka);

  ctsApi.WithExternalHttpEndpoints();
  ctsApi.WithEnvironment("DatabaseName", ctsDatabaseName);
  ctsApi.WithEnvironment("KafkaBrokerPartionIdentifId", ctsKafkaBrokerPartionIdentifId);
  
}









// 

builder.Build().Run();
