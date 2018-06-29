using CheatSheet.Bool;
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
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static int Main(string[] args)
        {
            try
            {
                Logger.InfoFormat("Starts with arguments: {0}", string.Join(" ", args));

                Process1.KillOldSelf();

                //AppDomain.CurrentDomain.UnhandledException += (_, e) =>
                //{
                //    Logger.Error(e);
                //    Outlook1.Create(new[] { "administrator@example.com" }, null, $"{Meta.GetApplicationName()} Error by {Environment.UserName} on {Environment.MachineName} at {DateTime.Now:yyyy-MM-dd HH:mm}", e.ExceptionObject.ToString());
                //};

                #region NonClickOnceOnly

                if (!args.Any() || !String1.ContainsIgnoreCase(new[] { "Foo", "Bar" }, args[0]))
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("Usage 1: {0} Foo");
                    sb.AppendLine("Usage 1: {0} Bar");

                    Console.WriteLine(sb.ToString(), AppDomain.CurrentDomain.FriendlyName);

                    throw new ArgumentOutOfRangeException(string.Join(" ", args));
                }

                #endregion NonClickOnceOnly

                #region ClickOnceOnly

                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    Logger.InfoFormat("ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                    if (ApplicationDeployment.CurrentDeployment.CheckForUpdate() && ApplicationDeployment.CurrentDeployment.Update())
                    {
                        Logger.InfoFormat("Updates self to ClickOnce Publish Version: {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion);

                        Process.Start(ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri);
                    }
                }

                #endregion ClickOnceOnly

                Logger.Info("Exits with 0");

                return 0;
            }
            catch (Exception e)
            {
                Logger.Error(e);

                Logger.Info("Exists with 1");

                return 1;
            }
        }
    }
}