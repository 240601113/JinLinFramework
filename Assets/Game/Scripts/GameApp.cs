using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameApp : UnitySingleton<GameApp>
{
    public void Init()
    {

    }

    public void EnterGame()
    {
    #if RELEASE_BUILD

    #else
        Debug.Log("已正常进入游戏");
    #endif

        #region//测试同步加载场景
        //bool loadSceneSuccess = SceneMgr.Instance.LoadSceneSync("Main", LoadSceneMode.Single);
        //if (loadSceneSuccess)
        //{
        //#if RELEASE_BUILD

        //#else
        //    Debug.Log("测试同步加载：加载成功，可执行后续逻辑");
        //#endif

        //    this.TestGame();
        //}
        ////end
        #endregion

        #region//测试异步加载场景
        ////@测试1-直接回调
        //SceneMgr.Instance.LoadSceneAsync("Main",
        //                                (success, scene) =>  //场景加载回调
        //                                {
        //                                   if (success)
        //                                   {
        //                                    #if RELEASE_BUILD

        //                                    #else
        //                                        Debug.Log($"测试异步加载成功：当前激活场景为{scene.name}");                                      
        //                                    #endif 
        //                                       // 可以在这里添加场景加载后的初始化逻辑
        //                                       this.TestGame();
        //                                   }
        //                                   else
        //                                   {
        //                                       Debug.Log("测试异步加载失败");
        //                                   }
        //                                },

        //                                progress =>  //进度回调
        //                                {
        //                                    Debug.Log($"测试异步加载进度：{progress:P0}");
        //                                }, 
        //                                LoadSceneMode.Single
        //);
        //end

        //@测试2-简单方式
        SceneMgr.Instance.LoadSceneAsync("Main", TestRunningContent, AsynLoadProgress, LoadSceneMode.Single);
        //end
        #endregion
    }

    /// <summary>
    /// 异步场景加载后测试框架代码运行
    /// </summary>
    /// <param name="success">场景是否加载成功</param>
    /// <param name="scene">场景名</param>
    public void TestRunningContent(bool success, Scene scene)
    {
        #region//测试LogMgr日志管理
        //LogMgr.Instance.Log("日志管理器测试！！！");
        #endregion

        #region//测试ResMgr资源管理
        ////@测试同步资源加载
        //GameObject prefab = ResMgr.Instance.LoadAssetSync<GameObject>("Assets/Game/Resources/Cube.prefab");
        //if (prefab == null)
        //{
        //    LogMgr.Instance.Log("预制体加载失败！检查路径是否正确");
        //    return;
        //}
        //else
        //{
        //    LogMgr.Instance.Log("预制体加载成功！！！");
        //}

        //GameObject instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);  //实例化到场景中
        ////end

        //@测试异步加载资源
        //ResMgr.Instance.LoadAssetAsync<GameObject>("Assets/Game/Resources/Cube.prefab", AsynLoadRes);
        //end
        #endregion

        #region//测试FileTools工具类

        ////@测试FullPathToAssetPath
        //LogMgr.Instance.Log(FileTools.FullPathToAssetPath(
        //    "E:\\01 UnityProject20241002\\100 君麟科技游戏框架\\01 单机PC版\\" +
        //    "JunLinFramework1.0\\Assets\\Game\\Resources"));
        ////end

        ////测试GetFileExtension
        //LogMgr.Instance.Log(FileTools.GetFileExtension("Cube.prefab"));
        //// end

        ////@测试GetAllFilesInFolder方法
        //string[] Folders = FileTools.GetAllFilesInFolder("Assets");  
        //for (int i = 0; i < Folders.Length; i++)
        //{
        //    LogMgr.Instance.Log("测试GetAllFilesInFolder方法：");
        //    LogMgr.Instance.Log(Folders[i]);
        //}
        ////end

        ////@测试：检查文件所在的文件夹是否存在，不存在则在filePath路径下创建文件夹
        //FileTools.CheckFileAndCreateDirWhenNeeded("Assets/游戏测试/FileTools工具类测试/CheckFileAndCreateDirWhenNeeded/test.prefab");
        ////end

        ////@测试：查找Assets文件夹下是否存在CheckDirAndCreateWhenNeeded文件夹 若没有直接创建
        //FileTools.CheckDirAndCreateWhenNeeded("Assets/游戏测试/FileTools工具类测试/CheckDirAndCreateWhenNeeded");
        ////end

        ////@测试：将二进制数据写入自定义文件
        //byte[] customData = new byte[] { 0x01, 0x02, 0x03, 0x04 };  //自己写一个二进制数据
        //string customFilePath = "Assets/游戏测试/FileTools工具类测试/Configs/custom.config";  //保存到自定义格式文件
        //FileTools.SafeWriteAllBytes(customFilePath, customData);  //安全写入二进制文件 如没查找到传入路径 则创建
        ////end

        ////@测试：读取二进制文件
        //byte[] byt = FileTools.SafeReadAllBytes("Assets/游戏测试/FileTools工具类测试/Configs/custom.config");
        //Debug.Log("二进制文件数据为："+ byt[0] + byt[1]);
        ////end

        ////@测试：写入文本到文件并保存一个文本文件
        //string jsonContent = @" //
        //{
        //    ""playerName"": ""Hero"",
        //    ""level"": 10,
        //    ""skills"": [""Fireball"", ""Shield""]
        //}
        //";
        //string jsonPath = "Assets/游戏测试/FileTools工具类测试/SafeWriteAllText/player.json";  //保存为JSON文件
        //FileTools.SafeWriteAllText(jsonPath, jsonContent);
        ////end

        ////@测试：读取json文件
        //string[] str = FileTools.SafeReadAllLines("Assets/游戏测试/FileTools工具类测试/SafeWriteAllText/player.json");

        ////遍历数组 获取所有字符串拼接起来
        //for (int i = 0; i < str.Length; i++)
        //{
        //    Debug.Log("文件数据为："+str[i]);
        //}
        ////end

        ////@测试：读取文本文件
        //print(FileTools.SafeReadAllText("Assets/游戏测试/FileTools工具类测试/PetConfigData.tex"));  
        ////end

        ////@测试：删除目录文件夹包括文件夹内的文件
        //FileTools.DeleteDirectory("Assets/游戏测试/FileTools工具类测试/Configs");   //这个好像有问题，Configs删除不了
        ////end

        ////@测试：删除目录文件夹的文件 保留文件夹
        //FileTools.SafeClearDir("Assets/游戏测试/FileTools工具类测试/SafeWriteAllText");
        ////end

        ////@测试：删除目录下所有文件夹所有文件 包括子文件夹
        //FileTools.SafeDeleteDir("Assets/测试文件夹");
        ////end

        ////@测试：删除目录下的单个指定文件
        //FileTools.SafeDeleteFile("Assets/测试文件夹/fffd.cs");
        ////end

        ////@测试：重命名文件
        //FileTools.SafeRenameFile("Assets/测试文件夹/NewBehaviourScript.cs", "Assets/测试文件夹/Script.cs");
        ////end

        ////@测试：拷贝文件到新路径
        //FileTools.SafeCopyFile("Assets/Configs/player.json", "Assets/测试文件夹/player.json");
        ////end
        #endregion

        #region//测试DataTools工具类
        //byte[] sourceBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };//测试拷贝字节数组
        ////  准备目标数组）
        //byte[] targetBytes = new byte[10]; // 长度设为10，预留足够空间


        //DataTools.CopyBytes(  //调用 CopyBytes：从源数组索引1开始，拷贝3个字节，写入目标数组索引2的位置
        //    copyTo: targetBytes,    // 目标数组
        //    offsetTo: 2,            // 目标起始索引
        //    copyFrom: sourceBytes,  // 源数组
        //    offsetFrom: 1,          // 源起始索引
        //    count: 3                // 拷贝3个字节
        //);


        //Debug.Log("字节数组（UTF-8）：" + BitConverter.ToString(sourceBytes).Replace("-", " "));

        //string message = "王大锤世界";


        //byte[] byteData = DataTools.StringToBytes(message);//测试字符串转字节

        //// 在 Unity 控制台打印结果（以十六进制展示）
        //Debug.Log("转换后的字节数组：" + BitConverter.ToString(byteData));



        //DataTools.StringToUTFBytes(message);//测试字符串转字节数组 UTF8格式

        //Debug.Log(DataTools.BytesToString(byteData));//测试字节数组转字符串


        //// 模拟一个 HTTP GET 请求的查询字符串
        //string queryString = "name=张三&age=25&isVip=true&score=90.5";

        //// 调用方法解析
        //Hashtable paramsTable = DataTools.HttpGetInfo(queryString);//测试解析HPPT请求解析参数

        //// 从 Hashtable 中获取参数值
        //if (paramsTable != null)
        //{
        //    string name = paramsTable["name"] as string; // 张三
        //    string age = paramsTable["age"] as string;   // 25
        //    Debug.Log($"姓名：{name}，年龄：{age}");
        //}


        //// 示例1：生成 1-10 之间的随机整数（包含1不包含10 可能1-9）
        //int num1 = DataTools.RandInt(1, 10);
        //Debug.Log("1-10的随机数：" + num1);//测试生成随机数



        //string playerTempId = DataTools.RandString(8);
        //Debug.Log("玩家临时ID：" + playerTempId); // 测试生成随机字符串 可以当做玩家的临时id 游客账号 也可以当做随机取名字


        //string original = "王大锤";
        //string md5Hash = DataTools.Md5(original);//测试哈希加密
        //Debug.Log($"原始字符串：{original}\nMD5哈希值：{md5Hash}");
        #endregion

        #region//测试UnityTools工具类
        //// 假设玩家位置为球心，攻击范围半径为5（则半径平方为25）
        //Vector3 playerPos = transform.position;
        //float attackRange = 5f;

        //float attackRangeSqr = attackRange * attackRange; // 半径平方 = 25
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.position = playerPos;
        //// 要判断的敌人位置
        //Vector3 enemyPos = sphere.transform.position;

        //// 检查敌人是否在攻击范围内
        //bool isInRange = UnityTools.SphereContant(playerPos, attackRangeSqr, enemyPos);//测试敌人是否在自己球形半径范围内
        //if (isInRange)
        //{
        //    Debug.Log("敌人在攻击范围内！");
        //}
        //else
        //{
        //    Debug.Log("敌人不在攻击范围内！");
        //}

        //// 假设场景中有一个带碰撞体的物体（如玩家）
        //// 要检测的点（如敌人位置）
        //Vector3 enemyPo = new Vector3(2, 0, 3);
        //playerCollider = GameObject.Find("Capsule").GetComponent<Collider>();
        //// 判断敌人是否在玩家的包围盒范围内
        //bool isInBounds = UnityTools.BoundContant(playerCollider, enemyPos);//测试检测玩家是否被人撞到了
        //if (isInBounds)
        //{
        //    Debug.Log("敌人在玩家的包围盒内！");
        //}
        //else
        //{
        //    Debug.Log("敌人在玩家的包围盒外！");
        //}


        //UnityTools.DestroyAllChildren(GameObject.Find("Cube").GetComponent<Transform>());//测试删除目标物体下所有子节点

        //string vecStr = "1.5, 3.0, -2.5";
        //// 转换，默认值设为 Vector3.zero
        //Vector3 result = UnityTools.StringToVector3(vecStr, Vector3.zero);//测试字符串转V3坐标
        //Debug.Log($"转换结果：{result}");
        #endregion

        #region//测试EventMgr事件管理
        ////@测试EventMgr脚本
        //EventMgr.Instance.AddEventListener("test", this.OnTestCall);
        //EventMgr.Instance.EmitEvent("test", "7777");
        ////end
        #endregion

        #region//测试TimerMgr定时器管理
        //TimerMgr.Instance.ScheduleOnce((object udata) =>
        //{
        //    Debug.Log("测试定时器，5秒后触发");
        //}, 5f);

        //int timeID = TimerMgr.Instance.Schedule((object udata) =>
        //{
        //    Debug.Log("测试定时器，3秒后第一次触发定时器，触发后每个2秒触发一次，无限触发");
        //}, -1, 2, 3f);

        //TimerMgr.Instance.ScheduleOnce((object udata) =>
        //{
        //    Debug.Log("测试定时器，10秒后关闭timeID的定时器");
        //    TimerMgr.Instance.UnShedule(timeID);
        //}, 10f);
        #endregion

        ////@ 测试SoundMgr脚本
        //SoundMgr.Instance.PlayMusic("Assets/Game/Resources/Sounds/bestReward.mp3");
        //// end

        #region//测试UI管理

        GameObject uiObject = new GameObject("UI");  //创建空物体并命名为"UI" 
        //@生成Canvas物体，并命名为MainCanvas
        GameObject mainCanvasObj = new GameObject("MainCanvas");  //创建空物体并命名为"MainCanvas"
        Canvas canvas = mainCanvasObj.AddComponent<Canvas>();  //添加Canvas组件
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // 设置 Canvas 渲染模式（默认是 Screen Space - Overlay）
        CanvasScaler mainCanvasScaler = mainCanvasObj.AddComponent<CanvasScaler>();  //添加CanvasScaler组件（用于适配不同分辨率） 
        mainCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;  //设置UI Scale Mode为Scale With Screen Size
        mainCanvasScaler.referenceResolution = new Vector2(1920, 1080);  //设置Reference Resolution为1920x1080
        mainCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;  //设置Screen Match Mode为Match Width Or Height  
        mainCanvasScaler.matchWidthOrHeight = 1f;  //设置Match值为0.987     
        mainCanvasScaler.referencePixelsPerUnit = 100;  //设置Reference Pixels Per Unit为100

        mainCanvasObj.AddComponent<GraphicRaycaster>();  //添加 GraphicRaycaster 组件（用于UI射线检测，处理交互）


        mainCanvasObj.transform.parent = uiObject.transform;

        GameObject windowsCanvasObj = new GameObject("WindowsCanvas");  //创建空物体并命名为"windowsCanvas"
        windowsCanvasObj.transform.parent = uiObject.transform;


        if (FindObjectOfType<EventSystem>() == null)  //自动创建 EventSystem（如果场景中没有）
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>(); // 处理输入
        }

        UIMgr.Instance.ShowUIView("Assets/Game/Resources/UI/Prefabs/UIHome.prefab");  //显示UIHome界面
        ////UIMgr.Instance.ShowUIViewWithCanvas 这个还没有测试。
        //// end

        #endregion

        #region//测试3D游戏角色的血条架构与设计
        ////@生成UI物体
        //GameObject uiObject = new GameObject("UI");  //创建空物体并命名为"UI" 
        ////end

        ////@生成Canvas物体，并命名为MainCanvas
        //GameObject mainCanvasObj = new GameObject("MainCanvas");  //创建空物体并命名为"MainCanvas"
        //Canvas canvas = mainCanvasObj.AddComponent<Canvas>();  //添加Canvas组件
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;  // 设置 Canvas 渲染模式（默认是 Screen Space - Overlay）
        //CanvasScaler mainCanvasScaler = mainCanvasObj.AddComponent<CanvasScaler>();  //添加CanvasScaler组件（用于适配不同分辨率） 
        //mainCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;  //设置UI Scale Mode为Scale With Screen Size
        //mainCanvasScaler.referenceResolution = new Vector2(1920, 1080);  //设置Reference Resolution为1920x1080
        //mainCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;  //设置Screen Match Mode为Match Width Or Height  
        //mainCanvasScaler.matchWidthOrHeight = 1f;  //设置Match值为0.987     
        //mainCanvasScaler.referencePixelsPerUnit = 100;  //设置Reference Pixels Per Unit为100
        //mainCanvasObj.AddComponent<GraphicRaycaster>();  //添加 GraphicRaycaster 组件（用于UI射线检测，处理交互）

        //mainCanvasObj.transform.parent = uiObject.transform;  //UI物体作为MainCanvas物体的父物体
        //// end

        ////@添加灯光  
        //GameObject lightObj = new GameObject("Directional Light");  //创建一个空物体作为灯光载体
        //Light directionalLight = lightObj.AddComponent<Light>();  //添加方向光组件
        //directionalLight.type = LightType.Directional; // 设置灯光类型

        //// 设置灯光属性
        //directionalLight.color = Color.white; // 灯光颜色
        //directionalLight.intensity = 1.0f;    // 强度
        //directionalLight.shadows = LightShadows.Soft; // 阴影类型

        //lightObj.transform.rotation = Quaternion.Euler(45, 0, 0);  // 调整灯光方向（指向下方45度）
        ////end

        ////@加载地图预制体
        //GameObject mapPrefab = ResMgr.Instance.LoadAssetSync<GameObject>("Assets/Game/Resources/Maps/Prefabs/Map001.prefab");
        //if (mapPrefab == null)
        //{
        //    LogMgr.Instance.Log("预制体加载失败！检查路径是否正确");
        //    return;
        //}
        //else
        //{
        //    LogMgr.Instance.Log("预制体加载成功！！！");
        //}

        //GameObject mapObj = Instantiate(mapPrefab, new Vector3(0, 0, 0), Quaternion.identity);  //实例化到场景中

        //mapObj.name = mapPrefab.name;  //去掉(Clone)后缀,使用原始预制体的名称
        ////end

        ////@加载人物预制体

        //GameObject peoplePrefab = ResMgr.Instance.LoadAssetSync<GameObject>("Assets/Game/Resources/Models/Prefabs/Player.prefab");
        //if (peoplePrefab == null)
        //{
        //    LogMgr.Instance.Log("预制体加载失败！检查路径是否正确");
        //    return;
        //}
        //else
        //{
        //    LogMgr.Instance.Log("预制体加载成功！！！");
        //}

        //GameObject peopleObj = Instantiate(peoplePrefab, new Vector3(0, 0, 0), Quaternion.identity);  //实例化到场景中
        //peopleObj.name = peoplePrefab.name;
        ////end

        ////@显示游戏界面UI
        //UIMgr.Instance.ShowUIView("Assets/Game/Resources/UI/3DBlood/Prefabs/GameUI.prefab");
        ////end
        #endregion

        #region//测试TimerMgr定时器管理
        //@ 测试Debugger脚本

        // end

        //@ 测试Debugger脚本

        // end
        #endregion

        #region//测试TimerMgr定时器管理
        //@ 测试Debugger脚本

        // end

        //@ 测试Debugger脚本

        // end
        #endregion

    }
    /// <summary>
    /// 异步场景加载进度
    /// </summary>
    /// <param name="progress">加载进度数值</param>
    public void AsynLoadProgress(float progress)
    {
        // 进度回调
        Debug.Log($"异步场景加载进度：{progress:P0}");  //progress是浮点型，必须用$和:P0，否则显示不出数字
    }

    /// <summary>
    /// 测试异步资源加载
    /// </summary>
    /// <param name="res"></param>
    public void AsynLoadRes(GameObject res)
    {
        Debug.Log("异步加载资源测试成功！！！");
        GameObject.Instantiate(res);
        
    }


    private void OnTestCall(string eventName, object udata)
    {
        LogMgr.Instance.Log("测试事件订阅与发布管理模块：" + udata.ToString());
    }
}

