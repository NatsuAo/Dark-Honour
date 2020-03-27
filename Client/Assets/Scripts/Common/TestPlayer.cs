/****************************************************
    文件：TestPlayer.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/3/12 11:21:54
	功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Transform camTrans;
    private Vector3 camOffset;
    public CharacterController ctrl;

    private float targetBlend;
    private float currentBlend;
    public Animator anim;
    public GameObject daggerskill1fx;

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
    private void Start()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }
    private void Update()
    {
        #region Input

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero)
        {
            Dir = _dir;
            SetBlend(Constants.BlendMove);
        }
        else
        {
            Dir = Vector2.zero;
            SetBlend(Constants.BlendIdle);
        }

        #endregion

        if (currentBlend != targetBlend)
        {
            UpdateMixBlend();
        }

        if (isMove)
        {
            // 设置方向
            SetDir();

            // 产生移动
            SetMove();

            // 相机跟随
            SetCam();
        }
    }

    private void SetDir()
    {
        // 摄像机里角色的朝向相当于是在XY坐标系上取上下左右，于是这里就计算其目标朝向与当前朝向（即默认朝向上方）的夹角
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        // 再将这个夹角转换到世界坐标系里去，那就是角色绕Y轴旋转，在XZ坐标系上看旋转角度
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetCam()
    {
        if (camTrans != null)
        {
            camTrans.position = transform.position - camOffset;
        }
    }

    public void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        anim.SetFloat("Blend", currentBlend);
    }

    public void ClickSkill1Btn()
    {
        anim.SetInteger("Action", 1);
        daggerskill1fx.gameObject.SetActive(true);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.9f);
        daggerskill1fx.gameObject.SetActive(false);
        anim.SetInteger("Action", -1);
        
    }
}