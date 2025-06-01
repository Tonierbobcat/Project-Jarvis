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
    // this is the api url for ollama 
    // https://github.com/ollama/ollama
    // the api has two options for generating text for the end user
    // chat and generate. chat allows the developer to send message data along with the roles of the sender 'system', 'user', 'assistant', or 'tool'
    private const string URL = "http://localhost:11434/api/chat";
    
    private readonly HttpClient _client = new();
    public LLMModel Model { get; set; } = LLMModel.Llama3_2;
    
    // this is in order to stream text to the end user
    // it first waits to receive all the data from the llm api and then
    public async Task<Stream> RequestAsync(Message[] messages) {
        //todo add more cases
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
                    SenderType.Tool => "tool",
                    SenderType.System => "system",
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