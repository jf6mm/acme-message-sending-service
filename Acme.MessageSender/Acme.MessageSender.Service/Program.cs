using Acme.MessageSender.Common.Models.Mapping;
using Acme.MessageSender.Common.Models.Settings;
using Acme.MessageSender.Core.Interfaces;
using Acme.MessageSender.Core.Services;
using Acme.MessageSender.Core.Services.EmployeeNotification;
using Acme.MessageSender.Infrastructure.ApiAgents;
using Acme.MessageSender.Infrastructure.Email;
using Acme.MessageSender.Infrastructure.FileSystem;
using Acme.MessageSender.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.AutoMessageSender.Service
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseWindowsService()
				.ConfigureLogging((context, loggingBuilder) =>
				{
					loggingBuilder.ClearProviders();
					loggingBuilder.AddFile(context.Configuration.GetSection("Logging"));
					loggingBuilder.AddConsole();
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();

					services.Configure<AppSettings>(hostContext.Configuration.GetSection("AppSettings"));

					services.AddSingleton<IEmployeeApiAgent, EmployeeApiAgent>();
					services.AddSingleton<IBirthdayNotifier, BirthdayNotifier>();
					services.AddSingleton<IWorkAnniversaryNotifier, WorkAnniversaryNotifier>();
					services.AddSingleton<IEmailAgent, EmailAgent>();
					services.AddSingleton<IFileSystemAgent, FileSystemAgent>();
					services.AddSingleton<IEmailRegisterFileAgent, EmailRegisterFileAgent>();
					services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
					services.AddSingleton<IEmployeeDateCalculator, EmployeeDateCalculator>();

					var mappingConfig = ModelConfiguration.CreateMapperConfiguration();
					IMapper mapper = mappingConfig.CreateMapper();
					services.AddSingleton(mapper);
				});
	}
}
