using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public float SceneLoadingTime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadScene), SceneLoadingTime);
    }

    void LoadScene()
    {
     
        SceneManager.LoadSceneAsync(1);
    }
}
