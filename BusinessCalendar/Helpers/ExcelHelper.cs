using ClosedXML.Excel;

namespace BusinessCalendar.Helpers
{
    public static class ExcelHelper
    {
        private static string _templatesPath = @"ExcelTemplates";

        public static void SaveWithFileName(IXLWorkbook xlc, string filename) => xlc.SaveAs(filename);

        public static void Save(IXLWorkbook excel, Stream stream)
        {
            excel.SaveAs(stream);
        }

        public static IXLWorkbook LoadTemplate(string templateName)
        {
            return new XLWorkbook(Path.Combine(_templatesPath, templateName));
        }

        public static IXLWorksheet GetSheet(IXLWorkbook wb, int number) 
            => wb.Worksheet(number);
        public static void SetSheetName(IXLWorksheet ws, string name) 
            => ws.Name = name;
        public static IXLCell GetCell(IXLWorksheet ws, int row, int column) 
            => ws.Cell(row, column);
        public static void SetCell(IXLWorksheet ws, int row, int column, string text)
            => ws.Cell(row, column).SetValue(text);

        public static void SetCellValues(IXLWorksheet ws, int rowStart, int columnStart, object[,] cellValues)
        {
            var rowsMax = cellValues.GetUpperBound(0) + 1;

            var columnsMax = cellValues.GetUpperBound(1) + 1;

            for (var i = 0; i < rowsMax; i++)
            {
                for (var j = 0; j < columnsMax; j++)
                {
                    var r = rowStart + i;
                    var c = columnStart + j;
                    var val = cellValues[i, j];
                    SetObjCell(ws, r, c, val);
                }
            }
        }

        public static void SetObjCell(IXLWorksheet ws, int row, int column, object obj)
        {
            var cell = ws.Cell(row, column);

            if (obj is string stringObj)
            {
                cell.SetValue(stringObj);
            }
            else if (obj is int intObj)
            {
                cell.SetValue(intObj);
            }
            else if (obj is double doubleObj)
            {
                cell.SetValue(doubleObj);
            }
            else if (obj is decimal decimalObj)
            {
                cell.SetValue(decimalObj);
            }
            else if (obj is float floatObj)
            {
                cell.SetValue(floatObj);
            }
            else if (obj is long)
            {
                cell.SetValue(obj.ToString());
            }
            else if (obj is DateTime dateTimeObj)
            {
                cell.SetValue($"{dateTimeObj:dd.MM.yyyy}");
            }
            else if (obj is TimeSpan timeSpanObj)
            {
                cell.SetValue(timeSpanObj);
            }
        }

        public static void SetBorderAtRange(IXLWorksheet sheet, int startRow, int startColumn, int endRow, int endColumn, XLBorderStyleValues border)
        {
            sheet.Range(startRow, startColumn, endRow, endColumn).Style.Border.SetOutsideBorder(border);
            sheet.Range(startRow, startColumn, endRow, endColumn).Style.Border.SetInsideBorder(border);
        }

        public static void AutoSizeCells(IXLWorksheet ws)
        {
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();
        }
    }
}