using ModuloPagamentos.Repositories;
using ModuloPagamentos.Services;
using Microsoft.AspNetCore.Hosting;

namespace ModuloPagamentos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Register repository and service for DI
            builder.Services.AddSingleton<ICartaoRepository>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                var filePath = Path.Combine(env.ContentRootPath, "Data", "cartoes.csv");
                return new CartaoRepository(filePath);
            });
            builder.Services.AddSingleton<CartaoService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
