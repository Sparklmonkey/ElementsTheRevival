using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

// Original code by PlayItSafe_Fries
// modified by bourriquet and colinsenner
/// https://forum.unity.com/threads/webgl-template.363057/

public class PostProcessWebGL
{
    // Fill in here the folder name from: `Assets/WebGLTemplates/<FOLDER>`
    // Hard-coding this variable is undesirable. Unity Cloud Build allows the user to define an environment variable,
    // which could be used here, but it's more to remember during setup and thus, the simpler hard-coded solution seems best.
    const string CUSTOM_TEMPLATE_NAME = "ElementsCustom";

    static string TemplatePath { get { return Paths.Combine(Application.dataPath, "WebGLTemplates", CUSTOM_TEMPLATE_NAME); } }

#if UNITY_CLOUD_BUILD && UNITY_WEBGL
        // Make sure to add this to your Unity Cloud Build -> Advanced Options -> Post-Export Method -> `PostProcessWebGL.PostExport`
        public static void PostExport(string exportPath)
        {
            Debug.Log("PostProcessWebGL.PostExport() called.");

            Debug.LogFormat($"  PlayerSettings.WebGL.template = '{PlayerSettings.WebGL.template}'");
            Debug.LogFormat($"  TemplatePath: '{TemplatePath}'");

            // Clear the TemplateData folder, built by Unity.
            FileUtilExtended.CreateOrCleanDirectory(Paths.Combine(exportPath, "TemplateData"));

            // Copy contents from WebGLTemplate. Ignore all .meta files
            FileUtilExtended.CopyDirectoryFiltered(TemplatePath, exportPath, true, @".*/\.+|\.meta$", true);

            // Replace contents of index.html
            FixIndexHtml(exportPath);
        }
#endif

    // Replaces %...% defines in index.html
    static void FixIndexHtml(string pathToBuiltProject)
    {
        string buildFolderName = "Build";
        string buildDir = Paths.Combine(pathToBuiltProject, buildFolderName);

        // Build Keywords map
        Dictionary<string, string> replaceKeywordsMap = new Dictionary<string, string>();

        replaceKeywordsMap.Add("{{{ LOADER_FILENAME }}}", FindFileNameWithExtension(buildDir, ".js"));
        replaceKeywordsMap.Add("{{{ DATA_FILENAME }}}", FindFileNameWithExtension(buildDir, ".data.gz"));
        replaceKeywordsMap.Add("{{{ CODE_FILENAME }}}", FindFileNameWithExtension(buildDir, ".wasm.gz"));
        replaceKeywordsMap.Add("{{{ FRAMEWORK_FILENAME }}}", FindFileNameWithExtension(buildDir, ".js.gz"));
        replaceKeywordsMap.Add("{{{ BACKGROUND_FILENAME }}}", FindFileNameWithExtension(buildDir, ".jpg"));

        //App info
        replaceKeywordsMap.Add("{{{ COMPANY_NAME }}}", Application.companyName);
        replaceKeywordsMap.Add("{{{ PRODUCT_NAME }}}", Application.productName);
        replaceKeywordsMap.Add("{{{ PRODUCT_VERSION }}}", Application.version);

        string indexFilePath = Paths.Combine(pathToBuiltProject, "index.html");
        Func<string, KeyValuePair<string, string>, string> replaceFunction = (current, replace) => string.IsNullOrEmpty(replace.Value) ? current : current.Replace(replace.Key, replace.Value);
        if (File.Exists(indexFilePath))
        {
            File.WriteAllText(indexFilePath, replaceKeywordsMap.Aggregate<KeyValuePair<string, string>, string>(File.ReadAllText(indexFilePath), replaceFunction));
        }

        string rootPath = Environment.CurrentDirectory;
        string buildPath = Directory.Exists(pathToBuiltProject) ? pathToBuiltProject : Path.GetDirectoryName(pathToBuiltProject); // pc => return exe path
        bool isWindows = Environment.OSVersion.ToString().Contains("Windows");

#if UNITY_CLOUD_BUILD
        Debug.Log($"#UCB [PostBuild] Root path: {rootPath}");
        Debug.Log($"#UCB [PostBuild] Build Path: {buildPath}");
        Debug.Log($"#UCB [PostBuild] Builder OS: {Environment.OSVersion} | isWindows: {isWindows}"); 
        try
        {
            var bashInfo = new System.Diagnostics.ProcessStartInfo {
                FileName  = isWindows ? @"C:\tools\cygwin\bin\sh.exe" : "/bin/bash",
                Arguments = $"{rootPath}/Assets/Editor/UCB/UCB_SendBuild.sh {buildPath} "
                            + $"{PlayerSettings.iOS.buildNumber} "
                            + (isWindows?"-TRfv":"-Rfv") + " ",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                // RedirectStandardError  = true,
                CreateNoWindow = true,
            };
            using(var bash = System.Diagnostics.Process.Start(bashInfo))
            {
                Debug.Log($"#UCB\n===OUTPUT===\n{bash.StandardOutput.ReadToEnd()}\n=======\n");
                if(!bash.WaitForExit(10 * 60 * 1000)) // wait up to 10 minutes
                {
                    bash.Kill();
                    Debug.LogError($"#UCB [PostBuild] send FAILED");
                }
            }
            Debug.Log($"#UCB [PostBuild] send done");
        }
        catch (System.Exception e)
        {
            Debug.LogError("#UCB [PostBuild] send error:\n"+e);
        }
        
#else
        Debug.Log($"#[PostBuild] Not Unity cloud build, now exit");
#endif
    }
    static string FindFileNameWithExtension(string buildDirectory, string extension)
    {
        var dir = new DirectoryInfo(buildDirectory);
        foreach (var f in dir.GetFiles())
        {
            if (f.Name.EndsWith(extension))
            {
                return f.Name;
            }
        }

        return null;
    }
    private class FileUtilExtended
    {
        internal static void CreateOrCleanDirectory(string dir)
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);

