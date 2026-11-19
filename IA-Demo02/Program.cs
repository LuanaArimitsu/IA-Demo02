using Microsoft.Extensions.AI;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OllamaSharp;

const string openApiKey = "CHAVE_DO_OPEN_AI";
var uri = new Uri("http://localhost:11434");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
var app = builder.Build();

IDistributedCache cache = new MemoryDistributedCache(
    Options.Create(new MemoryDistributedCacheOptions()));

var client =
    app.Environment.IsDevelopment()
        ? new OllamaApiClient(uri, defaultModel: "llama3.2:latest")
        : new OpenAI.Chat.ChatClient("gpt-4o-mini", openApiKey)
        .AsIChatClient();

var cachedClient= new ChatClientBuilder(client)
    .UseDistributedCache(cache)
    .Build();

app.MapPost (pattern: "/", async (Question question) =>
{ 
    var response = await client.GetResponseAsync(question.prompt);
    return Results.Ok(response.Text);
});

app.MapPost(pattern: "/v2", async (Question question) =>
{
    var result = await client.GetResponseAsync(
    [
        new ChatMessage(ChatRole.System, "You are a very technical weather expert. Answer me with just 10 words."),
        new ChatMessage(ChatRole.User, question.prompt),
    ]);
    return Results.Ok(result.Text);
});

app.MapPost(pattern: "/v3", async (Question question) =>
{
    var result = await cachedClient.GetResponseAsync(
    [
        new ChatMessage(ChatRole.System, "You are a very technical weather expert. Answer me with just 10 words."),
        new ChatMessage(ChatRole.User, question.prompt),
    ]);
    return Results.Ok(result.Text);
});



app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

public record Question(string prompt);