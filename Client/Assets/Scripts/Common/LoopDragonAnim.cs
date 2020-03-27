/****************************************************
    文件：LoopDragonAnim.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/2/28 23:19:53
	功能：飞龙循环动画
*****************************************************/

using UnityEngine;

public class LoopDragonAnim : MonoBehaviour 
{
    private Animation anim;

    private void Awake()
    {
        anim = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if (anim != null)
        {
            InvokeRepeating("PlayerDragonAnim", 0, 20);
        }
    }

    private void PlayerDragonAnim()
    {
        if (anim != null)
        {
            anim.Play();
        }
    }
}