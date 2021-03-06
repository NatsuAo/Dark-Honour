﻿/****************************************************
	文件：Class1
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/2 15:17:49   	
	功能：缓存层
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class CacheSvc
{
	private static CacheSvc instance = null;
	public static CacheSvc Instance {
		get {
			if (instance == null)
			{
				instance = new CacheSvc();
			}
			return instance;
		}
	}
	private DBMgr dBMgr;

	private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
	private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();
	public void Init()
	{
		dBMgr = DBMgr.Instance;
		PECommon.Log("CacheSvc Init Done.");
	}

	public bool IsAcctOnLine(string acct)
	{
		return onLineAcctDic.ContainsKey(acct);
	}

	/// <summary>
	/// 根据账号密码返回对应账号数据，密码错误啧返回null，账号不存在则默认创建新账号
	/// </summary>
	public PlayerData GetPlayerData(string acct, string pass)
	{
		// 从数据库中查找账号数据
		return dBMgr.QueryPlayerData(acct, pass);
	}

	/// <summary>
	/// 账号上线，缓存数据
	/// </summary>
	public void AcctOnLine(string acct, ServerSession session, PlayerData playerData)
	{
		onLineAcctDic.Add(acct, session);
		onLineSessionDic.Add(session, playerData);
	}
}