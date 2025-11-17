using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : UnitySingleton<SceneMgr>
{
    public void Init() 
    {
        
    }

    /// <summary>
    /// 同步加载场景（会阻塞主线程）
    /// </summary>
    /// <param name="sceneName">场景名称（需在Build Settings中注册）</param>
    /// <param name="loadMode">加载模式（Single：卸载当前场景；Additive：附加场景）</param>
    /// <returns>是否加载成功</returns>
    public bool LoadSceneSync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        if (!IsSceneValid(sceneName))  // 检查场景合法性
        {

        #if RELEASE_BUILD

        #else
            Debug.Log("同步加载失败：场景 {sceneName} 不存在或未注册！");
        #endif
            
            return false;
        }

        try
        {
           
            SceneManager.LoadScene(sceneName, loadMode);  //同步加载场景    
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);  //验证加载结果
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);  //设置为激活场景
            #if RELEASE_BUILD

            #else
                Debug.Log("同步加载场景成功，加载的场景为：{sceneName}");
            #endif
                
                return true;
            }
            else
            {
            #if RELEASE_BUILD

            #else
                Debug.Log("同步加载失败：场景 {sceneName} 加载后无效！");
            #endif
                
                return false;
            }
        }
        catch (Exception e)
        {
        #if RELEASE_BUILD

        #else
            Debug.Log($"同步加载异常：{e.Message}");
        #endif
            
            return false;
        }
    }

    /// <summary>
    /// 异步加载场景（非阻塞）
    /// </summary>
    /// <param name="sceneName">场景名称</param>
    /// <param name="loadMode">加载模式</param>
    /// <param name="progressCallback">进度回调（0~1）</param>
    /// <param name="completeCallback">完成回调（是否成功，场景信息）</param>
    public void LoadSceneAsync(string sceneName,
                               Action<bool, Scene> completeCallback = null,
                               Action<float> progressCallback = null,
                               LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        
        if (!IsSceneValid(sceneName))  // 检查场景合法性
        {
        #if RELEASE_BUILD

        #else
            Debug.Log($"异步加载失败：场景 {sceneName} 不存在或未注册！");
        #endif

            completeCallback?.Invoke(false, new Scene());
            return;
        }

        StartCoroutine(AsyncLoadCoroutine(sceneName, loadMode, progressCallback, completeCallback));  //启动协程执行异步加载
    }

    /// <summary>
    /// 异步加载协程（核心逻辑）
    /// </summary>
    private IEnumerator AsyncLoadCoroutine(string sceneName,
                                          LoadSceneMode loadMode,
                                          Action<float> progressCallback,
                                          Action<bool, Scene> completeCallback)
    {
        // 开始异步加载
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, loadMode);
        asyncOp.allowSceneActivation = true;  //允许加载完成后自动激活

        while (!asyncOp.isDone)  // 轮询加载进度
        {
            // 进度值处理（当allowSceneActivation=false时，进度最高0.9，需要转换为0~1）
            float progress = asyncOp.allowSceneActivation ? asyncOp.progress : asyncOp.progress / 0.9f;
            progressCallback?.Invoke(Mathf.Clamp01(progress)); // 确保进度在0~1范围内
            yield return null; // 等待下一帧
        }

        // 验证加载结果
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
        #if RELEASE_BUILD

        #else
            Debug.Log($"异步加载场景成功，场景名为：{sceneName}");
        #endif

            completeCallback?.Invoke(true, loadedScene);
        }
        else
        {
        #if RELEASE_BUILD

        #else
            Debug.LogError("异步加载场景失败，场景名为：{sceneName}");
        #endif

            completeCallback?.Invoke(false, new Scene());
        }
    }

    /// <summary>
    /// 检查场景是否有效（已添加到Build Settings）
    /// </summary>
    private bool IsSceneValid(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return false;
        }

        // 检查场景是否在Build Settings中
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
