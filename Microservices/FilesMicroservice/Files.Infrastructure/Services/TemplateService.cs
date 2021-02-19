using Files.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Files.Infrastructure.Services
{
    public class TemplateService<T> : IHostedService where T: class
    {
        private readonly IEnvironmentApplication _environmentApplication;
        private readonly ILogger<TemplateService<T>> _logger;
        private readonly string Folder = "templates";

        public TemplateService(IEnvironmentApplication environmentApplication, ILogger<TemplateService<T>> logger)
        {
            _environmentApplication = environmentApplication ?? throw new ArgumentNullException(nameof(environmentApplication));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("......Create template for {0}......", typeof(T).Name);

            string path = Path.Combine(_environmentApplication.WebRootPath, Folder);
            string fileName = string.Concat(typeof(T).Name, ".xlsx");
            string fullPath = Path.Combine(path, fileName);
            
            CreateFolderIfNotExist(path);
            DeleteFileIfExist(fullPath);

            try
            {
                using ExcelPackage package = new ExcelPackage(new FileInfo(Path.Combine(path, fileName)));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(typeof(T).Name);

                var types = typeof(T).GetProperties();

                worksheet.Protection.IsProtected = true;

                for (int i = 1; i <= types.Length; i++)
                {
                    worksheet.Column(i).Style.Locked = false;
                    worksheet.Column(i).Width = 30;
                    worksheet.Column(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var cell = worksheet.Cells[1, i];

                    cell.Style.Locked = true;
                    cell.Value = types[i - 1].Name;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 14;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Medium);
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                }

                package.Save();

                _logger.LogInformation("......Create template for {0} successfully......", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError("......Failed to create {0} with {1}......", typeof(T).Name, ex.Message);
            }
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
        private void CreateFolderIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                _logger.LogInformation("......Create Folder for {0}......", typeof(T).Name);

                Directory.CreateDirectory(path);
            }
        }
        private void DeleteFileIfExist(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    _logger.LogInformation("......Delete {0}......", typeof(T).Name);
                    File.Delete(path);
                }
                catch (IOException e)
                {
                    _logger.LogError(e.Message);
                }
            }
        }
    }
}
