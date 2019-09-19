using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAnimation : MonoBehaviour
{
    public float timer;
    private Datenspeicherung dataStorage;
    public ParticleSystem particle;

    public bool abspielen;
    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (abspielen)
        {
            timer += Time.deltaTime;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            if (timer > 2)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                particle.Play(false);
                abspielen = false;
                timer = 0;
            }
        }

    }
}
