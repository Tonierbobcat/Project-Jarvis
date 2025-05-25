using ProjectJarvis.Core;

namespace ProjectJarvis;

public interface ILLMResponseQuery {
    Task<Stream> RequestAsync(Message[] messages);
}