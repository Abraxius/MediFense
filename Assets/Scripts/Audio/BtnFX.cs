using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnFX : MonoBehaviour
{

    public AudioSource myFX;
    public AudioClip clickfx;
    public AudioClip hoverfx;

    public void clicksound()
    {
        myFX.PlayOneShot(clickfx);
        DontDestroyOnLoad(this.gameObject); // beim Szenenwechsel wird nicht abgecuttet
    }

    public void hoverSound()
    {
        myFX.PlayOneShot(hoverfx);
    }
}
