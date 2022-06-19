using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System.Data;
using ZDToolbox.Classes;

namespace ZDToolbox.Helpers
{
    public static class ExcelHelper
    {
        public static Stream CreateExcel<T>(IEnumerable<T> list, string sheetName = "گزارش")
        {
            var excelFileName = Path.GetTempPath() + DateTime.Now.Ticks + ".xlsx";
            // Datatable is most easy way to deal with complex datatypes for easy reading and formatting. 
            var table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(list), (typeof(DataTable)));

            using (var document = SpreadsheetDocument.Create(excelFileName, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sheetName };

                sheets.Append(sheet);

                var headerRow = new Row();

                var columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    var cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(column.ColumnName)
                    };
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dataRow in table.Rows)
                {
                    var newRow = new Row();
                    foreach (var cell in columns.Select(col => new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(dataRow[col].ToString())
                    }))
                    {
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();

            }
            var fileByte = File.ReadAllBytes(excelFileName);
            File.Delete(excelFileName);
            return new MemoryStream(fileByte);
        }
        public static List<KeyValue<string, List<string>>> ReadExcel(Stream stream)
        {
            using var spreadsheetDocument = SpreadsheetDocument.Open(stream, true);
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var worksheetPart = workbookPart.WorksheetParts.First();
            var sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            var headers = GetHeaders(sheetData.Elements<Row>().First(), workbookPart);
            var values = GetValues(sheetData.Elements<Row>(), workbookPart);
            foreach (var header in headers.Where(header => values.Exists(x => x.Key == header.Value)))
                values.First(x => x.Key == header.Value).Key = header.Key;
            return values;
        }
        private static List<KeyValue<string, List<string>>> GetValues(IEnumerable<Row> rows, WorkbookPart workbookPart)
        {
            var values = new List<KeyValue<string, List<string>>>();

            foreach (var row in rows)
            {
                if (row.RowIndex == 1)
                    continue;
                foreach (var c in row.Elements<Cell>())
                {
                    var key = c.CellReference.ToString()[0].ToString();
                    if (!values.Exists(x => x.Key == key))
                        values.Add(new KeyValue<string, List<string>>()
                        {
                            Key = key,
                            Value = new List<string>()
                        });
                    if (key == "A") values.First(x => x.Key == key).Value.Add(c.InnerText);
                    else
                        values.First(x => x.Key == key).Value.Add(workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>()
                            .ElementAt(Convert.ToInt32(c.InnerText))
                            .InnerText);
                }
            }

            return values;
        }
        private static IEnumerable<KeyValue<string, string>> GetHeaders(Row r, WorkbookPart workbookPart)
        {
            return (from c in r.Elements<Cell>()
                    where c.DataType != null && c.DataType == CellValues.SharedString
                    select new KeyValue<string, string>()
                    {
                        Key = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>()
                            .ElementAt(Convert.ToInt32(c.InnerText))
                            .InnerText,
                        Value = c.CellReference.ToString()[0].ToString()
                    }).ToList();
        }
    }
}
