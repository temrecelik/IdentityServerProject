using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";//Cookie'nin ismi
    options.DefaultChallengeScheme = "OpenIdConnect"; //AuthServer'dan gelen cookie ile haberle�ecek

}).AddCookie("Cookies").AddOpenIdConnect("OpenIdConnect" , options =>
    {
        options.SignInScheme = "Cookies";
        options.Authority = "https://localhost:7193"; //Gelen cookie'yi kim da��tacak
        options.ClientId = "Client1WithUsers"; //Hibrit grant tipi i�in olu�turulan client'�n id'si
        options.ClientSecret = "secret"; //Olu�turulan client'�n �ifresi
        options.ResponseType = "code id_token";//AuthServer'daki authorize endpointine g�nderilen istek sonucu d�nen value'lar.

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
