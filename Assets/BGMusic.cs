using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public static BGMusic Instance;

    private AudioSource source;

    private void Start()
    {
        Instance = this;
        this.source = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void SetVolume(float vol)
    {
        this.source.volume = vol;
    }
}
