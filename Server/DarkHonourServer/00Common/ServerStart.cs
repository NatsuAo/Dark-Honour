/****************************************************
	文件：ServerStart
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/2/29 20:38:53
	功能：服务器入口
*****************************************************/
using System;
using System.Threading;

class ServerStart
{
    static void Main(string[] args)
    {
		ServerRoot.Instance.Init();

		while (true)
		{
			ServerRoot.Instance.Update();
			Thread.Sleep(20);
		}
    }
}
