using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr : UnitySingleton<EventMgr>
{
    public delegate void OnEventAction(string eventName, object udata);  //委托
    private Dictionary<string, OnEventAction> eventActions = null;  //创建事件与事件名映射字典
    public void Init() 
    {
        this.eventActions = new Dictionary<string, OnEventAction>();  //初始化字典
    }

    /// <summary>
    /// 事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="onEvent">事件回调方法</param>
    public void AddEventListener(string eventName, OnEventAction onEvent) 
    {
        if (this.eventActions.ContainsKey(eventName))  //如果事件字典包含 eventName
        {
            this.eventActions[eventName] += onEvent;  //添加事件
        }
        else 
        {
            this.eventActions[eventName] = onEvent;
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="onEvent">事件回调方法</param>
    public void RemoveEventListener(string eventName, OnEventAction onEvent) 
    {
        if (this.eventActions.ContainsKey(eventName)) 
        {
            this.eventActions[eventName] -= onEvent;
        }

        //if (this.eventActions[eventName] == null) 
        //{
        //    this.eventActions.Remove(eventName);  //删不删都无所谓，占不了多少内存
        //}
    }

    /// <summary>
    /// 触发事件监听
    /// </summary>
    /// <param name="eventName">事件的名称</param>
    /// <param name="udata">触发事件要传递的数据</param>
    public void EmitEvent(string eventName, object udata) 
    {
        if (this.eventActions.ContainsKey(eventName)) 
        {
            if (this.eventActions[eventName] != null)  //有可能删掉事件，所以要判断一下
            {
                this.eventActions[eventName](eventName, udata);
            }
        }
    }
}
