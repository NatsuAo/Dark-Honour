/****************************************************
	文件：StateIdle
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 22:12:16   	
	功能：待机状态
*****************************************************/
using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        entity.skEndCB = -1;
        //PECommon.Log("Enter StateIdle");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Exit StateIdle");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.nextSkillID != 0)
        {
            entity.Attack(entity.nextSkillID);
        }
        else
        {
            if (entity.entityType == EntityType.Player)
            {
                entity.canRlsSkill = true;
            }
            if (entity.GetDirInput() != Vector2.zero)
            {
                entity.Move();
                // 手动设置方向来获取当前摇杆的偏移量来判断是否要移动
                entity.SetDir(entity.GetDirInput());
            }
            else
            {
                entity.SetBlend(Constants.BlendIdle);
            }
            //PECommon.Log("Process StateIdle");
        }
    }
}

