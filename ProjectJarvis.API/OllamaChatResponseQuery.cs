using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProjectJarvis.Core;

namespace ProjectJarvis;

public class OllamaChatResponseQuery : ILLMResponseQuery {
    private const string URL = "http://localhost:11434/api/chat";
    
    private readonly HttpClient _client = new();
    public LLMModel Model { get; set; } = LLMModel.Llama3_2;
    
    public async Task<Stream> RequestAsync(Message[] messages) {
        var model = Model switch {
            LLMModel.Lumimaid0_2 => "leeplenty/lumimaid-v0.2:8b",
            _ => throw new Exception("Unhandled case")
        };
        
        var payload = JsonSerializer.Serialize(new {
            model,
            messages = messages.Select(node => new {
                role = node.role switch {
                    SenderType.User => "user",
                    SenderType.Assistant => "assistant",
                    _ => throw new NotImplementedException()
                },
                node.content
            })
        });
        
        Console.WriteLine($"Payload: {payload}");
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        
        var request = new HttpRequestMessage(HttpMethod.Post, URL) {
            Content = content
        };
    
        var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
    
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync();
    }
}