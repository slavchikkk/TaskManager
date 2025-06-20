using OfficeOpenXml;

namespace TaskManager;

public class Report
{
    public void GenerateByAllTasks(List<Task> data)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Beta Team");
        
        string dateTimeStr = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = $"Report_{dateTimeStr}.xlsx";
        
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells[1, 1].Value = "Отчет по всем задачам";
            
            FillWorksheet(worksheet, data);
            
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
    }

    public void GenerateByUser(List<Task> data, User user)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Beta Team");

        string dateTimeStr = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        var filePath = $"Report_{dateTimeStr}_{user.Name}.xlsx";

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            worksheet.Cells[1, 1].Value = "Отчет по задачам пользователя: ";
            worksheet.Cells[1, 2].Value = user.Name;

            List<Task> currentUserTasks = data.FindAll(task => task.Executor.Id == user.Id);
            FillWorksheet(worksheet, currentUserTasks);

            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
    }

    private void FillWorksheet(ExcelWorksheet worksheet, List<Task> data)
    {
        worksheet.Cells[2, 1].Value = "Task Id";
        worksheet.Cells[2, 2].Value = "Имя";
        worksheet.Cells[2, 3].Value = "Описание";
        worksheet.Cells[2, 4].Value = "Срок";
        worksheet.Cells[2, 5].Value = "Приоритет";
        worksheet.Cells[2, 6].Value = "Комментарий";
        worksheet.Cells[2, 7].Value = "Исполнитель";
        worksheet.Cells[2, 8].Value = "Прогресс";


        int rowNumber = 3;
        foreach (var task in data)
        {
            worksheet.Cells[rowNumber, 1].Value = task.Id;
            worksheet.Cells[rowNumber, 2].Value = task.Name;
            worksheet.Cells[rowNumber, 3].Value = task.Description;
            worksheet.Cells[rowNumber, 4].Value = task.Deadlines;
            worksheet.Cells[rowNumber, 5].Value = task.Priority;
            worksheet.Cells[rowNumber, 6].Value = task.Comment;
            worksheet.Cells[rowNumber, 7].Value = task.Executor.Name;
            worksheet.Cells[rowNumber, 8].Value = task.Progress;

            rowNumber++;
        }
    }
}