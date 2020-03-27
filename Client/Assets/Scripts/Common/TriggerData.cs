/****************************************************
    文件：TriggerData.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/3/17 23:7:37
	功能：地图触发数据类
*****************************************************/

using UnityEngine;

public class TriggerData : MonoBehaviour 
{
    public int triggerWave;
    public MapMgr mapMgr;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (mapMgr != null)
            {
                mapMgr.TriggerMonsterBorn(this, triggerWave);
            }
        }
    }
}