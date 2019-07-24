namespace Emon.Services.Services
{
    using Microsoft.Extensions.Logging;

    using System;
    using System.Timers;

    using Topshelf;
    using Topshelf.Runtime;

    /// <summary>
    /// Defines the <see cref="TestService" />
    /// </summary>
    public class TestService : ServiceControl
    {
        #region Fields

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<TestService> _logger;

        /// <summary>
        /// Defines the _timer
        /// </summary>
        private readonly Timer _timer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestService"/> class.
        /// </summary>
        /// <param name="settings">The settings<see cref="HostSettings"/></param>
        /// <param name="logger">The logger<see cref="ILogger{TestService}"/></param>
        public TestService(HostSettings settings, ILogger<TestService> logger)
        {
            _logger = logger;
            _timer = new Timer(30)
            {
                AutoReset = true,
                Enabled = true
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Continue
        /// </summary>
        public void Continue()
        {
            _timer.Start();
        }

        /// <summary>
        /// The Pause
        /// </summary>
        public void Pause()
        {
            _timer.Stop();
        }

        /// <summary>
        /// The Shutdown
        /// </summary>
        public void Shutdown()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        /// <summary>
        /// The Start
        /// </summary>
        /// <param name="hostControl">The hostControl<see cref="HostControl"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Start(HostControl hostControl)
        {
            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(5));


            _timer.Elapsed += (s, e) =>
            {
                _timer.Stop();

                _logger.LogInformation("Timer Stoped at {SignalTime}", e.SignalTime);
                _timer.Start();
            };
            return true;
        }

        /// <summary>
        /// The Stop
        /// </summary>
        /// <param name="hostControl">The hostControl<see cref="HostControl"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool Stop(HostControl hostControl)
        {
            _timer.Stop();
            _timer.Dispose();
            hostControl.Stop();
            return true;
        }

        #endregion
    }
}
