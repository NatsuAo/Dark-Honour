/****************************************************
    文件：DynamicWnd.cs
	作者：NatsuAo
    邮箱: yinhao7700@163.com
    日期：2020/2/28 23:32:21
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot
{
    public Animation tipsAnim;
    public Text txtTips;
    public Transform hpItemRoot;

    public Animation selfDodgeAnim;

    private bool IsTipsShow;
    private Queue<string> tipsQue = new Queue<string>();
    private Dictionary<string, ItemEntityHP> itemDic = new Dictionary<string, ItemEntityHP>();

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips, false);
    }

    private void Update()
    {
        if (tipsQue.Count > 0 && IsTipsShow == false)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                IsTipsShow = true;
                SetTips(tips);
            }
        }
    }

    #region Tips相关

    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            tipsQue.Enqueue(tips);
        }
    }

    private void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAnim.GetClip("TipsShowAnim");
        tipsAnim.Play();
        // 延时关闭激活状态
        StartCoroutine(AnimPlayDone(clip.length, () =>
        {
            SetActive(txtTips, false);
            IsTipsShow = false;
        }));
    }

    private IEnumerator AnimPlayDone(float sec, Action callback)
    {
        yield return new WaitForSeconds(sec);
        if (callback != null)
        {
            callback();
        }
    }
    #endregion

    public void AddHpItemInfo(string mName, Transform trans, int hp)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.HPItemPrefab, new Vector3(-1000, 0, 0), true);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            go.transform.SetParent(hpItemRoot);
            ItemEntityHP leh = go.GetComponent<ItemEntityHP>();
            leh.InitItemInfo(trans,hp);
            itemDic.Add(mName, leh);
        }
    }
    public void RmvHpItemInfo(string mName)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(mName, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(mName);
            return;
        }
    }

    public void RmvAllHpItemInfo()
    {
        foreach (var item in itemDic)
        {
            Destroy(item.Value.gameObject);
        }
        itemDic.Clear();
    }

    public void SetDodge(string key)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetDodge();
        }
    }
    public void SetCritical(string key, int critical)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }
    public void SetHurt(string key, int hurt)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }
    public void SetHPVal(string key, int oldVal, int newVal)
    {
        ItemEntityHP item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHPVal(oldVal, newVal);
        }
    }

    public void SetSelfDodge()
    {
        selfDodgeAnim.Stop();
        selfDodgeAnim.Play();
    }
}