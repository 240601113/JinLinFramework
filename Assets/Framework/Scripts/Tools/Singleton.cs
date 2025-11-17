using UnityEngine;

/// <summary>
/// 普通的单例模式，用于无需挂载在Unity物体上的脚本
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : new()  //where 限制模板的类型, new()指的是这个类型必须要能被实例化 
{
    private static T _instance;  //定义单例对象的静态变量
    private static object mutex = new object();  //定义线程锁
    public static T Instance
    {
        get {
            if (_instance == null)  //为什么不把这句去掉直接加锁，因为单例百分之99都不会为null，这样不用每次都检测锁，提高效率
            {
                lock (mutex)   //保证我们的单例，是线程安全的;
                { 
                    if (_instance == null) 
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}

/// <summary>
/// Unity单例, 用于需要挂载在Unity物体上的脚本。不用考虑多线程
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySingleton<T> : MonoBehaviour where T : Component 
{
    private static T _instance = null;
    public static T Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null) 
                {
                    GameObject obj = new GameObject();
                    _instance = (T)obj.AddComponent(typeof(T));
                    obj.hideFlags = HideFlags.DontSave;
                    obj.name = typeof(T).Name;
                }
            }
            return _instance;
        }
    }

    public virtual void Awake() 
    {
        //不删除
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null) 
        {
            _instance = this as T;
        }
        else 
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}





