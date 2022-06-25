using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Scorpius.EndpointDefinitions.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//if we want redirection to the correct port
//builder.Services.AddHttpsRedirection(options => options.HttpsPort = 9008);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAllEndpointDefinitions();

var adminFire = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(builder.Configuration.GetSection("FirebaseCM")["AppConfig"])
});
/* // if you want to filter the endpoints that are enabled;
 builder.Services.AddEndpointDefinitions(new Type[]
{
    typeof(Estimations), typeof(CsiLog)
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//redirection from http to https
//app.UseHttpsRedirection();
app.UseEndpointDefinitions();

app.Run();