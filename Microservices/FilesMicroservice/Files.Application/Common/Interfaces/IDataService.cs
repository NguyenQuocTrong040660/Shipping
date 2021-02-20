using System.Collections.Generic;

namespace Files.Application.Common.Interfaces
{
    public interface IDataService
    {
        List<T> ReadFromExcelFile<T>(string path);
        List<T> ValidateData<T>(string dataJson);
    }
}
