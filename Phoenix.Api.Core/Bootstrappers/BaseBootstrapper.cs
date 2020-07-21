using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Api.Core.Bootstrappers
{
    public abstract class BaseBootstrapper : IBootstrapper
    {
        public abstract void ConfigureServices(IServiceCollection services);
        public abstract void Configure(IApplicationBuilder app);
    }
}