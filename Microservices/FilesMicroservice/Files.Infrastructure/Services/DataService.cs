﻿using Files.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Files.Infrastructure.Services
{
    public class DataService : IDataService
    {
        private readonly ILogger<DataService> _logger;
        private readonly IEnvironmentApplication _environmentApplication;

        public DataService(IEnvironmentApplication environmentApplication, ILogger<DataService> logger)
        {
            _environmentApplication = environmentApplication ?? throw new ArgumentNullException(nameof(environmentApplication));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<T> ReadFromExcelFile<T>(string path)
        {
            string fullPath = string.Concat(_environmentApplication.WebRootPath, path);

            string jsonString = GetJsonStringFromExcel(fullPath);
            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }

        private static string GetJsonStringFromExcel(string path, bool hasHeader = true)
        {
            List<dynamic> models = new List<dynamic>();

            using (ExcelPackage excelPackage = new ExcelPackage(File.OpenRead(path)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                int rows = worksheet.Dimension.End.Row;
                int columns = worksheet.Dimension.End.Column;

                int startRow = hasHeader ? worksheet.Dimension.Start.Row + 1 : worksheet.Dimension.Start.Row;
                int startColumn = worksheet.Dimension.Start.Column;

                for (int i = startRow; i <= rows; i++)
                {
                    dynamic model = new JObject();

                    for (int j = startColumn; j <= columns; j++)
                    {
                        if (worksheet.Cells[i, j].Value != null)
                        {
                            string keyName = worksheet.Cells[1, j].Value.ToString();
                            model[keyName] = worksheet.Cells[i, j].Value.ToString();
                        }
                    }
                    models.Add(model);
                }
            }

            return JsonConvert.SerializeObject(models);
        }
    }
}