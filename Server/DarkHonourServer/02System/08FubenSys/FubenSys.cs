/****************************************************
	文件：FubenSys
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 15:20:35   	
	功能：副本战斗业务
*****************************************************/
using PEProtocol;
using System.Runtime.CompilerServices;

public class FubenSys
{
    private static FubenSys instance = null;
    public static FubenSys Instance {
        get {
            if (instance == null)
            {
                instance = new FubenSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        PECommon.Log("FubenSys Init Done.");
    }

    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.msg.reqFBFight;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFight
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapCfg(data.fbId).power;

        if (pd.fuben < data.fbId)
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < power)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                RspFBFight rspFBFight = new RspFBFight
                {
                    fbId = data.fbId,
                    power = pd.power
                };
                msg.rspFBFight = rspFBFight;
            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ReqFBFightEnd(MsgPack pack)
    {
        ReqFBFightEnd data = pack.msg.reqFBFightEnd;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFightEnd
        };
        // 校验战斗是否合法
        if (data.win)
        {
            if (data.costtime > 0 && data.resthp > 0)
            {
                // 根据副本ID获取相应奖励
                MapCfg rd = cfgSvc.GetMapCfg(data.fbid);
                PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

                // 任务进度数据更新
                PshTaskPrgs pshTaskPrgs = TaskSys.Instance.GetTaskPrgs(pd, 2);

                pd.coin += rd.coin;
                pd.crystal += rd.crystal;
                PECommon.CalcExp(pd, rd.exp);

                if (pd.fuben == data.fbid)
                {
                    pd.fuben += 1;
                }

                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    RspFBFightEnd rspFBFighten = new RspFBFightEnd
                    {
                        win = data.win,
                        fbid = data.fbid,
                        resthp = data.resthp,
                        costtime = data.costtime,

                        coin = pd.coin,
                        lv = pd.lv,
                        exp = pd.exp,
                        crystal = pd.crystal,
                        fuben = pd.fuben
                    };

                    msg.rspFBFightEnd = rspFBFighten;
                    msg.pshTaskPrgs = pshTaskPrgs;
                }
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }

        pack.session.SendMsg(msg);
    }
}