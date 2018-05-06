using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace CheatSheet.Office
{
    static class Excel1
    {
        // Usage:
        // using Excel = Microsoft.Office.Interop.Excel;
        // Action<Excel.Application> f = e =>
        // {
        //     var wb = e.Workbooks.Open(@"C:\Foo.xlsx");
        // };
        // Excel1.With(f);
        static void With(Action<Excel.Application> f)
        {
            Excel.Application app = null;

            try
            {
                app = new Excel.Application
                {
                    DisplayAlerts = false,
                    Visible = true
                };

                f(app);
            }
            finally
            {
                if (app != null)
                {
                    // Unlike Word, Close() does not make any error even when app.Workbooks.Count == 0.
                    // Unlike Word, Close() does not show any prompt for a dirty workbook.
                    app.Workbooks.Close();
                    app.Quit();

                    // Both of the following are needed in some cases
                    // while none of them are needed in other cases.
                    Marshal.FinalReleaseComObject(app);
                    GC.Collect();
                }
            }
        }

        static Excel.Worksheet CopyWorksheetToTop(Excel._Workbook wb, int sourceWorksheetIndex)
        {
            wb.Worksheets[sourceWorksheetIndex].Copy(wb.Worksheets[1]);
            return wb.Worksheets[1];
        }

        static Excel.Worksheet CopyWorksheetToBottom(Excel._Workbook wb, int sourceWorksheetIndex)
        {
            wb.Worksheets[sourceWorksheetIndex].Copy(Type.Missing, wb.Worksheets[wb.Worksheets.Count]);
            return wb.Worksheets[wb.Worksheets.Count];
        }

        static void MergeExcels(Excel._Application app, IEnumerable<string> sourcePaths, string destinationPath)
        {
            var dstWb = app.Workbooks.Add("");
            var srcWbs = sourcePaths.Select(sourcePath => app.Workbooks.Add(sourcePath));

            // Don't start with i = 1 because trying to delete the last sheet makes an error. 
            for (var i = dstWb.Worksheets.Count; 2 <= i; i--)
            {
                dstWb.Worksheets[i].Delete();
            }

            // Keep the last sheet to be deleted when it is no longer the last sheet.
            Excel.Worksheet ws1 = dstWb.Worksheets[1];

            // app.Workbooks[1] is a destination so needs to be skipped.
            for (var i = app.Workbooks.Count; 2 <= i; i--)
            {
                var srcWb = app.Workbooks[i];
                for (var j = srcWb.Worksheets.Count; 1 <= j; j--)
                {
                    srcWb.Worksheets[j].Copy(dstWb.Worksheets[1]);
                }
            }

            ws1.Delete();
            dstWb.SaveAs(destinationPath);

            foreach (var srcWb in srcWbs)
            {
                srcWb.Close();
            }
        }

        static void SaveAsCsv(Excel._Application app, string path)
        {
            var wb = app.Workbooks.Add(path);

            foreach (Excel.Worksheet ws in wb.Worksheets)
            {
                ws.Activate();
                wb.SaveAs(Path.ChangeExtension(Io.File1.AppendSuffix(path, "-" + ws.Name), ".csv"), Excel.XlFileFormat.xlCSV);
            }

            app.Workbooks.Close();
        }

        static void CreateHelloWorldExcel(Excel._Application app)
        {
            Excel.Worksheet ws = app.Workbooks.Add().Worksheets[1];
            ws.Cells[1, 1] = "Hello World";
            ws.SaveAs(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "HelloWorld.xlsx"));
        }
    }
}