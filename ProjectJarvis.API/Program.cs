using ProjectJarvis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

IUserDatabase database = new SQLLiteUserDatabase();

var endpoints = new UserEndpoints(database);

app.MapPost("/create-user", (HttpContext ctx, UserAuthForm form) => endpoints.CreateUser(ctx, form));

app.MapPost("/remove-user", (HttpContext ctx, UserAuthForm form) => endpoints.RemoveUser(ctx, form));

app.MapPost("/message", (HttpContext ctx, MessageRequest request) => endpoints.Message(ctx, request));

app.MapPost("get-user", (HttpContext ctx, UserAuthForm form) => endpoints.GetUser(ctx, form));

app.Run();

public record MessageRequest(UserAuthForm? Auth, Location Location, string Content);

public record Location(double Longitude, double Latitude);

public record UserAuthForm(string Id, string Password);