/****************************************************
	文件：FubenSys
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/10 20:30:47   	
	功能：副本业务系统
*****************************************************/
using PEProtocol;

public class FubenSys : SystemRoot
{
    public static FubenSys Instance = null;

    public FubenWnd fubenWnd;
    public override void InitSys()
    {
        base.InitSys();

        PECommon.Log("Init FubenSys...");
        Instance = this;
    }

    public void EnterFuben()
    {
        SetFubenWndState();
    }

    #region Fuben Wnd
    public void SetFubenWndState(bool isActive = true)
    {
        fubenWnd.SetWndState(isActive);
    }
    #endregion

    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByBFStart(msg.rspFBFight);

        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetFubenWndState(false);

        BattleSys.Instance.StartBattle(msg.rspFBFight.fbId);
    }
}
