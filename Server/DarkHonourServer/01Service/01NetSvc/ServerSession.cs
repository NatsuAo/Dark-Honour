/****************************************************
	文件：ServerSession
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/2/29 21:25:35   	
	功能：网络会话连接
*****************************************************/
using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg>
{
    public int sessionID = ServerRoot.Instance.GetSessionID();
    protected override void OnConnected()
    {
        PECommon.Log($"SessionID:{sessionID} Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log($"SessionID:{sessionID} RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(new MsgPack(this, msg));
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log($"SessionID:{sessionID} Client DisConnect");
    }
}
