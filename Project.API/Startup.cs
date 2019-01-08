using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using Consul;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Project.API.Applications.Queries;
using Project.API.Applications.Service;
using Project.API.Dto;
using Project.Domain.AggregatesModel;
using Project.Infrastructure;
using Project.Infrastructure.Repositories;

namespace Project.API
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
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //AddAuthentication 则 identity 会帮我们装载 认证token中的claims 到 System.Security.Claims.ClaimsPrincipal User
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:90";     //发送到网关进行，再由网关进行转发到配置的认证服务器
                    options.Audience = "project_api";
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });

            services.AddDbContext<ProjectContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MysqlProject"), sql =>
                    {
                        sql.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    });
            });

            services.Configure<ServiceDiscoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            services.AddScoped<IRecommendService, TestRecommendService>()
                .AddScoped<IProjectQueries, ProjectQueries>(sp => new ProjectQueries(Configuration.GetConnectionString("MysqlProject"), sp.GetRequiredService<ProjectContext>()))
                .AddScoped<IProjectRepository, ProjectRepository>(sp =>
                {
                    var projectContext = sp.GetRequiredService<ProjectContext>();
                    return new ProjectRepository(projectContext);
                });

            services.AddCap(options =>
            {
                options.UseEntityFramework<ProjectContext>()
                    .UseRabbitMQ("localhost")
                    .UseDashboard();

                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 9003;
                    d.NodeId = 1;
                    d.NodeName = "CAP ProjectAPI Node";
                });
            });

            services.AddMediatR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime applicationLifetime,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //开始的时候注册服务
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                RegisterService(app, serviceOptions, consul);
            });

            //停止的时候移除服务
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                DeRegisterService(app, serviceOptions, consul);
            });

            app.UseAuthentication();
            app.UseCap();

            app.UseMvc();
        }

        private void RegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features?.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            if (addresses != null)
                foreach (var address in addresses)
                {
                    var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";

                    var httpCheck = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                        Interval = TimeSpan.FromSeconds(30),
                        HTTP = new Uri(address, "HealthCheck").OriginalString
                    };

                    var registration = new AgentServiceRegistration()
                    {
                        Check = httpCheck,
                        Address = address.Host == "[::]" ? "localhost" : address.Host,
                        ID = serviceId,
                        Name = serviceOptions.Value.ServiceName,
                        Port = address.Port
                    };

                    consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
                }
        }

        private void DeRegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features?.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            if (addresses != null)
                foreach (var address in addresses)
                {
                    var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                }
        }
    }
}
