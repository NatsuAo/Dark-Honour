/****************************************************
    文件：MainCityWnd.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/3/3 20:26:38
	功能：主城UI界面
*****************************************************/

using PEProtocol;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot
{
    #region UIDefine
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Animation menuAnim;
    public Button btnMenu;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    public Button btnGuide;
    #endregion

    private bool menuState = true;
    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;
    private AutoGuideCfg curtTaskData;

    #region MainFunctions
    protected override void InitWnd()
    {
        base.InitWnd();

        // 屏幕自适应
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);

        RegisterTouchEvents();

        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtPower, $"体力:{pd.power}/{PECommon.GetPowerLimit(pd.lv)}");
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);

        #region 经验条
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");
        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        // 计算UI在当前屏幕高度下的缩放比例，之所以用屏幕高度计算是因为canvas是选择根据高度进行缩放
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion

        // 设置自动任务图标
        curtTaskData = resSvc.GetGuideCfg(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuiodeBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuiodeBtnIcon(-1);
        }
    }

    private void SetGuiodeBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }

        SetSprite(img, spPath);
    }
    #endregion

    #region ClickEvents
    public void ClickFubenkBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.EnterFuben();
    }
    public void ClickTaskBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenTaskWnd();
    }
    public void ClickBuyPowerBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenBuyWnd(0);
    }
    public void ClickMKCoinBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenBuyWnd(1);
    }
    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenStrongWnd();
    }
    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务，正在开发中...");
        }
    }
    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);

        menuState = !menuState;
        AnimationClip clip = null;
        if (menuState)
        {
            clip = menuAnim.GetClip("OpenMCMenu");
        }
        else
        {
            clip = menuAnim.GetClip("CloseMCMenu");
        }
        menuAnim.Play(clip.name);
    }
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }
    public void ClickChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenChatWnd();
    }
    public void RegisterTouchEvents()
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint);
            // 因为evt传进来的是世界坐标，所以使用position
            imgDirBg.transform.position = evt.position;
        });
        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            // 这里使用的坐标是相对于父物体的本地坐标，所以使用localPosition
            imgDirPoint.transform.localPosition = Vector2.zero;
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
        });
        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            MainCitySys.Instance.SetMoveDir(dir.normalized);
        });
    }
    #endregion
}