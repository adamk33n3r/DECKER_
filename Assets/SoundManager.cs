using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip clickSound;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        this.source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.source.PlayOneShot(this.clickSound);
        }
    }
}
