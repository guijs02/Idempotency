using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect("localhost:6379");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/orders", async
    (HttpContext context,
    [FromServices] IConnectionMultiplexer redis,
    [FromBody] OrderRequest order) =>
{

    var db = redis.GetDatabase();

    string key = $"{order.UserId}-{order.OrderId}";

    var created = await db.StringSetAsync(key, "processing", TimeSpan.FromMinutes(5), when: When.NotExists);

    if (!created)
    {
        return Results.Conflict(new
        {
            status = "processing",
            message = "Já existe um pedido em andamento. Por favor, aguarde!"
        });
    }

    await Task.Delay(2000);

    // ✅ Salva resultado final com TTL maior
    await db.StringSetAsync(key, JsonSerializer.Serialize(order), TimeSpan.FromHours(1));

    return Results.Accepted(
        $"/orders/{order.OrderId}/status",
        new { message = "Processamento iniciado. Aguarde." });
});

app.Run();

public record OrderRequest(Guid UserId, int OrderId, int ProductId);
