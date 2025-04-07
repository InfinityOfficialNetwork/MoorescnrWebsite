using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Metrics;
using MoorescnrWebsite.Database;
using System.IO.Compression;

namespace MoorescnrWebsite
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Logging.SetMinimumLevel(LogLevel.Trace);

			if (OperatingSystem.IsWindows())
				builder.Logging.AddEventLog(o => {
					o.LogName = "MoorescnrWebsite";
					o.SourceName = "MoorescnrWebsite";
				});

			builder.Metrics.EnableMetrics(null);

			builder.Services.AddHttpLogging();

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.Configure<GzipCompressionProviderOptions>(options =>
			{
				options.Level = CompressionLevel.SmallestSize;
			});

			string connectionString = builder.Configuration.GetConnectionString("DatabaseConnection") ?? throw new InvalidOperationException("ConnectionString:DatabaseConnection string missing, check config.");
			builder.Services.AddDbContextFactory<CnrDbContext>(options =>
				options.UseSqlite(connectionString)
				.EnableSensitiveDataLogging());

			builder.Services.AddResponseCaching()
				.AddOutputCache()
				.AddMemoryCache();

			builder.Services.AddMemoryCache();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseHttpsRedirection();
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}




			app.UseRouting();

			app.UseAuthorization();

			app.MapStaticAssets();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			await app.RunAsync();
		}
	}
}
