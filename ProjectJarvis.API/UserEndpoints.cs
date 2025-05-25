using Microsoft.AspNetCore.Identity;
using ProjectJarvis.Core;

namespace ProjectJarvis;

public class UserEndpoints(IUserDatabase database) {
    
    private readonly IPasswordHasher<UserData> hasher = new PasswordHasher<UserData>();
    
    public IResult CreateUser(HttpContext ctx, UserAuthForm auth, string name) {
        var user = UserData.Create(name, user => hasher.HashPassword(user, auth.Password)); 
    
        var success = database.Put(auth.Id, user);
    
        return !success ? Results.Conflict("User already exists.") :
            Results.Created($"/users/{auth.Id}", user);
    }

    public IResult RemoveUser(HttpContext ctx, UserAuthForm auth) {
        var user = database.GetUserFromId(auth.Id);
    
        if (user == null)
            return Results.NotFound("User not found.");
    
        if (!VerifyCredentials(user, auth))
            return Results.Unauthorized();

        var removed = database.Remove(user);
    
        return !removed ? Results.StatusCode(500) :
            Results.Ok("User removed.");
    }

    private bool VerifyCredentials(UserData user, UserAuthForm auth) {
        var result = hasher.VerifyHashedPassword(user, user.Password, auth.Password);
        return result != PasswordVerificationResult.Failed;
    }
    
    public async Task Message(HttpContext ctx, MessageRequest request) {
        var user = database.GetUserFromId(request.Auth.Id);
        if (user == null) {
            ctx.Response.StatusCode = 404;
            await ctx.Response.WriteAsync("User not found.");
            return;
        }

        var sysMsg = "You are an AI assistant to {user} you are currently talking to {user}. Do what ever {user} asks of you" // todo add this to config
            .Replace("{user}", user.Name);
        
        var log = user.MessageLog;
        try {
            await JarvisPostRequests.SendMessage(ctx, log, sysMsg + " " + request.Content); // todo include the userdata and environment json
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Console.WriteLine($"Error processing message: {e.Message}");
            ctx.Response.StatusCode = 500;
        }
    }
    
}