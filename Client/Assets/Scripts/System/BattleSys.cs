/****************************************************
	文件：BattleSys
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 16:15:24   	
	功能：战斗业务系统
*****************************************************/
using PEProtocol;
using UnityEngine;

public class BattleSys : SystemRoot
{
    public static BattleSys Instance = null;
    public PlayerCtrlWnd playerCtrlWnd;
    public BattleEndWnd battleEndWnd;

    public BattleMgr battleMgr;

    private int fbid;
    private double startTime;
    public override void InitSys()
    {
        base.InitSys();

        PECommon.Log("Init BattleSys...");
        Instance = this;
    }

    public void StartBattle(int mapId)
    {
        fbid = mapId;
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();

        battleMgr.Init(mapId, () =>
        {
            startTime = timerSvc.GetNowTime();
        });
        SetPlayerCtrlWndState();
    }

    public void EndBattle(bool isWin, int restHP)
    {
        playerCtrlWnd.SetWndState(false);
        GameRoot.Instance.dynamicWnd.RmvAllHpItemInfo();

        if (isWin)
        {
            double endTime = timerSvc.GetNowTime();
            // 发送结算战斗请求
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqFBFightEnd,
                reqFBFightEnd = new ReqFBFightEnd
                {
                    win = isWin,
                    fbid = fbid,
                    resthp = restHP,
                    costtime = (int)((endTime - startTime) / 1000)
                }
            };

            netSvc.SendMsg(msg);
        }
        else
        {
            SetBattleEndWndState(FBEndType.Lose);
        }
    }

    public void DestroyBattle()
    {
        SetPlayerCtrlWndState(false);
        SetBattleEndWndState(FBEndType.None, false);
        GameRoot.Instance.dynamicWnd.RmvAllHpItemInfo();
        Destroy(battleMgr.gameObject);
    }

    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetBattleEndWndState(FBEndType endType, bool isActive = true)
    {
        battleEndWnd.SetWndType(endType);
        battleEndWnd.SetWndState(isActive);
        battleMgr.entitySelfPlayer.SetDir(Vector2.zero);
        battleMgr.entitySelfPlayer.SetBlend(Constants.BlendIdle);
    }

    public void RspFightEnd(GameMsg msg)
    {
        RspFBFightEnd data = msg.rspFBFightEnd;
        GameRoot.Instance.SetPlayerDataByFBEnd(data);

        battleEndWnd.SetBattleEndData(data.fbid, data.costtime, data.resthp);
        SetBattleEndWndState(FBEndType.Win);
    }

    public void SetMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playerCtrlWnd.currentDir;
    }
}