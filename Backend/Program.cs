using DotNetEnv;
using Models;

var builder = WebApplication.CreateBuilder(args);


// Load environment variables from .env file
DotNetEnv.Env.Load();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<StripeSettings>(options =>
{
    options.PublicKey = Env.GetString("STRIPE_PUBLIC_KEY");
    options.SecretKey = Env.GetString("STRIPE_SECRET_KEY");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
