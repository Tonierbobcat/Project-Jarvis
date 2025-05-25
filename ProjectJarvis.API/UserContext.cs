using ProjectJarvis.Core;

namespace ProjectJarvis;

public class UserContext {
    public UserData User { get; private set; }
    public EnvironmentalData Env { get; private set; }
    public Location Location { get; private set; }
}