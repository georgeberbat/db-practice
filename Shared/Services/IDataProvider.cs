using System.Collections;

namespace Shared.Services
{
    public interface IDataProvider
    {
        Task SaveToExelFile(IEnumerable entityList, string filePath);
    }
}