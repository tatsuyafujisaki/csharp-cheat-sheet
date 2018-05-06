using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace CheatSheet.Office
{
    static class Word1
    {
        // Usage:
        // using Word = Microsoft.Office.Interop.Word;
        // Action<Word.Application> f = w =>
        // {
        //      var d = w.Documents.Open(@"C:\Foo.docx");
        // };
        // Word1.With(f);
        static void With(Action<Word.Application> f)
        {
            Word.Application app = null;

            try
            {
                app = new Word.Application
                {
                    DisplayAlerts = Word.WdAlertLevel.wdAlertsNone,
                    Visible = true
                };
                f(app);
            }
            finally
            {
                if (app != null)
                {
                    if (0 < app.Documents.Count)
                    {
                        // Unlike Excel, Close(...) makes an error when app.Documents.Count == 0
                        // Unlike Excel, Close(...) without wdDoNotSaveChanges shows a prompt for a dirty document.
                        app.Documents.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                    }

                    app.Quit();

                    // Both of the following are needed in some cases
                    // while none of them are needed in other cases.
                    Marshal.FinalReleaseComObject(app);
                    GC.Collect();
                }
            }
        }

        static void SaveWordAsText(Word._Application app, string path)
        {
            var doc = app.Documents.Open(path);
            doc.SaveAs(Path.ChangeExtension(path, ".txt"), Word.WdSaveFormat.wdFormatText, Encoding: 65001);
        }

        static bool HasNoProtection(Word._Document doc) => doc.ProtectionType == Word.WdProtectionType.wdNoProtection;

        static void ProtectWord(Word._Application app, string path, string password)
        {
            var doc = app.Documents.Open(path);
            doc.Protect(Word.WdProtectionType.wdAllowOnlyReading, Password: password);
            doc.Save();
        }

        static void Merge(Word._Application app, string secondWordPath)
           => app.Selection.InsertFile(secondWordPath);

        static void ReconcileWords(Word._Application app, string inPath1, string inPath2, string outPath)
        {
            var doc1 = app.Documents.Add(inPath1);
            var doc2 = app.Documents.Add(inPath2);

            if (new[] { doc1.ProtectionType, doc2.ProtectionType }.Any(pt => pt != Word.WdProtectionType.wdNoProtection))
            {
                app.Documents.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
                return;
            }

            // CompareDocuments(...) makes an error if either one of the two docs is protected.
            app.CompareDocuments(doc1, doc2).SaveAs(outPath);

            app.Documents.Close();
        }

        static void MoveToBeginningOfDocument(Word._Application app)
        {
            app.Selection.HomeKey(Word.WdUnits.wdStory);
        }

        static void MoveToEndOfDocument(Word._Application app)
        {
            app.Selection.EndKey(Word.WdUnits.wdStory);
        }

        static void CreateSectionWithoutHeaderFooter(Word._Application app)
        {
            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);

            Action<Word.HeadersFooters> delete = hfs =>
            {
                var hf = hfs[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

                // Unlink the header and the footer of the last section to those of earlier sections.
                hf.LinkToPrevious = false;

                // Delete the header and the footer of the last section but headers and footers in earlier sections remain the same, thanks to the unlinking.
                hf.Range.Delete();
            };

            var section = app.ActiveDocument.Sections.Last;
            delete(section.Headers);
            delete(section.Footers);

            var ps = app.Selection.PageSetup;
            ps.TopMargin = 0;
            ps.RightMargin = 0;
            ps.BottomMargin = 0;
            ps.LeftMargin = 0;
            ps.HeaderDistance = 0;
            ps.FooterDistance = 0;
        }

        static void InsertFileWithoutHeaderFooter(Word._Application app, string path)
        {
            app.Selection.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            app.Selection.InsertFile(path);

            Action<Word.HeadersFooters> f = hfs =>
            {
                var hf = hfs[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

                // Unlink the header and the footer of the last section to those of earlier sections.
                hf.LinkToPrevious = false;

                // Delete the header and the footer of the last section but headers and footers in earlier sections remain the same, thanks to the unlinking.
                hf.Range.Delete();
            };

            var section = app.ActiveDocument.Sections.Last;
            f(section.Headers);
            f(section.Footers);

            var ps = app.Selection.PageSetup;
            ps.TopMargin = 0;
            ps.RightMargin = 0;
            ps.BottomMargin = 0;
            ps.LeftMargin = 0;
            ps.HeaderDistance = 0;
            ps.FooterDistance = 0;
        }
    }
}