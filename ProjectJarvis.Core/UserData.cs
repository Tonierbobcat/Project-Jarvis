using System.Text;

namespace ProjectJarvis.Core;

public class UserData {
    public MessageLog MessageLog { get; private set; } = new();
    public string Name { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    
    public Guid Guid { get; private set; }

    private UserData() {
        Guid = Guid.NewGuid();
    }

    public static UserData Create(string name, Func<UserData, string> password) {
        var user = new UserData {
            Name = name,
        };
        user.Password = password(user);
        return user;
    }
}

public class MessageLog : ICloneable {
    private LinkedList<Message> Nodes { get; set; } = [];

    public LinkedList<Message> Get => Nodes;
    
    public void Log(SenderType sender, string message) {
        Nodes.AddLast(new Message(sender, message));
    }
    
    public void Log(SenderType sender, StringBuilder message) {
        Log(sender, message.ToString());
    }

    public object Clone() {
        var messages = new Message[Nodes.Count];
        Nodes.CopyTo(messages, 0);
        
        var log = new MessageLog 
        {
            Nodes = new LinkedList<Message>(messages)
        };
        
        return log;
    }
}

public record Message(SenderType role, string content);

public enum SenderType {
    User,
    Assistant
}