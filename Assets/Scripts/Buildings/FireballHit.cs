using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hat nur die Funktion die Explosion des Feuerballs nach erfüllung wieder zu löschen
public class FireballHit : MonoBehaviour
{
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {
            Destroy(this.gameObject);
        }
    }
}
