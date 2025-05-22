using EncryptedFileApp.Components;
using EncryptedFileApp.Data;
using EncryptedFileApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddSingleton<FileEncryptionService>();


builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<AzureKeyVaultService>();


builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AuthState>());


builder.Services.AddAuthorizationCore();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection(); 
app.UseAntiforgery();      

app.MapStaticAssets();    
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); 

app.Run();
