using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DataSett.ViewModel.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register services
builder.Services.AddSingleton<BusinessDomainService>();

await builder.Build().RunAsync();
