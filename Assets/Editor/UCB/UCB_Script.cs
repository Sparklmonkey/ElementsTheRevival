using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

public class UCB_Script
{

#if UNITY_CLOUD_BUILD
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        string buildNumber = manifest.GetValue("buildNumber", "0");
        Debug.Log("#UCB [PreExport] Setting build number to " + buildNumber);
        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);
        PlayerSettings.iOS.buildNumber = buildNumber;
    }
#endif


    [PostProcessBuildAttribute(1)]
    public static void OnPostBuild(BuildTarget target, string pathToBuiltProject)
    {
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
}