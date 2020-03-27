/****************************************************
	文件：Class1
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/2 22:20:55   	
	功能：数据库管理类
*****************************************************/
using MySql.Data.MySqlClient;
using PEProtocol;
using System;

public class DBMgr
{

    private static DBMgr instance = null;
    public static DBMgr Instance {
        get {
            if (instance == null)
            {
                instance = new DBMgr();
            }
            return instance;
        }
    }
    private MySqlConnection conn;

    public void Init()
    {
        conn = new MySqlConnection("server=localhost;User Id = root;password=;Database=darkgod;Charset=utf8");
        try
        {
            conn.Open();
        }
        catch (Exception e)
        {

        }
        PECommon.Log("DBMgr Init Done.");

        QueryPlayerData("xxx", "000");
    }

    public PlayerData QueryPlayerData(string acct, string pass)
    {
        PlayerData playerData = null;
        MySqlDataReader reader = null;
        bool isNew = true;
        try
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                isNew = false;
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    // 密码正确，返回玩家数据
                    playerData = new PlayerData
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond")
                        // TOADD
                    };
                }
            }
        }
        catch (Exception e)
        {
            PECommon.Log("Query PlayerData By Acct & Pass Error:" + e, LogType.Error);
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (isNew)
            {
                // 不存在账号数据，创建新的默认账号数据，并返回
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500
                    // TOADD
                };
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }

        return playerData;
    }

    private int InsertNewAcctData(string acct, string pass, PlayerData pd)
    {
        int id = -1;
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct,pass =@pass,name=@name,level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond", conn);
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pd.name);
            cmd.Parameters.AddWithValue("level", pd.lv);
            cmd.Parameters.AddWithValue("exp", pd.exp);
            cmd.Parameters.AddWithValue("power", pd.power);
            cmd.Parameters.AddWithValue("coin", pd.coin);
            cmd.Parameters.AddWithValue("diamond", pd.diamond);
            //TOADD
            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e)
        {
            PECommon.Log("Insert PlayerData Error:" + e, LogType.Error);
        }
        return id;
    }
}
