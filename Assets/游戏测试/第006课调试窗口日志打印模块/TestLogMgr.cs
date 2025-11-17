using UnityEngine;

public class TestLogMgr : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.AddComponent<LogMgr>().Init();
        LogMgr.Instance.Log("我是平常信息！！！");
        LogMgr.Instance.Log("我是警告信息！！！");
        LogMgr.Instance.Log("我是错误信息！！！");
    }
}
