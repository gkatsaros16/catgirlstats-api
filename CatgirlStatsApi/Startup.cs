using System;
using System.IO;
using System.Threading.Tasks;
using CatgirlStatsLogic.Jobs;
using CatgirlStatsLogic.Services;
using CatgirlStatsModels;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;

namespace CatgirlStatsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IServiceProvider ServiceProvider { get; set; }
        public IServiceCollection Services { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("Enable", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            var config = configBuilder.Build();

            services.AddScoped<IGraphQLClient>(s => new GraphQLHttpClient(Configuration["GraphQLURI"], new NewtonsoftJsonSerializer()));
            services.AddControllers();

            services.AddScoped<ICatgirlStatsConsumer, CatgirlStatsConsumer>();
            services.AddScoped<ICatgirlStatsService, CatgirlStatsService>(); 
            services.AddScoped<IBNBService, BNBService>(); 
            services.AddScoped<ITofuNFTService, TofuNFTService>(); 
            services.AddScoped<INFTradeService, NFTradeService>(); 
            services.AddScoped<ISupportService, SupportService>(); 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatgirlStatsApi", Version = "v1" });
            });
            var secrets = new Secrets {
                CatgirlStatsDBPass = Configuration["CATGIRLSTATSDBPASS"] ?? ""
            };
            services.AddSingleton(secrets);
            // ServiceProvider = services.BuildServiceProvider();
            // Scheduler();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatgirlStatsApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // private async void Scheduler() 
        // {
        //     // Grab the Scheduler instance from the Factory
        //     StdSchedulerFactory factory = new StdSchedulerFactory();
        //     IScheduler scheduler = await factory.GetScheduler();
        //     // and start it off
        //     await scheduler.Start();

        //     // define the job and tie it to our HelloJob class
        //     IJobDetail job = JobBuilder.Create<HelloJob>()
        //         .WithIdentity("job1", "group1")
        //         .Build();

        //     // Trigger the job to run now, and then repeat every 10 seconds
        //     ITrigger trigger = TriggerBuilder.Create()
        //         .WithIdentity("trigger1", "group1")
        //         .StartNow()
        //         .WithSimpleSchedule(x => x
        //             .WithIntervalInSeconds(2)
        //             .RepeatForever())
        //         .Build();

        //     // Tell quartz to schedule the job using our trigger
        //     await scheduler.ScheduleJob(job, trigger);

        //     // some sleep to show what's happening
        //     await Task.Delay(TimeSpan.FromSeconds(60));

        //     // and last shut down the scheduler when you are ready to close your program
        //     await scheduler.Shutdown();
        // } 

        // public class HelloJob : IJob
        // {
        //     public async Task Execute(IJobExecutionContext context)
        //     {
        //         await Console.Out.WriteLineAsync("Greetings from HelloJob!");
        //         var bnbService = ServiceProvider.GetService<BNBService>();
        //         await bnbService.SetBNBCurrentPrice();
        //     }
        // }
    }
}
