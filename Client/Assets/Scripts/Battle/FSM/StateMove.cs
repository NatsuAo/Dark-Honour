/****************************************************
	文件：StateMove
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 22:13:22   	
	功能：移动状态
*****************************************************/
public class StateMove : IState
{
	public void Enter(EntityBase entity, params object[] args)
	{
		entity.currentAniState = AniState.Move;
		//PECommon.Log("Enter StateMove");
	}

	public void Exit(EntityBase entity, params object[] args)
	{
		//PECommon.Log("Exit StateMove");
	}

	public void Process(EntityBase entity, params object[] args)
	{
		//PECommon.Log("Process StateMove");
		entity.SetBlend(Constants.BlendMove);
	}
}