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




// here we map our user end points such as accessing user data and creating a user
app.MapPost("/create-user", (HttpContext ctx, UserAuthForm form) => endpoints.CreateUser(ctx, form)); // maybe something like ValidateAPIKey(key, execute)

app.MapPost("/remove-user", (HttpContext ctx, UserAuthForm form) => endpoints.RemoveUser(ctx, form));

app.MapPost("/message", (HttpContext ctx, MessageRequest request) => endpoints.Message(ctx, request));

app.MapPost("/get-user", (HttpContext ctx, UserAuthForm form) => endpoints.GetUser(ctx, form));

//public api key specific actions

//todo map a post here to return meteorological data from latitude and longitude

//todo map method to return 

app.Run();

public record MessageRequest(UserAuthForm? Auth, Location Location, string Content);

public record Location(double Longitude, double Latitude);

public record UserAuthForm(string Id, string Password);