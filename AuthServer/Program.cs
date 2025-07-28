using AuthServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//IdentityServer Yap�land�r��lmas�
builder.Services.AddIdentityServer().AddInMemoryApiResources(Config.GetApiResource())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddDeveloperSigningCredential() //Token'�n asemetrik olarak imzalanm�s� i�in gereken public ve private key'i development ortam�nda otomatik olarak olu�turur.
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddTestUsers(Config.GetUsers().ToList());


builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();


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

app.UseIdentityServer();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();//Quick UI i�in indirdik

//Direkt olarak Pages klas�r�ndeki home �ndex'e y�nlendirme
app.MapGet("/", context =>
{
    context.Response.Redirect("/Index");
    return Task.CompletedTask;
});

app.Run();