            Directory.CreateDirectory(dir);
        }

        // Fix forward slashes on other platforms than windows
        internal static string FixForwardSlashes(string unityPath)
        {
            return ((Application.platform != RuntimePlatform.WindowsEditor) ? unityPath : unityPath.Replace("/", @"\"));
        }

        // Copies the contents of one directory to another.
        public static void CopyDirectoryFiltered(string source, string target, bool overwrite, string regExExcludeFilter, bool recursive)
        {
            RegexMatcher excluder = new RegexMatcher()
            {
                exclude = null
            };
            try
            {
                if (regExExcludeFilter != null)
                {
                    excluder.exclude = new Regex(regExExcludeFilter);
                }
            }
            catch (ArgumentException)
            {
                UnityEngine.Debug.Log("CopyDirectoryRecursive: Pattern '" + regExExcludeFilter + "' is not a correct Regular Expression. Not excluding any files.");
                return;
            }
            CopyDirectoryFiltered(source, target, overwrite, excluder.CheckInclude, recursive);
        }

        internal static void CopyDirectoryFiltered(string sourceDir, string targetDir, bool overwrite, Func<string, bool> filtercallback, bool recursive)
        {
            // Create directory if needed
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
                overwrite = false;
            }

            // Iterate all files, files that match filter are copied.
            foreach (string filepath in Directory.GetFiles(sourceDir))
            {
                string localPath = filepath.Substring(sourceDir.Length);
                if (filtercallback(localPath))
                {
                    string fileName = Path.GetFileName(filepath);
                    string to = Path.Combine(targetDir, fileName);

                    File.Copy(FixForwardSlashes(filepath), FixForwardSlashes(to), overwrite);
                }
            }

            // Go into sub directories
            if (recursive)
            {
                foreach (string subdirectorypath in Directory.GetDirectories(sourceDir))
                {
                    string localPath = subdirectorypath.Substring(sourceDir.Length);
                    if (filtercallback(localPath))
                    {
                        string directoryName = Path.GetFileName(subdirectorypath);
                        CopyDirectoryFiltered(Path.Combine(sourceDir, directoryName), Path.Combine(targetDir, directoryName), overwrite, filtercallback, recursive);
                    }
                }
            }
        }

        internal struct RegexMatcher
        {
            public Regex exclude;
            public bool CheckInclude(string s)
            {
                return exclude == null || !exclude.IsMatch(s);
            }
        }
    }

    private class Paths
    {
        // Combine multiple paths using Path.Combine
        public static string Combine(params string[] components)
        {
            if (components.Length < 1)
                throw new ArgumentException("At least one component must be provided!");

            string str = components[0];
            for (int i = 1; i < components.Length; i++)
                str = Path.Combine(str, components[i]);

            return str;
        }
    }
}