using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolMgr : UnitySingleton<GameObjectPoolMgr>
{
    /// <summary>
    /// 游戏对象池根路径
    /// </summary>
    private Transform gameObjectPoolRoot = null;  //定义游戏对象池根物体，作为整个游戏总得游戏对象池挂载点
    private Dictionary<string, Transform> gameObjectPoolMap = null;  //定义游戏对象路径到具体游戏对象池的映射
    private Transform tempGameObjectPoolRoot = null;  //定义临时游戏对象池根物体

    public void Init() 
    {
        #region  1、初始化游戏对象池字典
        this.gameObjectPoolMap = new Dictionary<string, Transform>();
        #endregion

        #region  2、先查找是否存在GameObjectPoolRoot物体，如果不存在则在Game物体下创建一个GameObjectPoolRoot物体
        //
        //this.gameObjectPoolRoot = this.transform.Find("GameObjectPoolRoot");  //这里使用this.transform.Find似乎没有用GameObject.Find("GameObjectPoolRoot")好，因为"GameObjectPoolRoot"物体不一定要成为当前脚本物体的子物体
        if (GameObject.Find("GameObjectPoolRoot") == null)  //如果这里为空
        {
            this.gameObjectPoolRoot = new GameObject().transform;  //创建节点
            this.gameObjectPoolRoot.SetParent(GameObject.Find("Game").transform, false);  //将GameObjectPoolRoot根物体放在Game物体下
            this.gameObjectPoolRoot.localPosition = Vector3.zero;  //本地位置设置为0
            this.gameObjectPoolRoot.gameObject.name = "GameObjectPoolRoot";  //节点名称设置为GameObjectPoolRoot
        }
        else
        {
            this.gameObjectPoolRoot = GameObject.Find("GameObjectPoolRoot").transform;
        }

        this.gameObjectPoolRoot.gameObject.SetActive(false);  //设置gameObjectPoolRoot物体为不激活状态，总游戏对象池设置为不激活状态，则游戏对象池具体对象则可以不用设置
        #endregion

        #region  3、先查找是否存在tempGameObjectPoolRoot物体，如果不存在则在this.gameObjectPoolRoot物体下创建一个tempGameObjectPoolRoot物体
        if (GameObject.Find("tempGameObjectPoolRoot") == null) 
        {
            this.tempGameObjectPoolRoot = new GameObject().transform;
            this.tempGameObjectPoolRoot.SetParent(this.gameObjectPoolRoot, false);  //将临时tempItemRoot节点放在gameObjectPoolRoot下
            this.tempGameObjectPoolRoot.localPosition = Vector3.zero;
            this.tempGameObjectPoolRoot.gameObject.name = "tempGameObjectPoolRoot";
        }
        else
        {
            this.tempGameObjectPoolRoot = GameObject.Find("tempGameObjectPoolRoot").transform;
        }
        this.tempGameObjectPoolRoot.gameObject.SetActive(false);  //这里应该设置为true吧？
        #endregion
    }

    /// <summary>
    /// 以资源预制体路径或名称为key创建游戏对象池，以资源预制体名称+Root为value创建具体游戏对象池物体
    /// 注意：物体路径不要出现两个及以上的“.”，路径必须以/连接。
    /// </summary>
    /// <param name="assetPrefabPath">资源预制体路径或名称</param>
    /// <param name="count">创建资源预制体对应的物体对象个数，默认数量为10</param>
    public void CreateGameObjectPool(string assetPrefabPath, int count = 10) 
    {
        #region  1、创建前必要检查:如果assetPrefabPath已存在对应的具体游戏对象池或者count数值为负数，则退出方法
        if (this.gameObjectPoolMap.ContainsKey(assetPrefabPath))  //如果已存在同名的游戏对象池映射
        {
            return;
        }

        if (count <= 0)
        {
            return;
        }
        #endregion

        #region  2、创建具体对象池根物体作为具体对象池，并以对象名称+Root作为具体对象池名称，并设置为未激活状态
        Transform gameObjectTypeRoot = new GameObject().transform;  //创建游戏对象类型根物体，作为该类型具体游戏对象池
        gameObjectTypeRoot.SetParent(this.gameObjectPoolRoot, false);  //将类型根节点设置为NodePoolRoot子节点
        int stringStartIndex = assetPrefabPath.LastIndexOf("/");  //获取路径最后一个/的位置
        int stringEndIndex = assetPrefabPath.LastIndexOf(".");    //获取路径最后一个.的位置
        string assetName = assetPrefabPath.Substring(stringStartIndex + 1, stringEndIndex - stringStartIndex-1);
        gameObjectTypeRoot.name = assetName + "Root";  //将预制体资源路径设为类型节点名字+Root
        gameObjectTypeRoot.gameObject.SetActive(false);  //设置具体的游戏对象根物体为未激活状态 
        #endregion

        //#region  3、将资源预制体路径和具体游戏对象池绑定，并生成count数量的游戏对象
        //this.gameObjectPoolMap.Add(assetPrefabPath, gameObjectTypeRoot);  //将资源预制体路径和具体游戏对象池绑定
        //GameObject gameObjectPrefab = ResMgr.Instance.LoadAssetSync<GameObject>(assetPrefabPath);   //这里用同步加载和异步加载资源都是一样的
        //for (int i = 0; i < count; i++)
        //{
        //    GameObject tempGameObject = GameObject.Instantiate(gameObjectPrefab);
        //    tempGameObject.transform.SetParent(gameObjectTypeRoot, false);
        //    tempGameObject.transform.localPosition = Vector3.zero;
        //}
        //#endregion
    }

    /// <summary>
    /// 清理游戏对象池
    /// </summary>
    /// <param name="assetPrefabPath">资源预制体路径</param>
    /// <param name="reserveCount">清空时留下多少个，默认一个不留</param>
    public void ClearGameObjectPool(string assetPrefabPath, int reserveCount = 0) 
    {
        #region  1、清理前必要检查
        if (!this.gameObjectPoolMap.ContainsKey(assetPrefabPath))   
        {
            return;
        }

        Transform gameObjectTypeRoot = this.gameObjectPoolMap[assetPrefabPath];
        if (gameObjectTypeRoot == null) 
        {
            return;
        }


        reserveCount = (reserveCount < 0) ? 0 : reserveCount;
        if (gameObjectTypeRoot.childCount <= reserveCount)  //如果类型根节点子物体数小于要留下的数目
        {
            return;
        }
        #endregion

        #region  2、删除游戏对象具体类型根物体的子物体
        int count = gameObjectTypeRoot.childCount - reserveCount;  //计算最终要清理的个数
        for (int i = 0; i < count; i++) 
        {
            GameObject.DestroyImmediate(gameObjectTypeRoot.GetChild(0));
        }
        #endregion
    }

    /// <summary>
    /// 获取游戏中的物体，该物体都是实例化好的
    /// </summary>
    /// <param name="assetPrefabPath">资源预制体路径</param>
    /// <returns></returns>
    public GameObject GetGameObject(string assetPrefabPath) 
    {
        #region  1、获取前必要检查
        if (!this.gameObjectPoolMap.ContainsKey(assetPrefabPath)) 
        {
            return null;
        }

        Transform gameObjectTypeRoot = this.gameObjectPoolMap[assetPrefabPath];
        if (gameObjectTypeRoot == null) 
        {
            return null;
        }
        #endregion

        #region  2、如果游戏对象类型根物体下没有子物体，则创建子物体并返回
        GameObject tempGameObject = null;
        //if (gameObjectTypeRoot.childCount <= 0)  // 如果当前类型根物体没有子物体，则创建一个子物体后放入游戏对象池中并返回
        //{ 
        //    GameObject gameObjectPrefab = ResMgr.Instance.LoadAssetSync<GameObject>(assetPrefabPath);  //重新加载一个节点
        //    tempGameObject = GameObject.Instantiate(gameObjectPrefab);

        //    tempGameObject.transform.SetParent(this.tempGameObjectPoolRoot, false);  //将该节点设置在临时根物体上
        //    tempGameObject.transform.localPosition = Vector3.zero;
        //    return tempGameObject;
        //}
        #endregion

        #region  3、获取游戏对象类型根物体的第一个子物体
        tempGameObject = gameObjectTypeRoot.GetChild(0).gameObject;  
        tempGameObject.transform.SetParent(this.tempGameObjectPoolRoot, false);  //将该节点设置在临时根物体上，此时gameObjectTypeRoot就会少一个节点
        tempGameObject.transform.localPosition = Vector3.zero;
        #endregion

        return tempGameObject;
    }

    /// <summary>
    /// 回收游戏对象到对应的游戏对象池中
    /// </summary>
    /// <param name="assetPrefabPath">资源预制体路径</param>
    /// <param name="obj"></param>
    public void RecycleGameObject(string assetPrefabPath, GameObject obj) 
    {
        #region  1、回收前检查
        if (!this.gameObjectPoolMap.ContainsKey(assetPrefabPath)) 
        {
            return;
        }
        #endregion

        #region  2、回收物体，回收的方式是通过将游戏物体重新放回对应的gameObjectTypeRoot物体下
        Transform gameObjectTypeRoot = this.gameObjectPoolMap[assetPrefabPath];
        obj.transform.SetParent(gameObjectTypeRoot, false);
        obj.transform.localPosition = Vector3.zero;
        #endregion
    }
}
