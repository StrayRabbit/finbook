using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using User.API.Data;
using User.API.Dtos;
using User.API.Filters;
using User.API.Models;
using DotNetCore.CAP;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace User.API
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
                    options.Audience = "user_api";
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                });

            services.AddDbContext<UserContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("MysqlUser"));
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

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddCap(options =>
            {
                options.UseEntityFramework<UserContext>()
                    .UseRabbitMQ("localhost")
                    .UseDashboard();

                options.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 9001;
                    d.NodeId = 0;
                    d.NodeName = "CAP UserAPI Node";
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

            //RegisterZipkinTrace(app, loggerFactory, applicationLifetime, "user_api");

            app.UseAuthentication();
            app.UseMvc();
            //InitUserDatabase(app);
            //UserContextSeed.SeedAsync(app, loggerFactory).Wait();
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

            //var client = new ConsulClient();
            //var httpCheck = new AgentServiceCheck()
            //{
            //    DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
            //    Interval = TimeSpan.FromSeconds(30),
            //    HTTP = $"http://localhost:9001/HealthCheck"
            //};

            //var agentReg = new AgentServiceRegistration()
            //{
            //    ID = "servicename:9001",
            //    Check = httpCheck,
            //    Address = "localhost",
            //    Name = "userapi",
            //    Port = 9001
            //};

            //client.Agent.ServiceRegister(agentReg).ConfigureAwait(false);
        }

        private void DeRegisterService(IApplicationBuilder app,
            IOptions<ServiceDiscoveryOptions> serviceOptions,
            IConsulClient consul)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>()
                .Addresses
                .Select(p => new Uri(p));

            foreach (var address in addresses)
            {
                var serviceId = $"{serviceOptions.Value.ServiceName}_{address.Host}:{address.Port}";
                consul.Agent.ServiceDeregister(serviceId).GetAwaiter().GetResult();
            }
        }

        public void RegisterZipkinTrace(IApplicationBuilder app,
            ILoggerFactory loggerFactory,
            IApplicationLifetime lifetime,
            string applicationName)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = 1.0f;
                var logger = new TracingLogger(loggerFactory, "zipkin4net");
                var httpSender = new HttpZipkinSender("http://localhost:9411", "application/json");
                var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new Statistics());

                var consoleTracer = new zipkin4net.Tracers.ConsoleTracer();

                TraceManager.RegisterTracer(tracer);
                TraceManager.RegisterTracer(consoleTracer);
                TraceManager.Start(logger);
            });

            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());

            app.UseTracing(applicationName);
        }
    }
}
