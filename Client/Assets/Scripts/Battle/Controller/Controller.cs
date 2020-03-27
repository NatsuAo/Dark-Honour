/****************************************************
	文件：Controller
	作者：NatsuAo
	邮箱: yinhao7700@163.com
	日期：2020/3/11 23:19:55   	
	功能：表现实体控制器抽象基类
*****************************************************/
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
	public Animator anim;
    public CharacterController ctrl;
    public Transform hpRoot;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir {
        get => dir; set {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    protected Transform camTrans;

    protected bool SkillMove = false;
    protected float skillMoveSpeed = 0f;

    protected TimerSvc timerSvc;

    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    public virtual void Init()
    {
        timerSvc = TimerSvc.Instance;
    }

    public virtual void SetBlend(float blend)
    {
        anim.SetFloat("Blend", blend);
    }
    public virtual void SetAction(int act)
    {
        anim.SetInteger("Action", act);
    }

    public virtual void SetFX(string name, float destroy)
    {

    }

    public void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        SkillMove = move;
        skillMoveSpeed = skillSpeed;
    }

    public virtual void SetAtkRotationLocal(Vector2 atkDir)
    {
        // 摄像机里角色的朝向相当于是在XY坐标系上取上下左右，于是这里就计算其目标朝向与当前朝向（即默认朝向上方）的夹角
        float angle = Vector2.SignedAngle(atkDir, new Vector2(0, 1));
        // 再将这个夹角转换到世界坐标系里去，那就是角色绕Y轴旋转，在XZ坐标系上看旋转角度
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
    public virtual void SetAtkRotationCam(Vector2 camDir)
    {
        // 摄像机里角色的朝向相当于是在XY坐标系上取上下左右，于是这里就计算其目标朝向与当前朝向（即默认朝向上方）的夹角
        float angle = Vector2.SignedAngle(camDir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        // 再将这个夹角转换到世界坐标系里去，那就是角色绕Y轴旋转，在XZ坐标系上看旋转角度
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
}