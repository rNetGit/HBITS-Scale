using HBITSExplorer.Components;
using HBITSExplorer.Services;
//using HBITSExplorer.Data; // your services
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<RateService>();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ? Serve CSS/JS
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
