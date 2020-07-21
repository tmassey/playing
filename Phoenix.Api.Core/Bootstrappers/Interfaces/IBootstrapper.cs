using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Api.Core.Bootstrappers
{
    public interface IBootstrapper
    {
        void ConfigureServices(IServiceCollection services);
        void Configure(IApplicationBuilder app);
    }
}