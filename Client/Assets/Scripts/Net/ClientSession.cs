/****************************************************
    文件：ClientSession.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/2/29 22:0:19
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;

public class ClientSession : PENet.PESession<GameMsg>
{
    protected override void OnConnected()
    {
        GameRoot.AddTips("连接服务器成功");
        PECommon.Log("Connect to Server Succ");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRoot.AddTips("服务器断开连接");
        PECommon.Log("DisConnect to Server");
    }
}