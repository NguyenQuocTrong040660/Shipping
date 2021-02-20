using Files.Application.Common.Interfaces;
using Files.Domain.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

            string jsonString = GetJsonStringFromExcel<T>(fullPath);
            return JsonConvert.DeserializeObject<List<T>>(jsonString);
        }

        public List<T> ValidateData<T>(string dataJson)
        {
            var itemInvalids = new List<T>();

            var data = JsonConvert.DeserializeObject<List<T>>(dataJson);

            foreach (var item in data)
            {
                var properties = typeof(T).GetProperties();
               
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(item);
                    var customeAttr = prop.GetCustomAttribute<ValidateDataType>();

                    if (customeAttr == null)
                    {
                        continue;
                    }
                    
                    if (customeAttr.IsRequired)
                    {
                        if (value == null)
                        {
                            itemInvalids.Add(item);
                            continue;
                        }
                    }

                    string valueString = value.ToString();

                    if (customeAttr.IsNumber)
                    {
                        if (!int.TryParse(valueString, out _))
                        {
                            itemInvalids.Add(item);
                            continue;
                        }
                    }

                    if (customeAttr.IsDateTime)
                    {
                        if (!DateTime.TryParse(valueString, out _))
                        {
                            itemInvalids.Add(item);
                            continue;
                        }
                    }
                }
            }

            return itemInvalids;
        }

        private static string GetJsonStringFromExcel<T>(string path, bool hasHeader = true)
        {
            List<dynamic> models = new List<dynamic>();

            using (ExcelPackage excelPackage = new ExcelPackage(File.OpenRead(path)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                int rows = worksheet.Dimension.End.Row;
                int columns = worksheet.Dimension.End.Column;

                int startRow = hasHeader ? worksheet.Dimension.Start.Row + 1 : worksheet.Dimension.Start.Row;
                int startColumn = worksheet.Dimension.Start.Column;

                var types = typeof(T).GetProperties();

                for (int i = startRow; i <= rows; i++)
                {
                    dynamic model = new JObject();

                    for (int j = startColumn; j <= types.Length; j++)
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
