/****************************************************
    文件：ItemEntityHP.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/3/15 11:39:10
	功能：血条物体
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHP : MonoBehaviour
{
    #region UI Define
    public Image imgHPGray;
    public Image imgHPRed;

    public Animation criticalAnim;
    public Text txtCritical;

    public Animation dodgeAnim;
    public Text txtDodge;

    public Animation hpAnim;
    public Text txtHp;
    #endregion

    private RectTransform rect;
    private Transform rootTrans;
    private int hpVal;
    private float scaleRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;

        UpdateMixBlend();
        imgHPGray.fillAmount = currentPrg;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
    }

    public void InitItemInfo(Transform trans ,int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHPGray.fillAmount = 1;
        imgHPRed.fillAmount = 1;
    }

    public void SetDodge()
    {
        dodgeAnim.Stop();
        txtCritical.text = "闪避";
        dodgeAnim.Play();
    }

    public void SetCritical(int critical)
    {
        criticalAnim.Stop();
        txtCritical.text = "暴击 " + critical;
        criticalAnim.Play();
    }
    public void SetHurt(int hurt)
    {
        hpAnim.Stop();
        txtHp.text = "-" + hurt;
        hpAnim.Play();
    }

    private float currentPrg;
    private float targetPrg;

    public void SetHPVal(int oldVal, int newVal)
    {
        currentPrg = oldVal * 1.0f / hpVal;
        targetPrg = newVal * 1.0f / hpVal;
        imgHPRed.fillAmount = targetPrg;
    }
}