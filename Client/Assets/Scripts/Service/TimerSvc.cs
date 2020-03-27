/****************************************************
	文件：TimerSvc
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/8 21:20:53   	
	功能：计时服务
*****************************************************/
using System;
using System.Threading;

public class TimerSvc : SystemRoot
{
    public static TimerSvc Instance = null;

    private PETimer pt;
    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();

        // 设置日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });

        PECommon.Log("Init TimeSvc...");
    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public double GetNowTime()
    {
        return pt.GetMillisecondsTime();
    }

    public void DelTask(int tid)
    {
        pt.DeleteTimeTask(tid);
    }
}