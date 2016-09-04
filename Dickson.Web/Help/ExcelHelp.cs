using Dickson.Core.ComponentModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Dickson.Web.Helper
{
   public class ExcelHelp
    {
        public static FileStreamResult Export(string[] titles, string fileName, Action<ExcelWorksheet> rendContent)
        {
            Requires.NotNull(titles, nameof(titles));
            Requires.NotNullOrEmpty(fileName, nameof(fileName));
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(fileName);
                for (var cell = 0; cell < titles.Length; cell++)
                {
                    ws.Cells[1, cell + 1].Value = titles[cell];
                }
                if (rendContent != null)
                {
                    rendContent(ws);
                }
                var fileStream = new MemoryStream();
                package.SaveAs(fileStream);
                fileStream.Position = 0;
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var result = new FileStreamResult(fileStream, contentType);
                result.FileDownloadName = $"{fileName}.xlsx";
                return result;
            }
        }
    }
}
