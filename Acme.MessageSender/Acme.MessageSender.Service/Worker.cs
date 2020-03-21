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
		private readonly IBirthdayMessageSender _birthdayMessageSender;

		public Worker(ILogger<Worker> logger, IBirthdayMessageSender birthdayMessageSender)
		{
			_logger = logger;
			_birthdayMessageSender = birthdayMessageSender;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}