using API.DataModel;
using API.Repository.UserRepo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Database connection

var SqlConnectionString = builder.Configuration["SqlConnectionString"];
builder.Services.AddSingleton<DbContext>(provider => new DbContext(SqlConnectionString));

//Add repoistories
builder.Services.AddScoped<IUserRepository, UserRepository>();


var getSqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(getSqlConnectionString);



//Authentication Configuration
builder.Services.AddAuthentication();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;

}).AddDapperStores(options =>
{
    options.ConnectionString = SqlConnectionString;
});
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? " " : " ")}");

app.UseHttpsRedirection();

app.MapGroup("/account").MapIdentityApi<IdentityUser>();
app.UseAuthorization();

app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
    [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
});

app.MapControllers();

app.Run();
