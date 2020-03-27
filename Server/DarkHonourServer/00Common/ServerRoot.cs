/****************************************************
	文件：ServerRoot
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/2/29 20:38:51   	
	功能：服务器初始化
*****************************************************/
using System;
using System.Collections.Generic;
using System.Text;

public class ServerRoot
{
	private static ServerRoot instance = null;
	public static ServerRoot Instance {
		get {
			if (instance == null)
			{
				instance = new ServerRoot();
			}
			return instance;
		}
	}

	public void Init()
	{
		// 数据层
		DBMgr.Instance.Init();

		// 服务层
		CacheSvc.Instance.Init();
		NetSvc.Instance.Init();
		CfgSvc.Instance.Init();
		TimerSvc.Instance.Init();
		// 业务系统层
		LoginSys.Instance.Init();
		GuideSys.Instance.Init();
		StrongSys.Instance.Init();
		ChatSys.Instance.Init();
		BuySys.Instance.Init();
		PowerSys.Instance.Init();
		TaskSys.Instance.Init();
		FubenSys.Instance.Init();
	}

	public void Update()
	{
		NetSvc.Instance.Update();
		TimerSvc.Instance.Update();
	}

	private int SessionID = 0;
	public int GetSessionID()
	{
		if (SessionID == int.MaxValue)
		{
			SessionID = 0;
		}
		return SessionID += 1;
	}
}
