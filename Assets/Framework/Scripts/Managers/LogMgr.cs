using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.VersionControl;
using UnityEngine;


//#if DEBUG  
#if RELEASE_BUILD  //如果是发布模式而不是Debug模式，则执行下面Log空函数，不打印任何东西
public class LogMgr : UnitySingleton<LogMgr>
{
    public void Init()
    {
      
    }
    public void Log(object message) 
    {

    }    
}
#else  //如果不是发布模式，则执行下面函数

/// <summary>
/// 安卓端还是没测试，故算1.0版本
/// </summary>
public class LogMgr : UnitySingleton<LogMgr>
{
    StreamWriter streamWriter;  //文本文件的写入操作类
    string path;  //运行文件exe路径
    string filePath;  //日志文件路径

    private List<string> infoList = new List<string>();  //定义消息列表，用于展示在UI界面上
    private bool DebugInfo = false;  //用户按钮翻转

    //@显示帧率
    private float timeDelta = 0.5f;  //规定每0.5秒统计一次帧率
    private float prevComputeTime = 0;  //上一次的时间戳
    private int iFrameCount = 0;  //帧数
    private float fps = 0;
    //end

    public void Init()
    {
        path = Path.GetDirectoryName(Application.dataPath) ?? Application.dataPath;  //获取exe文件所在目录的路径。双问号操作符，如果左边不为null，则返回左操作数，反之则返回右操作数  
        //Debug.Log("日志文件所在路径为：" + path);
        filePath = Path.Combine(path, "JunLinFrameworkLog.txt");  //合成日志文件完整路径

        Application.logMessageReceived += HandleLog;  //添加事件触发方法      
        this.prevComputeTime = Time.realtimeSinceStartup;  //赋值当前时间，为计算帧率做准备
        this.iFrameCount = 0;  //赋值当前帧计数，为计算帧率做准备
    }

    private void Update()
    {
        this.iFrameCount++;  //帧数加1
        if (Time.realtimeSinceStartup >= this.prevComputeTime + this.timeDelta)  //如果当前时间大于上次统计的时间+this.timeDelta的总时间
        {
            this.fps = this.iFrameCount / (Time.realtimeSinceStartup - this.prevComputeTime);  //计算帧率
            this.prevComputeTime = Time.realtimeSinceStartup;  //把当前时间赋值给上一次统计的时间
            this.iFrameCount = 0;  //帧数归零
        }
    }

    private void OnApplicationQuit()  
    {
        Application.logMessageReceived -= HandleLog;  //删除事件触发方法
        
        if (streamWriter != null)
        {
            streamWriter.Dispose();  //释放StreamWriter对象占用的资源，确保所有的缓冲区都被正确地刷新
            streamWriter = null;
        }
    }

    /// <summary>
    /// Unity的日志打印回调函数，当Unity系统有内容（比如报错等）需要输出控制台，则会触发该回调
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type">日志类型</param>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)  //如果捕获到了错误或异常时，记录日志
        {
            LogToFile(logString, LogLevel.Error);
            LogToFile(stackTrace, LogLevel.Error);
        }

        if (type == LogType.Warning)  //如果捕获到了警告时，记录日志
        {
            LogToFile(logString, LogLevel.Warning);
            LogToFile(stackTrace, LogLevel.Warning);
        }

        if (type == LogType.Log)
        {
            LogToFile(logString, LogLevel.log);
            LogToFile(stackTrace, LogLevel.log);
        }
    }

    /// <summary>
    /// 将日志信息写入文件中
    /// </summary>
    /// <param name="log">日志信息</param>
    /// <param name="level">日志信息的等级，默认为日志信息</param>
    private void LogToFile(string log, LogLevel level=LogLevel.log)
    {
        streamWriter = new StreamWriter(filePath, true, Encoding.UTF8);  //生成StreamWriter对象,将文件格式设置为UTF8并打开文件
        var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");  //获取当前时间，并按照yyyy-MM-dd HH:mm:ss表示
        var logstr = "[" + level.ToString() + "]" + "[" + currentTime + "]" + log;
        streamWriter.WriteLine(logstr);
        streamWriter.Flush();  //强制将缓冲区中的数据立即写入到目标流（如文件、网络流等），并清空缓冲区
        streamWriter.Close();  //为了在随时都能打开log.txt文件，所以写完一句就关掉文件一次
    }

    /// <summary>
    /// 将自己要输出的信息写入文件中。处于非发布模式时，同时用Debug.log输出显示在控制台上
    /// </summary>
    /// <param name="log">自己要输出的信息</param>
    public void Log(string message)
    {
        //如果是安卓、苹果、WebGL（微信小游戏）
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WebGLPlayer)
        {
            infoList.Add(message);
        }
        else
        {
            infoList.Add(message);
            UnityEngine.Debug.Log(message);
        }
    }


    //@信息窗口
    public Rect infoWindowRect = new Rect(80, 20, 800, 1200);
    private void OnGUI()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.WebGLPlayer)

        {
            GUILayout.Space(40);
            if (GUILayout.Button("日志信息"))
            {
                DebugInfo = !DebugInfo;
            }
            else if (GUILayout.Button("清空信息"))
            {
                infoList.Clear();
            }

            //调试错误信息
            if (DebugInfo)
            {
                infoWindowRect = GUILayout.Window(1, infoWindowRect, infoWindow, "日志信息窗口");
            }
            // 显示我们当前的FPS
            GUI.Label(new Rect(0, Screen.height - 20, 200, 200), "帧率FPS:" + this.fps.ToString("f2"));
        }
        else
        {
            GUILayout.Space(40);
            if (GUILayout.Button("日志信息"))
            {
                DebugInfo = !DebugInfo;
            }
            else if (GUILayout.Button("清空信息"))
            {
                infoList.Clear();
            }

            //调试错误信息
            if (DebugInfo)
            {
                infoWindowRect = GUILayout.Window(1, infoWindowRect, infoWindow, "日志信息窗口");
            }
            // 显示我们当前的FPS
            GUI.Label(new Rect(0, Screen.height - 20, 200, 200), "帧率FPS:" + this.fps.ToString("f2"));
        }
    }

    //错误信息显示窗口
    private Vector2 infoPosition = new Vector2(0, 0);
    void infoWindow(int windowID)
    {
        infoPosition = GUILayout.BeginScrollView(infoPosition, false, true, GUILayout.Width(800), GUILayout.Height(600));

        GUILayout.Space(30);
        GUILayout.BeginVertical();  //垂直布局

        foreach (string str in infoList)
        {
            GUILayout.Label(str, GUILayout.Width(800));
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }
    //@end
}

/// < summary >
/// 日志等级
/// </ summary >
public enum LogLevel
{
    Error = 0,        //错误日志
    Warning = 1,      //警告日志
    log = 2,          //信息日志，对应Debug.log
}
#endif