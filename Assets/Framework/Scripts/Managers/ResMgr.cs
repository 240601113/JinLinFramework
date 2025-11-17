using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Timeline.Actions;


public class ResMgr : UnitySingleton<ResMgr>
{
    public void Init() 
    {

    }

    /// <summary>
    /// 同步资源加载
    /// </summary>
    /// <typeparam name="T">加载的类型</typeparam>
    /// <param name="assetPath">为方便开发assetPath变量路径以Assets开头，路径要含后缀名，如"Assets/AssetsPacakge/Cube.prefab"</param>
    /// <returns></returns>
    public T LoadAssetSync<T>(string assetPath) where T : UnityEngine.Object
    {
        //以Assets/Game/Resources/Cube.prefab为例提取Cube，因为Resources.Load 会自动检索 Assets
        //下所有 Resources 文件夹，因此路径不需要包含 Resources 本身，也不需要.prefab 后缀，因此要
        //对Assets/Game/Resources/Cube.prefab进行提取，提取出Cube。

        int resourcesIndex = assetPath.IndexOf("Resources/");  //查找“Resources/”的位置
        if (resourcesIndex == -1)
        {
            LogMgr.Instance.Log("路径中不包含Resources文件夹: " + assetPath);
            return null;
        }
        
        string pathAfterResources = assetPath.Substring(resourcesIndex + "Resources/".Length);  //截取“Resources/”之后的部分（例如：Cube.prefab）
        //LogMgr.Instance.Log("Resources/之后的路径为: " + pathAfterResources);
        string extensionName = Path.GetExtension(pathAfterResources);  //获取扩展名，比如.prefab

        string resourceName = pathAfterResources.Replace(extensionName, "");  //将pathAfterResources（也就是Cube.prefab中的.prefab替换为"",也就是空）
        //LogMgr.Instance.Log("路径中资源名为: " + resourceName);
        return Resources.Load<T>(resourceName);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetPath">目前是相对于Resrouces文件夹的路径，不需要具体文件的后缀名</param></param>
    /// <param name="action">接收资源的回调函数</param>
    public void LoadAssetAsync<T>(string assetPath, UnityAction<T> resAction) where T : UnityEngine.Object
    {
        //异步加载 不能马上得到加载的资源 至少要等一帧。
        //如果要等一帧，是否会在运行的时候发生问题，如果因为这个有问题
        StartCoroutine(LoadAssetIEnumerat(assetPath, resAction));  //在非发布版本开启日志打印管理初始化
    }

    private IEnumerator LoadAssetIEnumerat<T>(string assetPath, UnityAction<T> resAction) where T : Object//约束一下加载对象必须是Object类
    {
        int resourcesIndex = assetPath.IndexOf("Resources/");  //查找Resources/的位置
        if (resourcesIndex == -1)
        {
            LogMgr.Instance.Log("路径中不包含Resources文件夹: " + assetPath);
            yield return null;
        }

        string pathAfterResources = assetPath.Substring(resourcesIndex + "Resources/".Length);  //截取Resources/之后的部分（例如：Cube.prefab）
        //LogMgr.Instance.Log("Resources/之后的路径为: " + pathAfterResources);
        string extensionName = Path.GetExtension(pathAfterResources);  //获取扩展名，比如.prefab

        string resourceName = pathAfterResources.Replace(extensionName, "");  //将pathAfterResources（也就是Cube.prefab中的.prefab替换为"",也就是空）
        ResourceRequest res = Resources.LoadAsync<T>(resourceName);
        while (!res.isDone)
        {
            float progress = res.progress * 100f;  //进度范围：0（未开始）~ 1（完成），转换为百分比
            LogMgr.Instance.Log($"资源加载进度：{progress:F1}%");  // F1表示保留1位小数
            yield return null;  // 等待一帧必须有否则会阻塞主线程
        }
        yield return res;  //等待资源加载好才会执行下面语句
        LogMgr.Instance.Log("资源加载完毕");
        resAction(res.asset as T);  //把加载好的资源直接转换连同回调一起传出去
        res = null;
    }
}
