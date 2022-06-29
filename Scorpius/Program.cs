using EndpointDefinitions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//if we want redirection to the correct port
//builder.Services.AddHttpsRedirection(options => options.HttpsPort = 9008);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAllEndpointDefinitions();

var app = builder.Build();
app.Logger.LogInformation("Running with Firebase file: {FcmFile}", builder.Configuration.GetSection("FirebaseCM")["AppConfig"]);


var adminFire = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration.GetSection("FirebaseCM")["AppConfig"])
});


// Configure the HTTP request pipeline.
app.Logger.LogInformation("Running with env: {EnvName}", app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//redirection from http to https
//app.UseHttpsRedirection();
app.UseEndpointDefinitions();

app.Run();