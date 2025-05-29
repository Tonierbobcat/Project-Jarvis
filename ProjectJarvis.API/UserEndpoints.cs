using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using ProjectJarvis.Core;

namespace ProjectJarvis;

public class UserEndpoints(IUserDatabase database) {
    
    private readonly IPasswordHasher<UserData> hasher = new PasswordHasher<UserData>();
    
    public Results<Created<UserData>, Conflict, BadRequest> CreateUser(HttpContext ctx, UserAuthForm auth)
    {
        var exists = database.GetUserFromId(auth.Id) != null;
        if (exists)
            return TypedResults.Conflict(); // user already exists
        
        var user = UserData.Create(auth.Id, user => hasher.HashPassword(user, auth.Password)); 
        var success = database.Put(auth.Id, user);
        return !success
            ? TypedResults.BadRequest() // not success
            : TypedResults.Created($"/users/{auth.Id}", user); // ok
    }

    public IResult GetUser(HttpContext ctx, UserAuthForm auth) {
        var user = database.GetUserFromId(auth.Id);
        if (user == null) {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(user); //todo change this. return user instead
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
            .Replace("{user}", user.Id);
        
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