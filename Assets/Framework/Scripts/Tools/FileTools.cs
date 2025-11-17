using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

/// <summary>
/// added by wsh @ 2017.12.25
/// 功能：通用静态方法
/// </summary>

public class FileTools
{
    public const string AssetsFolderName = "Assets";

    /// <summary>
    /// 将文件路径path中的\\替换为/
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static string FormatToUnityPath(string path)
    {
        return path.Replace("\\", "/");
    }

    /// <summary>
    /// 将文件路径path中的/替换为\\
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    public static string FormatToSysFilePath(string path)
    {
        return path.Replace("/", "\\");
    }

    /// <summary>
    /// 将全路径full_path替换为以Asset开头的路径，比如将路径
    /// "E:\\01 UnityProject20241002\\100 君麟科技游戏框架\\01 单机PC版\\"JunLinFramework1.0\\Assets\\Game\\Resources"" 
    /// 转换为Assets/Game/Resources
    /// </summary>
    /// <param name="full_path"></param>
    /// <returns></returns>
    public static string FullPathToAssetPath(string full_path)
    {
        full_path = FormatToUnityPath(full_path);
        if (!full_path.StartsWith(Application.dataPath))
        {
            return null;
        }
        string ret_path = full_path.Replace(Application.dataPath, "");
        return AssetsFolderName + ret_path;
    }

    /// <summary>
    /// 获取文件的扩展名（后缀名），比如“Cube.prefab”得到“.prefab”
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileExtension(string path)
    {
        return Path.GetExtension(path).ToLower();
    }

