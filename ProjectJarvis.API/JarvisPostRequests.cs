using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectJarvis.Core;

namespace ProjectJarvis;

public static class JarvisPostRequests {
    public static async Task SendMessage(HttpContext ctx, MessageLog log, string message) {
        ctx.Response.Headers.ContentType = "text/plain";

        ILLMResponseQuery ollama = new OllamaChatResponseQuery {
            Model = LLMModel.Lumimaid0_2
        };
        
        log.Log(SenderType.User, message);
        
        var messages = log.Get.ToArray();

        var stream = await ollama.RequestAsync(messages);
        
        using var reader = new StreamReader(stream);
        var builder = new StringBuilder();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            
            await ctx.Response.WriteAsync(line + "\n"); 
            await ctx.Response.Body.FlushAsync();
            
            AppendResponse(builder, line);
        }
        
        log.Log(SenderType.Assistant, builder);
    }

    private static void AppendResponse(StringBuilder builder, string line) {
        JsonDocument doc;
        try {
            doc = JsonDocument.Parse(line);
        } catch (JsonException) {
            Console.WriteLine("Failed to parse JSON chunk:");
            Console.WriteLine(line);
            return;
        }
        
        var root = doc.RootElement;

        if (!root.TryGetProperty("message", out var message)) 
            return;
        if (!message.TryGetProperty("content", out var content))
            return;
        var text = content.GetString();
        if (!string.IsNullOrEmpty(text)) {
            builder.Append(text);
        }
    }
}

