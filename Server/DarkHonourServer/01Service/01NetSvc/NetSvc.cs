/****************************************************
	文件：NetSvc
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/2/29 20:57:40   	
	功能：
*****************************************************/

using PENet;
using PEProtocol;
using System.Collections.Generic;

public class MsgPack
{
	public ServerSession session;
	public GameMsg msg;

	public MsgPack(ServerSession session, GameMsg msg)
	{
		this.session = session;
		this.msg = msg;
	}
}
public class NetSvc
{
	private static NetSvc instance = null;
	public static NetSvc Instance {
		get {
			if (instance == null)
			{
				instance = new NetSvc();
			}
			return instance;
		}
	}

	public static readonly string obj = "lock";
	private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

	public void Init()
	{
		PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
		server.StartAsServer(ServerConfig.serverIP, ServerConfig.serverPort);

		PECommon.Log("NetSvc Init Done.");
	}

	public void AddMsgQue(MsgPack pack)
	{
		lock (obj)
		{
			msgPackQue.Enqueue(pack);
		}
	}

	public void Update()
	{
		if (msgPackQue.Count > 0)
		{
			//PECommon.Log("PackCount:" + msgPackQue.Count);
			lock (obj)
			{
				MsgPack pack = msgPackQue.Dequeue();
				HandOutMsg(pack);
			}
		}
	}

	private void HandOutMsg(MsgPack pack)
	{
		switch ((CMD)pack.msg.cmd)
		{
			case CMD.ReqLogin:
				LoginSys.Instance.ReqLogin(pack);
				break;
			case CMD.ReqRename:
				LoginSys.Instance.ReqRename(pack);
				break;
			case CMD.ReqGuide:
				GuideSys.Instance.ReqGuide(pack);
				break;
			case CMD.ReqStrong:
				StrongSys.Instance.ReqStrong(pack);
				break;
			case CMD.SndChat:
				ChatSys.Instance.SndChat(pack);
				break;
			case CMD.ReqBuy:
				BuySys.Instance.ReqBuy(pack);
				break;
			case CMD.ReqTakeTaskReward:
				TaskSys.Instance.ReqTakeTaskReward(pack);
				break;
			case CMD.ReqFBFight:
				FubenSys.Instance.ReqFBFight(pack);
				break;
			case CMD.ReqFBFightEnd:
				FubenSys.Instance.ReqFBFightEnd(pack);
				break;
		}
	}
}
