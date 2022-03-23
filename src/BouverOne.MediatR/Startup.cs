using BouvetOne.MediatR.Core.Infrastructure.Validators;
using BouvetOne.MediatR.Extensions;
using FluentValidation;

namespace BouvetOne.MediatR;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
      => services
              .AddLogging()
              .AddValidatorsFromAssembly(typeof(AddSummaryCommandValidator).Assembly)
              .AddMediatR()
              .AddDistributedMemoryCache()
              .AddControllers().Services
              .AddSwaggerGen();

    public void Configure(IApplicationBuilder application, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
            application.UseSwaggerUI();
        }

        application.UseRouting();
        //application.UseAuthentication();
        application.UseAuthorization();
        application.UseSwagger();
        application.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}