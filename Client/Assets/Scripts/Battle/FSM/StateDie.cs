/****************************************************
	文件：StateDie
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/14 22:41:28   	
	功能：死亡状态
*****************************************************/
public class StateDie : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Die;
        entity.RmvSkillCB();
    }

    public void Exit(EntityBase entity, params object[] args)
    {

    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        if (entity.entityType == EntityType.Monster)
        {
            entity.GetCC().enabled = false;
            TimerSvc.Instance.AddTimeTask((int tid) =>
            {
                entity.SetActive(false);
            }, Constants.DieAniLength);
        }
    }
}