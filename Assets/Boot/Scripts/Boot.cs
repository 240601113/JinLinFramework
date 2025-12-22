using System.Collections;
using UnityEngine;

public class Boot : UnitySingleton<Boot>
{
    public override void Awake()
    {
        base.Awake();
        //Application.targetFrameRate = 60;  //设定游戏帧率固定在60
        Application.runInBackground = true;  //当设置为 true 时：即使应用程序处于后台（用户切换到其他窗口、最小化窗口等），Unity仍然会继续执行 Update()、FixedUpdate() 等游戏循环函数，以及渲染画面（部分平台可能限制渲染）。
        this.StartCoroutine(nameof(this.BootStartup));
    }

    IEnumerator BootStartup()
    {
        // 检查热更新
        yield return this.CheckHotUpdate();

        // 游戏框架初始化
        yield return this.InitFramework();

        // 进入游戏
        GameApp.Instance.EnterGame();

        yield break;  //yield break在协程中表示直接跳出当前协程，结束执行。
    }

    /// <summary>
    /// 检查热更新
    /// </summary>
    /// <returns>无</returns>
    private IEnumerator CheckHotUpdate()
    {
        yield break;
    }
   
    /// <summary>
    /// 初始化游戏框架
    /// </summary>
    /// <returns>无</returns>
    private IEnumerator InitFramework()
    {
        this.gameObject.AddComponent<SceneMgr>().Init();  //场景管理器初始化
        this.gameObject.AddComponent<LogMgr>().Init();  //日志管理器初始化
        this.gameObject.AddComponent<ResMgr>().Init();  //资源管理器初始化
        this.gameObject.AddComponent<EventMgr>().Init();  //事件初始化
        this.gameObject.AddComponent<TimerMgr>().Init();  //定时器管理器初始化
        //this.gameObject.AddComponent<SoundMgr>().Init();  //声音管理器初始化
        ////this.gameObject.AddComponent<GameObjectPoolMgr>().Init();  //游戏对象池管理器初始化
        this.gameObject.AddComponent<UIMgr>().Init();  //UI管理器初始化
        this.gameObject.AddComponent<GameApp>().Init();  //游戏管理器（入口）初始化（这个一般都在框架管理器之后初始化）
        this.gameObject.AddComponent<JsonMgr>().Init(); //数据管理器 全局游戏数据的读取和存储
        //this.gameObject.AddComponent<FightMgr>().Init();

       

        yield break;
    }

}