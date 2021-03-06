using AP.FamilyTree.Web.Areas.Identity;
using AP.FamilyTree.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using AP.FamilyTree.Db;
using AP.FamilyTree.Mail;
using AP.FamilyTree.Web.Data.Services;
using AP.FamilyTree.Web.Data.Services.NodeServices;
using AP.FamilyTree.Web.Data.Services.SystemPageServices;
using AP.FamilyTree.Web.Data.Services.TreesServices;
using AP.FamilyTree.Web.Data.Services.UserServices;
using AP.FamilyTree.Web.Data.SharedService;
using MatBlazor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AP.FamilyTree.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("FamilyTreeDbConnetionString");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddAuthorization(config =>
            {
                config.AddPolicy("User", policy => policy.RequireRole("User", "Administrator"));
                config.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<FamilyTreeDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddMatBlazor();
            services.AddRazorPages();
            services.AddMvcCore();
            services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

            services.AddHttpClient();
            services.AddScoped(r =>
            {
                var client = new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator //()=> { true}
                });
                return client;
            });

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ILoggerProvider, LoggerProvider>();
            services.AddSingleton<IPushNotificationsQueue, PushNotificationsQueue>();

            services.AddHostedService<PushNotificationsDequeuer>();

            services.AddSingleton(Configuration.GetSection("MailSetting").Get<MailSettings>());
            services.AddSingleton<MailingService>();

            services.AddScoped<SystemPageService>();
            services.AddScoped<TreesService>();
            services.AddScoped<NodeService>();
            services.AddScoped<HumanService>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleService>();
            services.AddScoped<AccessService>();
            services.AddScoped<TreeBuidingService>();
            services.AddScoped<ReviewService>();

            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllerRoute("default", "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
