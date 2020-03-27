/****************************************************
	文件：IState
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 21:48:40   	
	功能：状态接口
*****************************************************/
public interface IState
{
	void Enter(EntityBase entity, params object[] args); // 可变参数

	void Process(EntityBase entity, params object[] args);

	void Exit(EntityBase entity, params object[] args);
}

public enum AniState
{
	None,
	Born,
	Idle,
	Move,
	Attack,
	Hit,
	Die
}