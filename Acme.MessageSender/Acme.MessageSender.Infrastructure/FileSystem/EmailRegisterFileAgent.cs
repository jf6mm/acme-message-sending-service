using Acme.MessageSender.Common.Helpers;
using Acme.MessageSender.Common.Models.Dto;
using Acme.MessageSender.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Acme.MessageSender.Infrastructure.FileSystem
{
	public class EmailRegisterFileAgent : IEmailRegisterFileAgent
	{
		private const string DirectoryPath = "data";
		private const string FileName = "EmailRegister.dat";
		private readonly IFileSystemAgent _fileSystemAgent;
		private readonly ILogger _logger;

		private string FilePath
		{
			get
			{
				return string.IsNullOrEmpty(DirectoryPath)
				  ? FileName
				  : string.Concat(DirectoryPath, "/", FileName);
			}
		}

		public EmailRegisterFileAgent(IFileSystemAgent fileSystemAgent, ILogger<EmailRegisterFileAgent> logger)
		{
			_fileSystemAgent = fileSystemAgent;
			_logger = logger;
		}

		#region Public Methods

		public SentEmailRegister GetEmailRegisterData()
		{
			string fileContent = _fileSystemAgent.ReadFileText(FilePath);
			try
			{
				return fileContent == null
				? null
				: JsonConvert.DeserializeObject<SentEmailRegister>(fileContent);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, $"Exception while attempting to deserialize JSON from {FilePath}");
				return null;
			}
		}

		public SentEmailRegister GetEmailRegisterDataForToday()
		{
			string fileContent = _fileSystemAgent.ReadFileText(FilePath);
			try
			{
				// If data returned is null return a new object with today's date.
				var sentEmailRegister = JsonConvert.DeserializeObject<SentEmailRegister>(fileContent);
				if (sentEmailRegister == null) return CreateNewEmailRegister();

				// if the data's datestamp is not today's date return a new object with today's date.
				var fileDate = DateTimeConverter.FromMillisecondsToDateTime(sentEmailRegister.FileDateEpoch);
				if (!(fileDate.Date == DateTime.Now.Date)) return CreateNewEmailRegister();

				// The data is still from today, return as is.
				return sentEmailRegister;
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, $"Exception while attempting to deserialize JSON from {FilePath}");
				return CreateNewEmailRegister();
			}
		}

		public void SaveEmailRegisterData(SentEmailRegister sentEmailRegister)
		{
			if (!string.IsNullOrEmpty(DirectoryPath))
			{
				_fileSystemAgent.CreateDirectory(DirectoryPath);
			}

			string fileContent = JsonConvert.SerializeObject(sentEmailRegister);
			_fileSystemAgent.WriteFileText(FilePath, fileContent);
		}

		#endregion

		#region Private Methods

		private SentEmailRegister CreateNewEmailRegister()
		{
			return new SentEmailRegister
			{
				FileDateEpoch = DateTimeConverter.FromDateTimeToMilliseconds(DateTime.Now.Date)
			};
		}

		#endregion
	}
}