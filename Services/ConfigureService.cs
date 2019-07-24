namespace Emon.Services
{
    using Emon.Services.Services;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using System;

    using Topshelf;

    /// <summary>
    /// Defines the <see cref="ConfigureService" />
    /// </summary>
    public class ConfigureService
    {
        #region Methods

        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="serviceProvider">The serviceProvider<see cref="IServiceProvider"/></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            var rc = HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.SetServiceName("ServiceSvc");
                hostConfigurator.SetDisplayName(" Service Name");
                hostConfigurator.SetDescription("Common Services");


                hostConfigurator.UseSerilog();

                hostConfigurator.Service<TestService>(sc =>
                {
                    sc.ConstructUsing(f => new TestService(f, serviceProvider.GetService<ILogger<TestService>>()));
                    sc.WhenStarted((s, h) => s.Start(h));
                    sc.WhenStopped((s, h) => s.Stop(h));
                    // optional pause/continue methods if used
                    sc.WhenPaused(s => s.Pause());
                    sc.WhenContinued(s => s.Continue());

                    // optional, when shutdown is supported
                    sc.WhenShutdown(s => s.Shutdown());
                });
                hostConfigurator.OnException(error =>
                {
                    var logger = serviceProvider.GetService<ILogger>();
                    logger.LogError(error, "Error On Service Running");
                });
                hostConfigurator.EnablePauseAndContinue();
                hostConfigurator.RunAsLocalSystem();
                hostConfigurator.StartAutomatically();
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        #endregion
    }
}
