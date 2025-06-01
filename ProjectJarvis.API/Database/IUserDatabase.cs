using ProjectJarvis.Core;

namespace ProjectJarvis;

// user's location not be stored directly on the database. 
// information about the location can be indirectly mentions on the MessageLog however in the future the log with be encrypted with the users key
public interface IUserDatabase {
    //method that's gets called when the connection to the database is successful
    void initialize();
    
    // returns the user by id
    /**
     * Returns User form id
     */
    UserData? GetUserFromId(string id);
    
    /**
     * Puts a user into the database
     */
    public bool Put(string id, UserData user);
    
    /**
     * Removes a users entry
     */
    bool Remove(UserData user);
}