using System.Text.Encodings.Web;
using SyncTelegramBot.Models.HelpModels;
using SyncTelegramBot.Services;
using SyncTelegramBot.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUnfClient, UnfClient>();
builder.Services.AddScoped<GetRequestHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RequestValues>(builder.Configuration.GetSection(RequestValues.Position));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (ctx, next) =>
{
    if (ctx.Request.Headers.TryGetValue("SecretKey", out var secretKey) && secretKey == app.Configuration["RequestStrings:SecretKey"])
        await next();
});

app.UseAuthorization();

app.MapControllers();

app.Run();