using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnLoad : MonoBehaviour
{
    public List<GameObject> keep;

    private void Start()
    {
        foreach (var obj in this.keep)
        {
            DontDestroyOnLoad(obj);
        }
        SceneManager.LoadScene("MainMenu");
    }

}
