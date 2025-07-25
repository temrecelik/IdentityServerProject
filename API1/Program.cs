using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

//Her �yelik sistemi i�in bir AddAuthentication servisine �ema ad� verilir => JwtBearerDefaults.AuthenticationScheme default olarak jwt'den ald�k

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {

    options.Authority = "https://localhost:7193"; //AccessToken'� da��tan AuthServer'�n aya�a kald�r�ld��� port/domain
    options.Audience = "resource_api1"; //AuthServer config class'�nda API1 i�in resource yap�land�rmas�

 });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Read", policy =>
    {
        policy.RequireClaim("scope", "api1.read");
    });

    options.AddPolicy("UpdateOrCreateOrDelete", policy =>
    {
        policy.RequireClaim("scope",new[] { "api1.create" ,"api1.update" ,"api1.delete"});
    });

});
    



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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
