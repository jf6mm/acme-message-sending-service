using System;
using System.Threading;
using System.Threading.Tasks;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.AutoMessageSender.Service
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IBirthdayNotifier _birthdayNotifier;
		private readonly IWorkAnniversaryNotifier _workAnniversaryNotifier;
		private readonly AppSettings _appSettings;

		public Worker(ILogger<Worker> logger, IBirthdayNotifier birthdayNotifier, IWorkAnniversaryNotifier workAnniversaryNotifier,
			IOptions<AppSettings> appSettings)
		{
			_logger = logger;
			_birthdayNotifier = birthdayNotifier;
			_workAnniversaryNotifier = workAnniversaryNotifier;
			_appSettings = appSettings.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Executing notification tasks at: {time}", DateTimeOffset.Now);

				try
				{
					await _birthdayNotifier.NotifyEmployees();
					// await _workAnniversaryNotifier.NotifyEmployees(); // TODO: uncomment once implemented.

					_logger.LogInformation("Finished executing notification tasks at: {time}", DateTimeOffset.Now);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Exception while executing notification tasks");
				}

				// Sleep for configured period
				int millisecondsToDelay = _appSettings.NotificationTaskDelayMinutes * 60 * 1000;
				await Task.Delay(millisecondsToDelay, stoppingToken);
			}
		}
	}
}