using DataSett.ViewModel;
using DataSett.ViewModel.Services;
using DataSettWorkbench.Components;

using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure AppSettings from appsettings.json and environment variables
builder.Services.Configure<DataSett.ViewModel.AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSingleton<IMetaDataIOService, MetaDataIOService>();

builder.Services.AddScoped<WorkbenchMainViewmodel>();
builder.Services.AddScoped<BusinessDomainViewModel>();
builder.Services.AddScoped<BusinessConceptViewModel>();
builder.Services.AddScoped<BusinessConceptRelationViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
