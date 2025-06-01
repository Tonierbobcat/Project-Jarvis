using System.Collections.Generic;
using System.Linq;
using ProjectJarvis.Core;

namespace ProjectJarvis;

// with sql lite we can have a .db file that can act as the database without having to reference an external database
public class SQLLiteUserDatabase : IUserDatabase {
    private Dictionary<string, UserData?> _cachedUsers = new();
    
    public void initialize() {
    }

    public UserData? GetUserFromId(string id) {
        return _cachedUsers.GetValueOrDefault(id); //todo change this
    }

    public bool Put(string id, UserData user) {
        _cachedUsers.Add(id, user);
        return true;
    }

    public bool Remove(UserData user) {
        for (var i = 0; i < _cachedUsers.Count; i++) {
            var u = _cachedUsers.ElementAt(i).Value;
            if (u == null)
                continue; 
            if (u.Guid != user.Guid)
                continue;
            _cachedUsers.Remove(_cachedUsers.ElementAt(i).Key);
            return true;
        }
        
        return false;
    }
}