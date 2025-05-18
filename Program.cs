using EncryptedFileApp.Components;
using EncryptedFileApp.Data;
using EncryptedFileApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Tilføj logging og konfiguration automatisk

// Tilføj services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<FileEncryptionService>();
builder.Services.AddHttpContextAccessor();

// AzureKeyVaultService skal have IConfiguration og ILogger indsprøjtet
builder.Services.AddScoped<AzureKeyVaultService>();

// Authentication og DbContext som før
builder.Services.AddScoped<AppAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<AppAuthenticationStateProvider>());

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
