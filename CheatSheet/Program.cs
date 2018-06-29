using log4net;
using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CheatSheet
{
    static class Program
    {
        static int Main(string[] args)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            // AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            // {
            //     Logger.Error(e);
            //     Outlook1.Create(new[] { "administrator@example.com" }, null, $"{Meta.GetApplicationName()} Error by {Environment.UserName} on {Environment.MachineName} at {DateTime.Now:yyyy-MM-dd HH:mm}", e.ExceptionObject.ToString());
            // };

            try
            {
                Process1.KillOldSelf();


                #region NonClickOnceOnly

                if (args.Any())
                {
                    logger.InfoFormat("Starts with arguments: {0}", string.Join(" ", args));
                }
                else
                {
                    logger.Info("Starts with no arguments");

                    PrintUsage();

                    throw new ArgumentOutOfRangeException();
                }

                #endregion NonClickOnceOnly

                #region ClickOnceOnly

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    logger.InfoFormat("ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                    if (ApplicationDeployment.CurrentDeployment.CheckForUpdate() && ApplicationDeployment.CurrentDeployment.Update())
                    {
                        logger.InfoFormat("Updates self to ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                        Process.Start(ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri);
                    }
                }

                #endregion ClickOnceOnly

                logger.Info("Exits with 0");

                return 0;
            }
            catch (Exception e)
            {
                logger.Error(e);

                logger.Info("Exists with 1");

                return 1;
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