    /// <summary>
    /// 在某个文件夹中找特殊的文件，返回的是路径数组
    /// </summary>
    /// <param name="path"></param>
    /// <param name="extensions"></param>
    /// <param name="exclude"></param>
    /// <returns></returns>
    public static string[] GetSpecifyFilesInFolder(string path, string[] extensions = null, bool exclude = false)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        if (extensions == null)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        }
        else if (exclude)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => !extensions.Contains(GetFileExtension(f))).ToArray();
        }
        else
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(GetFileExtension(f))).ToArray();
        }
    }

    /// <summary>
    /// 在某个文件夹中依据pattern(后缀)找特殊的文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static string[] GetSpecifyFilesInFolder(string path, string pattern)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
    }

    /// <summary>
    /// 把一个文件夹中所有文件的路径都返回
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] GetAllFilesInFolder(string path)
    {
        return GetSpecifyFilesInFolder(path);
    }

    /// <summary>
    /// 获取这个路径下所有文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] GetAllDirsInFolder(string path)
    {
        return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
    }

    /// <summary>
    /// 检查文件所在的文件夹是否存在，不存在则在filePath路径下创建文件夹
    /// </summary>
    /// <param name="filePath"></param>
    public static void CheckFileAndCreateDirWhenNeeded(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        FileInfo file_info = new FileInfo(filePath);
        DirectoryInfo dir_info = file_info.Directory;
        if (!dir_info.Exists)
        {
            Directory.CreateDirectory(dir_info.FullName);
        }
    }

    /// <summary>
    /// 检查文件夹是否存在，不存在则在folderPath路径下创建文件夹
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    public static void CheckDirAndCreateWhenNeeded(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            return;
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    /// <summary>
    /// 安全的将二进制数据写到一个二进制文件中
    /// </summary>
    /// <param name="outFile"></param>
    /// <param name="outBytes"></param>
    /// <returns></returns>
    public static bool SafeWriteAllBytes(string outFile, byte[] outBytes)
    {
        try
        {
            if (string.IsNullOrEmpty(outFile))
            {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile))
            {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }
            File.WriteAllBytes(outFile, outBytes);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeWriteAllBytes failed! path = {0} with err = {1}", outFile, ex.Message));
            return false;
        }
    }

    /// <summary>
    /// 安全的读一个二进制文件
    /// </summary>
    /// <param name="outFile"></param>
    /// <param name="outLines"></param>
    /// <returns></returns>
    public static bool SafeWriteAllLines(string outFile, string[] outLines)
    {
        try
        {
            if (string.IsNullOrEmpty(outFile))
            {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile))
            {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }
            File.WriteAllLines(outFile, outLines);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeWriteAllLines failed! path = {0} with err = {1}", outFile, ex.Message));
            return false;
        }
    }

    /// <summary>
    /// 将一段文本写入到文件中
    /// </summary>
    /// <param name="outFile"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool SafeWriteAllText(string outFile, string text)
    {
        try
        {
            if (string.IsNullOrEmpty(outFile))
            {
                return false;
            }

            CheckFileAndCreateDirWhenNeeded(outFile);
            if (File.Exists(outFile))
            {
                File.SetAttributes(outFile, FileAttributes.Normal);
            }
            File.WriteAllText(outFile, text);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeWriteAllText failed! path = {0} with err = {1}", outFile, ex.Message));
            return false;
        }
    }

    public static byte[] SafeReadAllBytes(string inFile)
    {
        try
        {
            if (string.IsNullOrEmpty(inFile))
            {
                return null;
            }

            if (!File.Exists(inFile))
            {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllBytes(inFile);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeReadAllBytes failed! path = {0} with err = {1}", inFile, ex.Message));
            return null;
        }
    }

    public static string[] SafeReadAllLines(string inFile)
    {
        try
        {
            if (string.IsNullOrEmpty(inFile))
            {
                return null;
            }

            if (!File.Exists(inFile))
            {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllLines(inFile);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeReadAllLines failed! path = {0} with err = {1}", inFile, ex.Message));
            return null;
        }
    }

    public static string SafeReadAllText(string inFile)
    {
        try
        {
            if (string.IsNullOrEmpty(inFile))
            {
                return null;
            }

            if (!File.Exists(inFile))
            {
                return null;
            }

            File.SetAttributes(inFile, FileAttributes.Normal);
            return File.ReadAllText(inFile);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeReadAllText failed! path = {0} with err = {1}", inFile, ex.Message));
            return null;
        }
    }

    public static void DeleteDirectory(string dirPath)
    {
        string[] files = Directory.GetFiles(dirPath);
        string[] dirs = Directory.GetDirectories(dirPath);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(dirPath, false);
    }

    public static bool SafeClearDir(string folderPath)
    {
        try
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return true;
            }

            if (Directory.Exists(folderPath))
            {
                DeleteDirectory(folderPath);
            }
            Directory.CreateDirectory(folderPath);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeClearDir failed! path = {0} with err = {1}", folderPath, ex.Message));
            return false;
        }
    }

    public static bool SafeDeleteDir(string folderPath)
    {
        try
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return true;
            }

            if (Directory.Exists(folderPath))
            {
                DeleteDirectory(folderPath);
            }
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeDeleteDir failed! path = {0} with err: {1}", folderPath, ex.Message));
            return false;
        }
    }

    public static bool SafeDeleteFile(string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return true;
            }

            if (!File.Exists(filePath))
            {
                return true;
            }
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeDeleteFile failed! path = {0} with err: {1}", filePath, ex.Message));
            return false;
        }
    }

    public static bool SafeRenameFile(string sourceFileName, string destFileName)
    {
        try
        {
            if (string.IsNullOrEmpty(sourceFileName))
            {
                return false;
            }

            if (!File.Exists(sourceFileName))
            {
                return true;
            }
            File.SetAttributes(sourceFileName, FileAttributes.Normal);
            File.Move(sourceFileName, destFileName);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeRenameFile failed! path = {0} with err: {1}", sourceFileName, ex.Message));
            return false;
        }
    }

    public static bool SafeCopyFile(string fromFile, string toFile)
    {
        try
        {
            if (string.IsNullOrEmpty(fromFile))
            {
                return false;
            }

            if (!File.Exists(fromFile))
            {
                return false;
            }
            CheckFileAndCreateDirWhenNeeded(toFile);
            if (File.Exists(toFile))
            {
                File.SetAttributes(toFile, FileAttributes.Normal);
            }
            File.Copy(fromFile, toFile, true);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("SafeCopyFile failed! formFile = {0}, toFile = {1}, with err = {2}",
                fromFile, toFile, ex.Message));
            return false;
        }
    }

    
}

