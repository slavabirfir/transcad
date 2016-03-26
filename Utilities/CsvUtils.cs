using System.Data;
using System.Text;

namespace Utilities
{
    public static class CsvUtils
    {
        public static void WriteCsvFile(string fileName, DataTable dataTable, Encoding encoding)
        {
            using (var writer = new CsvWriter())
            {
                writer.WriteCsv(dataTable, fileName, encoding);//
            }
        }
    }
}