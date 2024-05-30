using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlEntity : MonoBehaviour
{
    public GameObject LockedObj;
    public GameObject[] Stars;
    int currentLevelNumber = -1;
    
    public void SetUI(int modenumberMultiplier)
    {
//        Debug.Log("modenumberMultiplier" + modenumberMultiplier);
        if (modenumberMultiplier < 0) modenumberMultiplier = 0;

        currentLevelNumber = (int.Parse(gameObject.name)) + (modenumberMultiplier);
        //Debug.Log("currentLevelNumberUI" + currentLevelNumber);

        string LvlNumberString = "LvlUnlocked" + currentLevelNumber;

        if ((PlayerPrefs.GetInt(LvlNumberString) == 1)||(gameObject.name=="0"))
        {
            LockedObj.SetActive(false);
            int Lvl_stars = PlayerPrefs.GetInt("LvlStars"+ currentLevelNumber);

            for (int i = 0; i < 3; i++)
            {
                Stars[i].SetActive(false);
            }
            for (int i = 0; i < Lvl_stars; i++)
            {
                Stars[i].SetActive(true);
            }
            GetComponent<Button>().enabled = true;
        }
        else
        {
            GetComponent<Button>().enabled = false;
            LockedObj.SetActive(true);
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i].SetActive(false);
            }
        }
    }

    public void SelectLevel()
    {
        CalCulateLevelUINumber();
        Sound_Manager.instance.PlayOnshootSound(Sound_Manager.instance.buttonClick);
        PlayerPrefs.SetInt("CurrentLevelNo", currentLevelNumber);
        PlayerPrefs.SetInt(GameConstants.CurrentLevelNo, currentLevelNumber);

        //Debug.Log("lvlClicked: "+PlayerPrefs.GetInt(GameConstants.CurrentLevelNo));

        SceneManager.LoadScene("Game Play");
    }

    void CalCulateLevelUINumber()
    {
        int _currentLevelNumber = int.Parse(gameObject.name);
        if (_currentLevelNumber > 4) _currentLevelNumber = 5 - _currentLevelNumber;
        PlayerPrefs.SetInt("ArrowUiLvlPos", _currentLevelNumber);
    }
}
