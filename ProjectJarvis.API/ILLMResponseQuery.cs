using System.IO;
using System.Threading.Tasks;
using ProjectJarvis.Core;

namespace ProjectJarvis;

public interface ILLMResponseQuery {
    Task<Stream> RequestAsync(Message[] messages);
}