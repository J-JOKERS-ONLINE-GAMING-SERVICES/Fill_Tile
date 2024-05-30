using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [System.Serializable]
    public struct UI
    {
        [SerializeField] public GameObject MainMenuPanel;
        [SerializeField] public GameObject GamePlayPanel;
        [SerializeField] public GameObject LevelCompletePanel;
        [SerializeField]  internal GameObject SettingsPanel;
        [SerializeField] internal GameObject BackgroundChangePanel;



    }

    public UI UserInterface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
       
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }
}
