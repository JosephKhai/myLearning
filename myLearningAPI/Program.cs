global using myLearningAPI;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks()
    .AddCheck("ICMP_01", new IcmpHealthCheck("www.ryadel.com", 100))
    .AddCheck("ICMP_02", new IcmpHealthCheck("www.google.com.au", 100))
    .AddCheck("ICMP_3", new IcmpHealthCheck($"www.{Guid.NewGuid():N}.com", 100));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks(new PathString("/api/health"), new CustomHealthCheckOptions());

app.MapControllers();

app.Run();
