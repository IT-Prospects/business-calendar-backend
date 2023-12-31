﻿using ClosedXML.Excel;

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

            switch (obj)
            {
                case string stringObj:
                    cell.SetValue(stringObj);
                    break;
                case int intObj:
                    cell.SetValue(intObj);
                    break;
                case double doubleObj:
                    cell.SetValue(doubleObj);
                    break;
                case decimal decimalObj:
                    cell.SetValue(decimalObj);
                    break;
                case float floatObj:
                    cell.SetValue(floatObj);
                    break;
                case long:
                    cell.SetValue(obj.ToString());
                    break;
                case DateTime dateTimeObj:
                    cell.SetValue($"{dateTimeObj:dd.MM.yyyy}");
                    break;
                case TimeSpan timeSpanObj:
                    cell.SetValue(timeSpanObj);
                    break;
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

        public static void SetCellHorizontalAlignmentCenter(IXLWorksheet ws, int row, int column) => ws.Cell(row, column).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

        public static void SetCellsHorizontalAlignmentCenter(IXLWorksheet ws, int startRow, int startColumn, int endRow, int endColumn) => ws.Range(startRow, startColumn, endRow, endColumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}