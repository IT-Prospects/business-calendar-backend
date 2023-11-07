using BusinessCalendar.Helpers;
using ClosedXML.Excel;

namespace BusinessCalendar.ExcelReports
{
    public static class EventSignUpReport
    {
        public static IXLWorkbook LoadTemplate()
        {
            var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Список участников");
            ExcelHelper.SetCell(sheet, 1, 1, "ФИО");
            ExcelHelper.SetCell(sheet, 1, 2, "Номер телефона");
            ExcelHelper.SetCell(sheet, 1, 3, "Электронная почта");
            ExcelHelper.SetCellsHorizontalAlignmentCenter(sheet, 1, 1, 1, 3);
            return workbook;
        }
    }
}
