using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using System;

/// <summary>
/// 存档读档
/// </summary>
/// 

//有些人只想用unity自带的Json解析 写个枚举供选择

public enum JsonType
{
    UnityJson,
    LitJson,
}

public class JsonMgr : UnitySingleton<JsonMgr>
{


    public void Init()
    {

    }

    //提供两个外部接口来共外部使用实现写入和读取  序列化和反序列化都在这执行


    //既然是保存数据 那传进来的就是各种类 干脆写成object
    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">数据类</param>
    /// <param name="fileName">希望的文件夹名称</param>
    /// <param name="type">使用unityjson还是Litjson</param>
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //得到储存路径
        string path = Application.persistentDataPath + "/" + fileName+".json";
        //序列化 得到Json字符串
        string Jsonstr = null;
        switch (type)
        {                                                                                                    
            case JsonType.UnityJson:
                Jsonstr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                Jsonstr = JsonMapper.ToJson(data);
                break;

        }

        //把1序列化好的json字符串存到指定路径文件夹中
        if (Jsonstr != null)
        {
            File.WriteAllText(path, Jsonstr);
            Debug.LogWarning("文件储存成功!路径在" + path);
        }
        else
        {
            Debug.LogError("json字符串为空");
        }
        
    }


    /// <summary>
    /// 读取指定文件 的json数据 反序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="fileName">文件名</param>
    /// <param name="type">使用unityjson还是Litjson</param>
    /// <returns></returns>
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson)where T : new()
    {

        //确定从哪个路径获取 


        //首先判断 默认数据文件夹中是否有想要的数据  如果有就 从中获取

        string path = Application.streamingAssetsPath + "/Json" +"/"+ fileName+".json";//这个路径是只读的默认路径 适合存放一些游戏启动时就需要读取的数据  游戏开机的时候可能就需要读取文件 所以先从这个路径读取一下 没有再切换下个路径读取

        if (!File.Exists(path))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";//如果在默认文件夹没找到 那就切换到这个可读可写的路径查找
        }

      
        if (!File.Exists(path))//如果读写文件夹还没找到
        {
            return new T();//返回一个默认对象出去
        }

        //进行反序列化

        string jsonstr = File.ReadAllText(path);//将文件里储存的文本读取出来
        T data = default(T);//核心目的是给泛型变量一个符合其类型的默认初始值，保证代码的安全性和兼容性（无论 T 是值类型还是引用类型，都能正确初始化）。
        //声明好泛型变量来接收数据 数据对象默认值
        switch (type)
        {
            case JsonType.UnityJson:
                data = JsonUtility.FromJson<T>(jsonstr);
                break;
            case JsonType.LitJson:
              data =  JsonMapper.ToObject<T>(jsonstr);
                
                break;
        }

        //把对象返回出去

        return data;
    }


}
