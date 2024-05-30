using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private void OnEnable()
    {
        if (GetComponent<Image>().enabled == true)
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
