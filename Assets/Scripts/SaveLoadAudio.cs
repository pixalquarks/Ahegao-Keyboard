using System.IO;
using SFB;
using UnityEngine;

public static  class SaveLoadAudio
{
    public static bool BuildingNewSchema = false;
    public static void OpenFileExplorer()
    {
#if UNITY_EDITOR
        var path = UnityEditor.EditorUtility.OpenFilePanel("Sound File", "", "mp3, wav");
#else
        var extension = new[]
        {
            new ExtensionFilter("Sound File", "mp3", "wav", "ogg")
        };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extension, false);
#endif
        if (path != null) Debug.Log(path);
    }
    
    public static string SaveAudio(string source, string fileName, string destination)
    {
        var sourceFile = Path.Combine(source, fileName);
        var destFile = Path.Combine(destination, fileName);

        Directory.CreateDirectory(destination);
        try
        {
            File.Copy(sourceFile, destFile);
            return destFile;
        }
        catch
        {
            return "error";
        }
        
    }

}
