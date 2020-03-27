/****************************************************
	文件：ChatSys
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/8 14:18:49   	
	功能：聊天业务系统
*****************************************************/

using PEProtocol;
using System.Collections.Generic;

public class ChatSys
{

	private static ChatSys instance = null;
	public static ChatSys Instance {
		get {
			if (instance == null)
			{
				instance = new ChatSys();
			}
			return instance;
		}
	}
	private CacheSvc cacheSvc = null;

	public void Init()
	{
		cacheSvc = CacheSvc.Instance;
		PECommon.Log("ChatSys Init Done.");
	}

	public void SndChat(MsgPack pack)
	{
		SndChat data = pack.msg.sndChat;
		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

		// 任务进度数据更新
		PshTaskPrgs pshTaskPrgs = TaskSys.Instance.GetTaskPrgs(pd, 6);

		GameMsg msg = new GameMsg
		{
			cmd = (int)CMD.PshChat,
			pshChat = new PshChat
			{
				name = pd.name,
				chat = data.chat
			},
			pshTaskPrgs = pshTaskPrgs
		};

		// 广播所有在线客户端
		List<ServerSession> lst = cacheSvc.GetOnlineServerSessions();
		// 重点优化，因为要给很多个客户端发送相同的数据，如果重复地序列化成二进制发送会重复浪费cpu，所以可以先序列化成二进制再重复发送
		byte[] bytes = PENet.PETool.PackNetMsg(msg);
		
		for (int i = 0; i < lst.Count; i++)
		{
			lst[i].SendMsg(bytes);
		}
	}
}
