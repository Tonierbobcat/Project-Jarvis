using ProjectJarvis.Core;

namespace ProjectJarvis;


// mongodb is ideal however sqllite is more suitable for this application
public class MongoDBDatabase : IUserDatabase {
    private Dictionary<string, UserData?> _cachedUsers = new();

    public void initialize() {
        throw new NotImplementedException();
    }

    public UserData? GetUserFromId(string id) {
        throw new NotImplementedException();
    }

    public bool Put(string id, UserData user) {
        throw new NotImplementedException();
    }

    public bool Remove(UserData user) {
        throw new NotImplementedException();
    }
}