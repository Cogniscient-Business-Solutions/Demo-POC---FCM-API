using FirebaseAdmin;
using FirebaseNotificationAPI.Services;
using Google.Apis.Auth.OAuth2;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Firebase Admin SDK configuration "firebase-adminsdk.json","fir-pushnotification-61e36-firebase-adminsdk-i09r8-54a6db97b9.json"
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("fir-pushnotification-61e36-firebase-adminsdk-i09r8-54a6db97b9.json")
});

// Add services for DI
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Add the connection string (Assuming you have it in appsettings.json)
builder.Services.AddSingleton(new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
