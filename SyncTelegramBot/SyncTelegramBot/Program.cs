using System.Text.Encodings.Web;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUNFClient, UNFClient>();
builder.Services.AddScoped<IGetRequestHandler, GetRequestHandler>();
builder.Services.AddScoped<IReceiptRequestHandler, ReceiptRequestHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RequestStrings>(builder.Configuration.GetSection(RequestStrings.Position));

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