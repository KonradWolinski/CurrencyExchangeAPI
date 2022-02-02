using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.Storage.SQLite;
using CurrencyExchangeAPI.Services;
using Hangfire.Storage;
using Hangfire.SqlServer;

namespace CurrencyExchangeAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddDbContext<Models.ExchangeRateContext>(
                options => options.UseSqlite(Configuration.GetConnectionString("ExchangeRateContext")));
            services.AddScoped<INBPWebService, NBPWebService>();
            services.AddHttpClient<NBPWebService>();
            services.AddScoped<INBPRepository, NBPRepository>();
            services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings());
            services.AddHangfireServer();

            var sqlStorage = new SQLiteStorage("HangfireConnection");
            JobStorage.Current = sqlStorage;

            Hangfire.RecurringJob.AddOrUpdate<UpdateData>(x => x.Update(), Hangfire.Cron.Daily);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}
