/****************************************************
	文件：MonsterController
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/13 20:13:10   	
	功能：怪物表现实体角色控制器
*****************************************************/

using UnityEngine;

public class MonsterController : Controller
{
	private void Update()
	{
		// AI逻辑表现
		if (isMove)
		{
			SetDir();
			SetMove();
		}
	}
	private void SetDir()
	{
		// 摄像机里角色的朝向相当于是在XY坐标系上取上下左右，于是这里就计算其目标朝向与当前朝向（即默认朝向上方）的夹角
		float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));
		// 再将这个夹角转换到世界坐标系里去，那就是角色绕Y轴旋转，在XZ坐标系上看旋转角度
		Vector3 eulerAngles = new Vector3(0, angle, 0);
		transform.localEulerAngles = eulerAngles;
	}
	private void SetMove()
	{
		ctrl.Move(transform.forward * Time.deltaTime * Constants.MonsterMoveSpeed);
		// 给一个向下的速度，便于在没有apply root motion时怪物可以落地，Fix Res Error
		ctrl.Move(Vector3.down * Time.deltaTime * Constants.MonsterMoveSpeed);
	}
}