using System;
using System.Threading;
using System.Threading.Tasks;
using Acme.MessageSender.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.AutoMessageSender.Service
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IBirthdayNotifier _birthdayNotifier;
		private readonly IWorkAnniversaryNotifier _workAnniversaryNotifier;


		public Worker(ILogger<Worker> logger, IBirthdayNotifier birthdayNotifier, IWorkAnniversaryNotifier workAnniversaryNotifier)
		{
			_logger = logger;
			_birthdayNotifier = birthdayNotifier;
			_workAnniversaryNotifier = workAnniversaryNotifier;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _birthdayNotifier.NotifyEmployees();

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}