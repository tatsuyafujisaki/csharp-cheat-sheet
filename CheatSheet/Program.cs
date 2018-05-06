using CheatSheet.Office;
using log4net;
using System;
using System.Deployment.Application;

namespace CheatSheet
{
    static class Program
    {
        static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

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
                    Logger.InfoFormat("Updating to ClickOnce Publish Version {0} ...", ApplicationDeployment.CurrentDeployment.UpdatedVersion);
                    ClickOnce.StartSelf();
                    return;
                }

                var argument = ClickOnce.GetArgument();
            }

            Console.WriteLine("Press any key to continue ..."); // The same message as from the "pause" command of batch programs.
            Console.ReadKey();
        }
    }
}