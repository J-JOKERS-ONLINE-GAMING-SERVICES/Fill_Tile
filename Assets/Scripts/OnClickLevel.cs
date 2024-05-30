using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClickLevel : MonoBehaviour
{
    public Text lvl_numb;
    public GameObject line;
    public Image Button;
    public Sprite inactive, active, bossActive, bossInactive;
    public int my_Number = 0;
    public GameObject[] Stars;

    private void Start()
    {
        int Index = PlayerPrefs.GetInt("StarsLevel" + (my_Number - 1));

        for (int i = 0; i < Index; i++)
        {
            Stars[i].SetActive(true);
        }

        if (PlayerPrefs.GetInt(GameConstants.UnlockLevel) >= (my_Number - 1))
        {
            if (my_Number % 5 == 0)
            {
                Button.sprite = bossActive;
            }
            else
            {
                Button.sprite = active;
                lvl_numb.text = my_Number.ToString();
            }
            line.GetComponent<Button>().interactable = true;
            Button.GetComponent<Button>().interactable = true;

        }
        else
        {
            if (my_Number % 5 == 0)
            {
                Button.sprite = bossInactive;
            }
            else
            {
                Button.sprite = inactive;
                lvl_numb.text = my_Number.ToString();
            }
        }
    }
   // public HapticManager Haptic_Manager;

    public void OnClick(GameObject Button)
    {
        //  int UnlockLevel = int.Parse(Button.name);
        Debug.Log("UnlockLevel :" + (my_Number - 1));
        if (PlayerPrefs.GetInt(GameConstants.Vibration) == 0)
        {
//            MenuHandler.Instance.Haptic_Manager.TriggerHapticFeedback(0.1f, 0.017f, 0.001f);
        }
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        PlayerPrefs.SetInt(GameConstants.CurrentLevelNo, ((my_Number - 1)));
        Debug.Log("Selected Level :" + PlayerPrefs.GetInt(GameConstants.CurrentLevelNo));


        //Ali
        /*        if (Sound_Manager.instance.ad_manager == null) Sound_Manager.instance.ad_manager = GameObject.Find("AdsManager");
                Sound_Manager.instance.ad_manager.GetComponent<AdManager>().ShowInterstitial();*/

        if (my_Number == 1)
        {
            SceneManager.LoadSceneAsync(3);
        }
        else
        {
            SceneManager.LoadSceneAsync(2);
        }
       
    }
}


//x=270
//y=9461


//x=-270
//y=9268