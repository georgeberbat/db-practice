using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;

namespace Shared.Services
{
    public class DataProvider : IDataProvider
    {
        public DataProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public Task SaveToExelFile(IEnumerable entityList, string filePath)
        {
            (IEnumerable<object> objects, PropertyInfo[] properties) x = GetExportObjects(entityList);

            var workBook = new XLWorkbook();
            var worksheet = workBook.Worksheets.Add("Лист1");

            var row = 1;
            var column = 1;
            foreach (var property in x.properties)
            {
                worksheet.Cell(row, column).Value = GetPropertyName(property);
                column++;
            }

            row = 2;
            column = 1;
            foreach (var entity in x.objects)
            {
                foreach (var property in x.properties)
                {
                    var propertyValue = property.GetValue(entity);
                    worksheet.Cell(row, column).SetValue(propertyValue?.ToString());
                    column++;
                }

                column = 1;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            workBook.SaveAs(filePath);

            return Task.CompletedTask;
        }

        private static (IEnumerable<object>, PropertyInfo[]) GetExportObjects(IEnumerable entityList)
        {
            var enumerable = entityList as object[] ?? entityList.Cast<object>().ToArray();
            var entityType = enumerable.FirstOrDefault()?.GetType();
            var properties = entityType?.GetProperties() ?? Array.Empty<PropertyInfo>();
            return (enumerable, properties);
        }

        private static string GetPropertyName(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(DisplayNameAttribute),
                false).Cast<DisplayNameAttribute>().SingleOrDefault()?.DisplayName ?? property.Name;
        }
    }
}