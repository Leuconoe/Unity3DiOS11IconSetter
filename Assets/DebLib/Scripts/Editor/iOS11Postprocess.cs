using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class iOS11Postprocess
{
    [PostProcessBuild(101)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
#if UNITY_IOS || true
        //read and check json
        string xcodeAppIconContentsFile = path + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset/Contents.json";
        string xcodeAppIconContentsText = null;
        try
        {
            xcodeAppIconContentsText = File.ReadAllText(xcodeAppIconContentsFile);
        }
        catch (Exception exception)
        {
            Console.WriteLine(string.Format("Failed to read app icon contents file at: {0} with error <{1}>", xcodeAppIconContentsFile, exception.Message));
        }

        // if exist idoiom, return
        if (xcodeAppIconContentsText.Contains("ios-marketing"))
        {
            Console.WriteLine("already add 1024x1024 icon");
            return;
        }
        else
        {
            // rewrite json
            string newString = xcodeAppIconContentsText.Replace("]", ", {\"size\" : \"1024x1024\", \"idiom\" : \"ios-marketing\",\"filename\" : \"Icon-marketing.png\", \"scale\" : \"1x\"}]");
            try
            {
                File.WriteAllText(xcodeAppIconContentsFile, newString);
            }
            catch (Exception exception)
            {
                Console.WriteLine(string.Format("Failed to write app icon contents file at: {0} with error <{1}>", xcodeAppIconContentsFile, exception.Message));
            }

            //overwrite icon
            System.IO.File.Copy(Application.dataPath + "/icons/Icon-marketing.png", path + "/Unity-iPhone/Images.xcassets/AppIcon.appiconset/Icon-marketing.png", true);
        }

#endif
    }
}