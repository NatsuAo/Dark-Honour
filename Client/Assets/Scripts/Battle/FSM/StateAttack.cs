﻿/****************************************************
	文件：StateAttack
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/12 17:19:35   	
	功能：攻击状态
*****************************************************/
public class StateAttack : IState
{
	public void Enter(EntityBase entity, params object[] args)
	{
		entity.currentAniState = AniState.Attack;
		entity.curtSkillCfg = ResSvc.Instance.GetSkillCfg((int)args[0]);
		//PECommon.Log("Enter StateAttack");
	}

	public void Exit(EntityBase entity, params object[] args)
	{
		entity.ExitCurtSkill();
		//PECommon.Log("Exit StateAttack");
	}

	public void Process(EntityBase entity, params object[] args)
	{
		if (entity.entityType == EntityType.Player)
		{
			entity.canRlsSkill = false;
		}

		entity.SkillAttack((int)args[0]);
		//PECommon.Log("Process StateAttack");
	}
}