using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject ProfilePopup,Settings_Slider,ShopePanel,LeaderboardPanel,LvlSelection,ModeSelection;
    bool IsSliderOpen;
    public InputField nameField;
    public Image Music_img, vib_img;


    //leaderboard
    public GameObject Leaderboard_Entry_prefab, LeaderboardContainer;
    public Sprite[] Entry_bg_sp;

    //
    public Text[] StarsAmount;
    public Sprite[] Music_sp, vib_sp;

    public GameObject[] Mode_Locks;
    public GameObject[] Levels_UI;
    public int Selected_mode=-1;

    public string PrivacyLink;
    public GameObject RestorePurchases;
    void Start()
    {

#if UNITY_IOS
        RestorePurchases.SetActive(true);
#endif

        if (!PlayerPrefs.HasKey(GameConstants.SFX))
        {
            PlayerPrefs.SetInt(GameConstants.SFX, 1);
            PlayerPrefs.SetInt(GameConstants.Music, 1);
            PlayerPrefs.SetInt(GameConstants.Vibration, 1);
            PlayerPrefs.SetInt(GameConstants.TotalStars, 0);
            PlayerPrefs.SetInt(GameConstants.OpenModes, 0);
            ProfilePopup.SetActive(true);
        }
        if (PlayerPrefs.GetInt(GameConstants.Music) == 1)
        {
            Music_img.sprite = Music_sp[0];
            vib_img.sprite = vib_sp[0];

        }
        else
        {
            Music_img.sprite = Music_sp[1];
             vib_img.sprite=vib_sp[1];

        }

        nameField.text = PlayerPrefs.GetString("Name");
        for (int i = 0; i < StarsAmount.Length; i++)
        {
            StarsAmount[i].text = PlayerPrefs.GetInt("TotalStars").ToString();
        }
        SpawnEntries();//leaderboard
    }

   
    public void PlaySfx()
    {
            HapticManager.instance.click();

        if (PlayerPrefs.GetInt(GameConstants.SFX) == 0) return;
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
    }

    public void SaveName()
    {
        PlaySfx();
        ProfilePopup.SetActive(false);
        PlayerPrefs.SetString("Name", nameField.text);
    }

    public void OnClick_PlayBtn()
    {
        PlaySfx();

        SceneManager.LoadScene("Game Play");
    }

    public void OnClick_ModeBtn()
    {
        PlaySfx();
        AD_Manager.instance.ShowInterstitialAd();

        for (int i = 0; i < PlayerPrefs.GetInt(GameConstants.OpenModes); i++)
        {
            Mode_Locks[i].transform.parent.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            Mode_Locks[i].transform.parent.gameObject.GetComponent<Button>().enabled = true ;
            Mode_Locks[i].SetActive(false);
        }

        
        ModeSelection.SetActive(true);
    }

    public void OnSelectMode(int modeNumber)
    {
        PlaySfx();
        AD_Manager.instance.ShowInterstitialAd();

        Selected_mode = modeNumber;
        PlayerPrefs.SetInt("Mode",modeNumber);
        SetLevelsUI();
    }

    public void OnClick_LevelPanelBtn()
    {
        PlaySfx();
        AD_Manager.instance.ShowInterstitialAd();

        SetLevelsUI();
    }

    void SetLevelsUI()//to be continued
    {
            if (Selected_mode == -1)
            {
                Selected_mode=GetModifier();
            }
            else
            {
                Selected_mode = Selected_mode * 10;
            }
        for (int i = 0; i < Levels_UI.Length; i++)
        {
            Levels_UI[i].GetComponent<LvlEntity>().SetUI(Selected_mode);
        }
        LvlSelection.SetActive(true);
    }

    int GetModifier()
    {
         int modifier = 0;
         modifier = 10 * PlayerPrefs.GetInt(GameConstants.OpenModes);
          return modifier;
    }
   //

    public void OnClick_ShopBtn()
    {
        PlaySfx();
        AD_Manager.instance.ShowInterstitialAd();

        ShopePanel.SetActive(true);
    }

    public void OnClick_LeaderboardBtn()
    {
        PlaySfx();
        Debug.Log("OnClick_LeaderboardBtn");
        AD_Manager.instance.ShowInterstitialAd();

        LeaderboardPanel.SetActive(true);
    }

    public void OnClick_OnAdFree_Success()
    {
        PlayerPrefs.SetInt(GameConstants.RemoveAds, 1);
    }

    public void OnClick_SettingBtn()
    {
        PlaySfx();
        if (!IsSliderOpen)
        {
            IsSliderOpen = true;
            Settings_Slider.SetActive(true);

        }
        else
        {
            IsSliderOpen = false;
            Settings_Slider.SetActive(false);
        }
    }


    public void OnClick_MusicBtn()
    {
        Debug.Log("Music "+PlayerPrefs.GetInt(GameConstants.Music));

        if (PlayerPrefs.GetInt(GameConstants.Music) == 0)//unmute
        {
            PlaySfx();
            PlayerPrefs.SetInt(GameConstants.SFX, 1);
            PlayerPrefs.SetInt(GameConstants.Music, 1);
            Sound_Manager.instance.MuteSounds();

        }
        else
        {
            
            PlayerPrefs.SetInt(GameConstants.SFX, 0);
            PlayerPrefs.SetInt(GameConstants.Music, 0);

            Sound_Manager.instance.MuteSounds();
        }
        Music_img.sprite = Music_sp[PlayerPrefs.GetInt(GameConstants.Music)];
    }

    public void OnClick_VibrationBtn()
    {
        PlaySfx();
        if (PlayerPrefs.GetInt(GameConstants.Vibration) == 1)
        {
            PlayerPrefs.SetInt(GameConstants.Vibration, 0);

        }
        else
        {
            PlayerPrefs.SetInt(GameConstants.Vibration, 1);

        }
        vib_img.sprite = vib_sp[PlayerPrefs.GetInt(GameConstants.Vibration)];

    }

    public void OnClick_PrivacyBtn()
    {
        PlaySfx();
        Application.OpenURL(PrivacyLink);
    }


    public void OnclickBackBtn()
    {
        PlaySfx();
        ShopePanel.SetActive(false);
        LeaderboardPanel.SetActive(false);
        LvlSelection.SetActive(false);
        ModeSelection.SetActive(false);

    }

    //Leaderboard

    void SpawnEntries()
    {
        int bg_number = 0;
        for (int i = 0; i < LeaderBoard_Manager.instance.Name.Count; i++)
        {
//            Debug.Log("SpawnEntries");
            GameObject Entry= Instantiate(Leaderboard_Entry_prefab, LeaderboardContainer.transform);
            Entry.GetComponent<RecordEntity>().record_bg.sprite = Entry_bg_sp[bg_number];
            Entry.GetComponent<RecordEntity>().Name.text = LeaderBoard_Manager.instance.Name[i];
            Entry.GetComponent<RecordEntity>().Score.text = LeaderBoard_Manager.instance.Score[i].ToString();

            bg_number++;
            if (bg_number >= Entry_bg_sp.Length) bg_number = 0;
        }
    }


    public void OnHintBought(int hintCount)
    {
        PlaySfx();
        PlayerPrefs.SetInt(GameConstants.HintCount, PlayerPrefs.GetInt(GameConstants.HintCount)+ hintCount);
    }

    public void OnBuyRemoveAds()
    {
        PlayerPrefs.SetInt(GameConstants.RemoveAds, 1);
    }
}
