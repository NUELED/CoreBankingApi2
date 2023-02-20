using CoreBankingApi2.DAL;
using CoreBankingApi2.Services.Implementations;
using CoreBankingApi2.Services.Interfaces;
using CoreBankingApi2.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings")); // Another way of calling from appsettings.json
builder.Services.AddDbContext<BankingDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection")));
builder.Services.AddControllers();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
           x.SwaggerDoc( "v3", new Microsoft.OpenApi.Models.OpenApiInfo
               { 
                 Title ="CoreBankinApi2 Doc",
                 Version = "v3",
                 Description = "A corebanking Api, for processing banking transactions",
                 Contact = new Microsoft.OpenApi.Models.OpenApiContact
                 {
                     Name = "Emmanuel Ekpenyong",
                     Email = "nueledem31@gmail.com",
                     Url = new Uri ("https://github.com/nueled")
                 }
              });

});  // The swagger api info is set here.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( x =>
    {
        var prefix = string.IsNullOrEmpty(x.RoutePrefix) ? "." : "..";
        x.SwaggerEndpoint($"{prefix}/swagger/v3/swagger.json", "A corebanking Api");

    });
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
