using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;

namespace CheatSheet
{
    enum LaunchType
    {
        DotApplication, // .application cannot take any argument.
        DotApprefMsWithoutArgument,
        DotApprefMsWithArgument,
    }

    static class ClickOnce
    {
        static LaunchType GetLaunchType()
        {
            if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData == null)
            {
                return LaunchType.DotApprefMsWithoutArgument;
            }
            else if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0].EndsWith(".application"))
            {
                return LaunchType.DotApplication;
            }
            else
            {
                return LaunchType.DotApprefMsWithArgument;
            }
        }

        internal static string GetArgument()
        {
            var lt = GetLaunchType();

            switch (lt)
            {
                case LaunchType.DotApplication:
                case LaunchType.DotApprefMsWithoutArgument:
                    return null;
                case LaunchType.DotApprefMsWithArgument:
                    return AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];
                default:
                    throw new InvalidEnumArgumentException(nameof(lt), (int)lt, typeof(LaunchType));
            }
        }

        static DeployManifest GetDeployManifest()
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                return null;
            }

            // Microsoft.Build.Tasks.Core
            using (var ms = new MemoryStream(AppDomain.CurrentDomain.ActivationContext.DeploymentManifestBytes))
            {
                return (DeployManifest)ManifestReader.ReadManifest("Deployment", ms, false);
            }
        }

        static string GetApprefMsPath()
        {
            var dp = GetDeployManifest();
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), dp.Publisher, dp.SuiteName, dp.Product + ".appref-ms");
        }

        internal static void StartSelf()
        {
            var lt = GetLaunchType();

            switch (lt)
            {
                case LaunchType.DotApplication:
                    Process.Start(ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri);
                    break;
                case LaunchType.DotApprefMsWithoutArgument:
                    Process.Start(GetApprefMsPath());
                    break;
                case LaunchType.DotApprefMsWithArgument:
                    Process.Start(GetApprefMsPath(), AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0]);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(lt), (int)lt, typeof(LaunchType));
            }
        }
    }
}
