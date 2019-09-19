using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    private float timer;
    private Datenspeicherung dataStorage;

    //Heal Zauber!

    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < 0.4 && timer > 0.3)
        {
            if (dataStorage.playerHp < dataStorage.playerHpMax)
            {
                dataStorage.playerHp += 50;
            }

            if (dataStorage.playerHp > dataStorage.playerHpMax)   //Falls du dich "überheilst"
            {
                dataStorage.playerHp = dataStorage.playerHpMax;
            }

        }
        if (timer > 1.4)
        {
            Destroy(this.gameObject);
        }
    }
}
