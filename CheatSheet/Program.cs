using CheatSheet.Office;
using log4net;
using System;
using System.Deployment.Application;
using System.Reflection;
using System.Text;

namespace CheatSheet
{
    static class Program
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {
            Process1.KillOldSelf();

            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                Logger.Error(e.ExceptionObject);
                Outlook1.Create(new[] { "administrator@example.com" }, null, $"{Meta.GetApplicationName()} Error by {Environment.UserName} on {Environment.MachineName} at {DateTime.Now:yyyy-MM-dd HH:mm}", e.ExceptionObject.ToString());
            };

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Logger.InfoFormat("ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                if (ApplicationDeployment.CurrentDeployment.CheckForUpdate() && ApplicationDeployment.CurrentDeployment.Update())
                {
                    Logger.InfoFormat("Updates self to ClickOnce Publish Version {0}", ApplicationDeployment.CurrentDeployment.UpdatedVersion);
                    ClickOnce.StartSelf();
                    return;
                }

                var arguments = ClickOnce.GetArguments();
            }
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