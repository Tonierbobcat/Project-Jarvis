using ProjectJarvis.Core;

namespace ProjectJarvis;


// this is unused my be used in the future idk
public class UserContext {
    public UserData User { get; private set; }
    public EnvironmentalData Env { get; private set; }
    public Location Location { get; private set; }
}