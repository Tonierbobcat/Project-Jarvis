using ProjectJarvis.Core;

namespace ProjectJarvis;

public interface IUserDatabase {
    void initialize();
    UserData? GetUserFromId(string id);
    public bool Put(string id, UserData user);
    bool Remove(UserData user);
}