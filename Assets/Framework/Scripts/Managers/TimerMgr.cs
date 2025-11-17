using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMgr : UnitySingleton<TimerMgr>
{
    class TimerNode
    {
        public int timerID;  //定时器ID
        public TimerHandler OnTimer;  //定时器委托
        public object param;  //传输的数据
        public int repeat;  //重复次数
        public float nextTriggerTime;  //下一次触发时间
        public float curTime;  //当前时间
        public float interval;  //每隔几秒的触发时间
        public bool isCancel;
    }

    public delegate void TimerHandler(object param);  //定义委托

    private Dictionary<int, TimerNode> timerNodes;  //定时器节点集合
    private int autoTimerID = 1;  //ID自动增加
    private List<int> removeTimerQueue;  //定时器删除队列
    public void Init()
    {
        this.timerNodes = new Dictionary<int, TimerNode>();
        this.removeTimerQueue = new List<int>();
        this.autoTimerID = 1;
    }
 

    /// <summary>
    /// 完整版定时函数
    /// </summary>
    /// <param name="OnTimer">定时时间到要回调的方法</param>
    /// <param name="param">传递的参数</param>
    /// <param name="repeat">重复次数，-1或者0表示无限重复</param>
    /// <param name="interval">每个几秒触发一次</param>
    /// <param name="delay">延迟几秒触发第一次</param>
    /// <returns></returns>
    public int Schedule(TimerHandler OnTimer, object param,int repeat, float interval, float delay)
    {

        if (OnTimer == null || interval < 0.0f || delay < 0.0f)
        {
            return 0;
        }

        int timerID = this.autoTimerID++;
        this.autoTimerID = (this.autoTimerID == 0) ? 1 : this.autoTimerID;

        TimerNode timerNode = new TimerNode();
        timerNode.OnTimer = OnTimer;
        timerNode.param = param;
        timerNode.repeat = (repeat <= 0) ? -1 : repeat;
        timerNode.interval = interval;
        timerNode.curTime = 0;
        timerNode.nextTriggerTime = delay;
        timerNode.timerID = timerID;
        timerNode.isCancel = false;

        this.timerNodes.Add(timerID, timerNode);

        return timerID;  //返回ID
    }

    /// <summary>
    /// 不传参数版定时器
    /// </summary>
    /// <param name="OnTimer">定时时间到要调用的函数</param>
    /// <param name="repeat">重复次数，-1或者0表示无限重复</param>
    /// <param name="interval">每个几秒触发一次</param>
    /// <param name="delay">延迟几秒触发第一次</param>
    /// <returns></returns>
    public int Schedule(TimerHandler OnTimer, int repeat,float interval, float delay)
    {
        return Schedule(OnTimer, null, repeat, interval, delay);
    }

    /// <summary>
    /// 只调用一次且可传参数版定时器
    /// </summary>
    /// <param name="OnTimer">定时时间到要调用的函数</param>
    /// <param name="param">传递的参数</param>
    /// <param name="delay">延迟几秒触发第一次</param>
    /// <returns></returns>
    public int ScheduleOnce(TimerHandler OnTimer, object param, float delay)
    {
        return Schedule(OnTimer, param, 1, 0, delay);
    }

    /// <summary>
    /// 只调用一次且不可传参数版定时器
    /// </summary>
    /// <param name="OnTimer">定时时间到要调用的函数</param>
    /// <param name="delay">延迟几秒触发第一次</param>
    /// <returns></returns>
    public int ScheduleOnce(TimerHandler OnTimer, float delay)
    {
        return Schedule(OnTimer, null, 1, 0, delay);
    }

    /// <summary>
    /// 通过定时器ID取消定时器
    /// </summary>
    /// <param name="timerID"></param>
    public void UnShedule(int timerID)
    {
        if (this.timerNodes.ContainsKey(timerID))
        {
            TimerNode timerNode = this.timerNodes[timerID];
            if (timerNode != null)
            {
                timerNode.isCancel = true;
                this.removeTimerQueue.Add(timerID);
            }
        }
    }
    void Update()
    {
        if (this.timerNodes == null)  //如果没有定时器，则直接返回 
        {
            return;
        }

        foreach (var key in this.timerNodes.Keys) 
        {
            TimerNode timerNode = this.timerNodes[key];
            if (timerNode == null || timerNode.isCancel == true) 
            {
                continue;
            }

            timerNode.curTime += Time.deltaTime;  //累计当前时间
            if (timerNode.nextTriggerTime <= timerNode.curTime)  //如果当前时间大于第一次触发的时间
            {
                timerNode.OnTimer(timerNode.param);  //触发委托函数

                timerNode.nextTriggerTime = timerNode.interval;
                timerNode.curTime = 0;

                if (timerNode.repeat != -1) 
                {
                    timerNode.repeat--;
                }

                if (timerNode.repeat == 0)  //定时器执行结束了, 就要删除掉
                {  
                    this.UnShedule(timerNode.timerID);
                }
            }
        }

        //// 清理掉过期Timer
        //foreach (var key in this.timerNodes.Keys) 
        //{
        //    TimerNode timerNode = this.timerNodes[key];
        //    if (timerNode == null || timerNode.isCancel == false) 
        //    {
        //        continue;
        //    }
        //    // 直接在foreach或者for循环中删除字典的key，会导致字典结构变化，从而导致问题
        //    // this.timerNodes.Remove(key); 
        //    this.removeTimerQueue.Add(key);
        //}

        for (int i = 0; i < this.removeTimerQueue.Count; i++) 
        {
            this.timerNodes.Remove(this.removeTimerQueue[i]);
        }
        this.removeTimerQueue.Clear();
    }
}
