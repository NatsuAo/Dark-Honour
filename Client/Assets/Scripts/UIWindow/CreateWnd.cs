/****************************************************
    文件：CreateWnd.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/2/29 0:17:0
	功能：Nothing
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot 
{
    public InputField iptName;
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    // 显示一个随机名字
    public void ClickRandomBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        string rdName = resSvc.GetRandomNameData(false);
        iptName.text = rdName;
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (iptName.text != "")
        {
            // 发送名字数据倒服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                reqRename = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }
}