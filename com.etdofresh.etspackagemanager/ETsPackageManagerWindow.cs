using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.ETsPackageManager
{
    public class ETsPackageManagerWindow : EditorWindow
    {
        private const string url = "https://gist.githubusercontent.com/ETdoFresh/022ecf1a98a84635eb3e113c0c13eaec/raw";
        private static PackagesJson _packageJsonList;
        private static List<PackageRow> _packageRows = new List<PackageRow>();
        private static ListRequest _listRequest;
        private double _lastUpdatedTime;

        [MenuItem("Window/ETs Package Manager")]
        public static void ShowWindow()
        {
            var window = GetWindow<ETsPackageManagerWindow>("ETs Package Manager");
            if (_packageJsonList == null)
                window.DownloadPackageList();
        }

        private void OnGUI()
        {
            _listRequest ??= UnityEditor.PackageManager.Client.List(true, true);
            if (_listRequest.IsCompleted && _listRequest.Result == null)
                _listRequest = UnityEditor.PackageManager.Client.List(true, true);
            
            if (GUILayout.Button("Update Registry"))
            {
                _listRequest = null;
                DownloadPackageList();
                _lastUpdatedTime = EditorApplication.timeSinceStartup;
            }

            if (_packageRows == null || _packageRows.Count == 0)
                if (_lastUpdatedTime + 5 < EditorApplication.timeSinceStartup)
                    DownloadPackageList();

            GUILayout.Space(10);

            var packageRows = GetPackageRows(_packageJsonList?.packages);
            foreach (var package in packageRows)
            {
                var windowWidth = position.width;
                var versionWidth = 80;
                var buttonWidth = 80;
                var titleWidth = windowWidth - versionWidth - buttonWidth - 20;
                package.selectedIndex = package.selectedIndex < package.versions.Length ? package.selectedIndex : 0;
                var selectedVersion = package.versions[package.selectedIndex];
                var title = GetTitle(package.name, selectedVersion);
                var description = GetDescription(package.name, selectedVersion);
                GUILayout.BeginHorizontal();
                GUILayout.Label(title, GUILayout.Width(titleWidth));
                package.selectedIndex = EditorGUILayout.Popup(package.selectedIndex, package.versions,
                    GUILayout.Width(versionWidth));
                if (AppNotInstalled(package) && GUILayout.Button("Install", GUILayout.Width(buttonWidth)))
                {
                    InstallPackage(package, selectedVersion);
                }
                else if (AppInstalled(package) && GUILayout.Button("Uninstall", GUILayout.Width(buttonWidth)))
                {
                    UninstallPackage(package);
                }

                GUILayout.EndHorizontal();
            }
        }

        private string GetTitle(string packageName, string version)
        {
            var package =
                _packageJsonList.packages.FirstOrDefault(p => p.packageName == packageName && p.version == version);
            return package?.title;
        }

        private string GetDescription(string packageName, string version)
        {
            var package =
                _packageJsonList.packages.FirstOrDefault(p => p.packageName == packageName && p.version == version);
            return package?.description;
        }

        private void InstallPackage(PackageRow package, string version)
        {
            var packagesDirectory = Directory.GetCurrentDirectory() + "/Packages";
            var packageDirectory = packagesDirectory + "/" + package.name;
            if (Directory.Exists(packageDirectory))
            {
                throw new Exception("Packages directory already exists");
            }

            var gitUrl = GetGitUrl(package.name, version);
            var addRequest = UnityEditor.PackageManager.Client.Add(gitUrl);
            while (!addRequest.IsCompleted) { }
            _listRequest = null;
        }

        private bool AppInstalled(PackageRow package)
        {
            if (_listRequest == null) return false;
            while (!_listRequest.IsCompleted) { }
            return _listRequest.Result.Any(p => p.name == package.name);
        }

        private bool AppNotInstalled(PackageRow package)
        {
            return !AppInstalled(package);
        }

        private void UninstallPackage(PackageRow package)
        {
            var removeRequest = UnityEditor.PackageManager.Client.Remove(package.name);
            while (!removeRequest.IsCompleted) { }
            _listRequest = null;
        }

        private string GetGitUrl(string packageName, string version)
        {
            var package =
                _packageJsonList.packages.FirstOrDefault(p => p.packageName == packageName && p.version == version);
            return package?.packageUrl;
        }

        private void RemovePackage(PackageRow package)
        {
            var packagesDirectory = Directory.GetCurrentDirectory() + "/Packages";
            var packageDirectory = packagesDirectory + "/" + package.name;
            if (!Directory.Exists(packagesDirectory))
            {
                throw new Exception("Packages directory does not exist");
            }

            Directory.Delete(packageDirectory, true);
        }

        private void ChangePackageVersion(PackageRow package, string version)
        {
            RemovePackage(package);
            InstallPackage(package, version);
        }

        private PackageRow[] GetPackageRows(PackageData[] packages)
        {
            if (packages == null) return Array.Empty<PackageRow>();
            foreach (var package in packages.OrderBy(x => x.packageName))
            {
                var myPackages = packages.Where(x => x.packageName == package.packageName).ToArray();
                if (_packageRows.Any(x => x.name == package.packageName))
                {
                    var packageRow = _packageRows.First(x => x.name == package.packageName);
                    packageRow.installedVersion = "NA";
                    packageRow.versions = myPackages.Select(x => x.version).OrderBy(x => x).ToArray();
                    packageRow.latestVersion = myPackages.Max(x => x.version);
                }
                else
                {
                    var packageRow = new PackageRow
                    {
                        name = package.packageName,
                        installedVersion = "NA",
                        latestVersion = myPackages.Max(x => x.version),
                        versions = myPackages.Select(x => x.version).OrderBy(x => x).ToArray(),
                    };
                    packageRow.selectedIndex = packageRow.versions.Length - 1;
                    _packageRows.Add(packageRow);
                }
            }

            return _packageRows.ToArray();
        }

        private void DownloadPackageList()
        {
            var saltyUrl = $"{url}?ticks={DateTime.Now.Ticks}";
            var urlContents = UnityWebRequest.Get(url);
            urlContents.SendWebRequest();
            while (!urlContents.isDone) { }

            if (urlContents.result == UnityWebRequest.Result.ConnectionError ||
                urlContents.result == UnityWebRequest.Result.ProtocolError ||
                urlContents.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.LogError(urlContents.error);
            }

            _packageJsonList = JsonUtility.FromJson<PackagesJson>(urlContents.downloadHandler.text);
            Debug.Log(
                $"Downloaded Registry of {_packageJsonList.packages.Length} package{(_packageJsonList.packages.Length == 1 ? "" : "s")}");
            urlContents.Dispose();
        }
    }

    [Serializable]
    public class PackagesJson
    {
        public PackageData[] packages = Array.Empty<PackageData>();
    }

    [Serializable]
    public class PackageData
    {
        public string packageName;
        public string version;
        public string title;
        public string description;
        public string packageUrl;
    }

    [Serializable]
    public class PackageRow
    {
        public string name;
        public string installedVersion;
        public string latestVersion;
        public string[] versions;
        public int selectedIndex;
    }
}