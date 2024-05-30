using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance;
    public GameObject LevelSelection, Content;
    public ScrollRect LevelScroll;
    public Sprite CurrentLevelColor;
    public Image Paths;
    public Text LevelNo;
    public int LoadingTime = 0;
    public float[] FillAmountpos;
//    public HapticManager Haptic_Manager;
    public Transform Parent;
    public GameObject[] level_button_prefab;
    public float[] x_pos;
    public bool IsRight = false;
    public float y_pos = 9461;

    public int Levels_To_Spawn;

    public bool Is_Test;
    public int UnlockLevelNo = 0;
    public bool Off_Debugs;

    private void Start()
    {
        Instance = this;
        if (Is_Test)
        {
            PlayerPrefs.SetInt(GameConstants.UnlockLevel, UnlockLevelNo);
        }
        if (Off_Debugs)
        {
            Debug.Log("Turning Off Debug Logs");
          //  Debug.unityLogger.logEnabled = false;
        }
        CheckInApps();
        SpawnLevels();
        Invoke(nameof(BannerCall), 1f);
    }
    void BannerCall()
    {
        if (Sound_Manager.instance.ad_manager == null) Sound_Manager.instance.ad_manager = GameObject.Find("AdsManager");

//        Sound_Manager.instance.ad_manager.GetComponent<AdManager>().ShowBanner();

        //if (AdManager.Instance != null)
        //{
        //    
        //}
        //else
        //{
        //    Debug.LogError("No Intance Found!");
        //}
    }



    public void SpawnLevels()
    {
        for (int i = 0; i < Levels_To_Spawn; i++)//100 levels
        {
            int index = Convert.ToInt32(IsRight);
            // Debug.Log("index " + index);
            GameObject Lvl_btn = Instantiate(level_button_prefab[index], Parent);
            Lvl_btn.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x_pos[index], y_pos);
            Lvl_btn.transform.GetChild(0).GetComponent<OnClickLevel>().my_Number = i + 1;
            Lvl_btn.name = Lvl_btn.name + (i + 1).ToString();
            y_pos = y_pos - 193;
            IsRight = !IsRight;
        }
        StartCoroutine(FillLine());
    }

    float offset = 0.024f, Diff = 0.0097f, firstoffset = 0.01f;
    IEnumerator FillLine()
    {
        int UnlockedLevel = PlayerPrefs.GetInt(GameConstants.UnlockLevel);
        float Path_Fill_Value;
        if (UnlockedLevel == 0)
        {
            firstoffset = 0;
            Debug.Log("UnlockedLevel is 0 offset value:" + firstoffset);
            Path_Fill_Value = offset;
        }
        else
        {
            Debug.Log("Unlocked Level Greater than 0");
            Path_Fill_Value = (offset + firstoffset) + (Diff * (UnlockLevelNo));
            Debug.Log("Filler Value : " + Path_Fill_Value);

        }
        yield return new WaitForSeconds(0.01f);
        Debug.Log("UnlockedLevel " + UnlockedLevel);
        //Path_Fill_Value = (offset + firstoffset) + (Diff * (UnlockLevelNo-1));
        Debug.Log("Path Fill Value : " + Path_Fill_Value);
        //Paths.DOFillAmount(Path_Fill_Value,2f);
        Debug.Log("Unlocked Level is :" + UnlockedLevel);
        float Target = Content.transform.position.y - Parent.transform.GetChild(UnlockedLevel).GetComponent<RectTransform>().transform.position.y;
        Content.transform.DOMoveY(Target, 2f);
    }

    public GameObject RemoveAds_Btn;
    public void CheckInApps()
    {
        if (PlayerPrefs.GetInt(GameConstants.RemoveAds) == 1)
        {
            RemoveAds_Btn.SetActive(false);
        }
    }

    public void SoundOnly()
    {
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
    }
}