using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using Consul;
using Contact.API.Data;
using Contact.API.Dto;
using Contact.API.Infrastructure;
using Contact.API.IntergrationEvents.EventHandling;
using Contact.API.Service;
using Contact.API.Services;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;

namespace Contact.API
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
                    options.Audience = "contact_api";
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            #region Consul and  Consul Service Disvovery
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
            #endregion

            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDiscoveryOptions>>().Value;

                return new LookupClient(serviceConfiguration.Consul.DnsEndPoint.ToIpEndPoint());
            });

            #region polly register
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注册全局单例 ResilientClientFactory
            services.AddSingleton<IResilientHttpClientFactory, ResilientClientFactory>(sp =>
           {
               var logger = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
               var httpContextAccesser = sp.GetRequiredService<IHttpContextAccessor>();
               var retryCount = 5;
               var exceptionCountAllowedBeforeBreaking = 5;

               return new ResilientClientFactory(logger, httpContextAccesser, retryCount, exceptionCountAllowedBeforeBreaking);
           });

            //注册全局单例 IHttpClient
            services.AddSingleton<IHttpClient>(sp => sp.GetRequiredService<IResilientHttpClientFactory>().CreateResilientHttpClient());
            #endregion


            services.AddSingleton(new HttpClient());

            services.AddScoped(typeof(ContactContext));
            services.AddScoped<IContactRepository, MongoContactRepository>();
            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserProfileChangedEventHandler>();

            services.AddMvc();

            services.AddCap(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MysqlContact"))
                    .UseRabbitMQ("localhost")
                    .UseDashboard()
                    .UseDiscovery(d =>
                    {
                        d.DiscoveryServerHostName = "localhost";
                        d.DiscoveryServerPort = 8500;
                        d.CurrentNodeHostName = "localhost";
                        d.CurrentNodePort = 9002;
                        d.NodeId = 2;
                        d.NodeName = "CAP ContactAPI Node";
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
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
            app.UseMvc();
            app.UseCap();
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
                    var serviceId = $"{serviceOptions.Value.ContactServiceName}_{address.Host}:{address.Port}";

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
                        Name = serviceOptions.Value.ContactServiceName,
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
                    var serviceId = $"{serviceOptions.Value.ContactServiceName}_{address.Host}:{address.Port}";
                    consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
                }
        }
    }
}
