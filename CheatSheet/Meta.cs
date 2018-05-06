using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CheatSheet
{
    static class Meta
    {
        // Explanatory wrapper
        static string GetAssemblyName() => AppDomain.CurrentDomain.FriendlyName;

        // Explanatory wrapper
        static string GetExecutablePath() => Assembly.GetExecutingAssembly().Location;

        // The following is faster than Assembly.GetExecutingAssembly().Location but requires PresentaionFramework.dll.
        // static string GetExecutablePath() => Application.ResourceAssembly.Location;

        // Explanatory wrapper
        static string GetExecutableDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        internal static string GetApplicationName() => Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

        static string GetAppSetting(string name) => ConfigurationManager.AppSettings[name]?.Replace("{UserName}", Environment.UserName);

        static bool? GetAppSettingAsBool(string name)
        {
            var s = ConfigurationManager.AppSettings[name];
            return s != null && bool.TryParse(s, out var b) ? b : (bool?)null;
        }

        static int? GetAppSettingAsInt(string name)
        {
            var s = ConfigurationManager.AppSettings[name];
            return s != null && int.TryParse(s, out var n) ? n : (int?)null;
        }

        static T? GetAppSettingAsEnum<T>(string name) where T : struct
        {
            var s = ConfigurationManager.AppSettings[name];
            return s != null && Enum.TryParse<T>(s, true, out var e) ? (T?)e : null;
        }

        static SqlConnectionStringBuilder CreateSqlConnectionStringBuilder(string s) => new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[s].ConnectionString);

        // Explanatory wrapper
        static void SettingsSample()
        {
            // Settings.Default.Foo = "FooValue";
            // Worse than the above because the existence of the key is not validated at the build time.
            // Settings.Default["Foo"] = "FooValue";

            // Settings.Default.Save();
        }

        static string GetUserScopeSettingsPath()
        {
            // UserScopeSettings is stored as:
            // %LOCALAPPDATA%\<CompanyName (If CompanyName is empty, DefaultNamespace is used.)>\<AssemblyName>_...\<AssemblyVersion (not AssemblyFileVersion)>\user.config

            // AssemblyName: Project Properties > Application > Assembly name
            // DefaultNamespace: Project Properties > Application > Default namespace
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
        }

        // This returns available data in the following priority.
        // 1. AssemblyVersion
        // 2. 0.0.0.0
        static string GetAssemblyVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        // This returns available data in the following priority.
        // 1. AssemblyVersion
        // 2. 0.0.0.0
        static string GetAssemblyVersion(string path) => AssemblyName.GetAssemblyName(path).Version.ToString();

        // This returns available data in the following priority.
        // 1. AssemblyFileVersion
        // 2. AssemblyVersion
        // 3. 0.0.0.0
        static string GetAssemblyFileVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        // This returns available data in the following priority.
        // 1. AssemblyFileVersion
        // 2. AssemblyVersion
        // 3. 0.0.0.0
        static string GetAssemblyFileVersion(string path) => FileVersionInfo.GetVersionInfo(path).FileVersion;

        // This returns available data in the following priority.
        // 1. AssemblyInformationalVersion
        // 2. AssemblyFileVersion
        // 3. AssemblyVersion
        // 4. 0.0.0.0
        static string GetAssemblyInformationalVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        // This returns available data in the following priority.
        // 1. AssemblyInformationalVersion
        // 2. AssemblyFileVersion
        // 3. AssemblyVersion
        // 4. 0.0.0.0
        static string GetAssemblyInformationalVersion(string path) => FileVersionInfo.GetVersionInfo(path).ProductVersion;

        static string GetClickOncePublishVersion() => ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();

        static string GetGuid() => ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value;

        static string GetLogPath => ((Hierarchy)LogManager.GetRepository()).Root.Appenders.OfType<RollingFileAppender>().First().File;
    }
}
