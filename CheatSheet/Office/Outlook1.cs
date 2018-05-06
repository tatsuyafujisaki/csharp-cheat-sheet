using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace CheatSheet.Office
{
    static class Outlook1
    {
        // Usage:
        // using Outlook = Microsoft.Office.Interop.Outlook;
        // Action<Outlook.Application> f = o =>
        // {
        //     var mapif = o.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);
        // };
        // Outlook1.With(f);
        static void With(Action<Outlook.Application> f)
        {
            Outlook.Application app = null;

            try
            {
                app = new Outlook.Application();
                f(app);
            }
            finally
            {
                if (app != null)
                {
                    app.Quit();

                    // Both of the following are needed in some cases,
                    // while none of thme are needed in other cases.
                    Marshal.FinalReleaseComObject(app);
                    GC.Collect();
                }
            }
        }

        internal static void Create(string[] to, string[] cc, string subject, string body, string[] attachments = null)
        {
            var app = new Outlook.Application();

            // As the return type is dynamic, it is recommended to specify its type.
            Outlook.MailItem mi = app.CreateItem(Outlook.OlItemType.olMailItem);

            if (to != null)
            {
                mi.To = string.Join(";", to);
            }

            if (cc != null)
            {
                mi.CC = string.Join(";", cc);
            }

            // No error even when subject is null.
            mi.Subject = subject;

            // No error even when body is null.
            mi.Body = body;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mi.Attachments.Add(attachment);
                }
            }

            mi.Display();

            // mi.attachments is always not null.
            foreach (var attachment in mi.Attachments)
            {
                Marshal.FinalReleaseComObject(attachment);
            }

            // Both Marshal.FinalReleaseComObject(...) and GC.Collect() are needed in some cases
            // while none of them are needed in other cases.
            Marshal.FinalReleaseComObject(mi.Attachments);
            Marshal.FinalReleaseComObject(mi);
            Marshal.FinalReleaseComObject(app);

            GC.Collect();
        }

        static Outlook.MailItem ReadMsgFile(Outlook._Application app, string path) => app.Session.OpenSharedItem(path);
    }
}