using HeijunkaAppLibrary.Data;
using HeijunkaAppLibrary.Databases;
using Newtonsoft.Json.Serialization;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Values travelling from Controller to View view ActionResult will use Pascal Case
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = null
);

// Resolve Time Zone Issue
builder.Services.AddMvc();
builder.Services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson();
builder.Services.AddMvc()
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
    .AddNewtonsoftJson(opt => opt.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local);


// Dependency Injection
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IDatabaseData, SqlData>();

if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion")))
{
    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2")))
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"scripts", @"ej2.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2", @"ej2.min.js"));
    }

    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2")))
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap4.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap4.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"material.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"material.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"highcontrast.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"highcontrast.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"fabric.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"fabric.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"tailwind.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"tailwind.css"));
        File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap5.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap5.css"));
    }
}
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Heijunka}/{action=Timeline}/{id?}");

app.Run();

