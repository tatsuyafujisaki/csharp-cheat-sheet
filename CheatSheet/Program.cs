using CheatSheet.Office;
using log4net;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CheatSheet
{
    static class Program
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static int Main(string[] args)
        {
            Process1.KillOldSelf();

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                Logger.Error(e.ExceptionObject);
                Outlook1.Create(new[] { "administrator@example.com" }, null, $"{Meta.GetApplicationName()} Error by {Environment.UserName} on {Environment.MachineName} at {DateTime.Now:yyyy-MM-dd HH:mm}", e.ExceptionObject.ToString());
            };

            #region NonClickOnceApplicationOnly

            Logger.InfoFormat("Starts with arguments: {0}", string.Join(" ", args));

            var validArguments = new HashSet<string>(new[] { "Foo", "Bar" }, StringComparer.OrdinalIgnoreCase);
            var arguments = new HashSet<string>(args, StringComparer.OrdinalIgnoreCase);

            if (!arguments.Any())
            {
                PrintUsage();
                Logger.Error("Exits with 1 because of no arguments");
                return 1;
            }

            if (!arguments.IsSubsetOf(validArguments))
            {
                PrintUsage();
                Logger.Error("Exits with 1 because not all arguments are valid");
                return 1;
            }

            #endregion NonClickOnceApplicationOnly

            #region ClickOnceApplicationOnly

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Logger.InfoFormat("ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                if (ApplicationDeployment.CurrentDeployment.CheckForUpdate() && ApplicationDeployment.CurrentDeployment.Update())
                {
                    Logger.InfoFormat("Updates self to ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                    Process.Start(ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri);
                }
            }

            #endregion ClickOnceApplicationOnly

            Logger.Info("Exits with 0");
            return 0;
        }

        static void PrintUsage()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Usage 1: {0} Foo");
            sb.AppendLine("Usage 1: {0} Bar");

            Console.WriteLine(sb.ToString(), AppDomain.CurrentDomain.FriendlyName);
        }
    }
}