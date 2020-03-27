﻿/****************************************************
	文件：LoginSys
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/2/29 20:59:09   	
	功能：登录业务系统
*****************************************************/
using PENet;
using PEProtocol;

public class LoginSys
{
    private static LoginSys instance = null;
    public static LoginSys Instance {
        get {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }
    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;
        // 当前账号是否已经上线
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin
        };
        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            // 已上线：返回错误信息
            msg.err = (int)ErrorCode.AcctIsOnLine;
        }
        else
        {
            // 未上线：
            // 账号是否存在
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                // 存在，密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                // 计算离线体力增长
                int power = pd.power;
                long now = timerSvc.GetNowTime();
                long milliseconds = now - pd.time;
                int addPower = (int)(milliseconds / (1000 * 60 * PECommon.PowerAddSpace)) * PECommon.PowerAddCount;
                if (addPower > 0)
                {
                    int powerMax = PECommon.GetPowerLimit(pd.lv);
                    if (pd.power < powerMax)
                    {
                        pd.power += addPower;
                        if (pd.power > powerMax)
                        {
                            pd.power = powerMax;
                        }
                    }
                }

                if (power != pd.power)
                {
                    cacheSvc.UpdatePlayerData(pd.id, pd);
                }

                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };
                // 缓存账号数据
                cacheSvc.AcctOnLine(data.acct, pack.session, pd);
            }
        }
        // 回应客户端
        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };
        // 名字是否已经存在
        if (cacheSvc.IsNameExist(data.name))
        {
            // 存在：返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            // 不存在：更新缓存，以及数据库，再返回给客户端
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ClearOfflineData(ServerSession session)
    {
        // 写入下线时间
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if (pd != null)
        {
            pd.time = timerSvc.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                PECommon.Log("Update offline time error", LogType.Error);
            }
            cacheSvc.AcctOffLine(session);
        }
    }
}
