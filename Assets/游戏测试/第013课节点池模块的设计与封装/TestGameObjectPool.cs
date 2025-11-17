using UnityEngine;

public class TestGameObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<GameObjectPoolMgr>().Init();
        this.gameObject.AddComponent<TimerMgr>().Init();

        GameObjectPoolMgr.Instance.CreateGameObjectPool("Assets/AssetsPacakge/Cube666.prefab", 10);  //创建10个Cube节点
        GameObject cube = GameObjectPoolMgr.Instance.GetGameObject("Assets/AssetsPacakge/Cube666.prefab");  //从节点池中获取一个Cube节点
        cube.transform.SetParent(this.transform, false);  //将节点设置在当前节点下
        cube.transform.position = new Vector3(10, 10, 10);
        GameObject cube1 = GameObjectPoolMgr.Instance.GetGameObject("Assets/AssetsPacakge/Cube666.prefab");  //从节点池中获取一个Cube节点
        cube1.transform.SetParent(GameObject.Find("Cube123").transform, false);  //将节点设置在当前节点下
        cube1.transform.position = new Vector3(1, 1, 1);

        TimerMgr.Instance.ScheduleOnce((object udata) =>  //5秒后进行回收
        {
            GameObjectPoolMgr.Instance.RecycleGameObject("Assets/AssetsPacakge/Cube666.prefab", cube);
        },5);

    }
}
