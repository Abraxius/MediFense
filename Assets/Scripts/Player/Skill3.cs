using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : MonoBehaviour
{
    private float timer;
    private Datenspeicherung dataStorage;
    private GameObject player;

    //Teleport Zauber!

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject.transform.GetChild(1).gameObject;
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.4)
        {
            player.GetComponent<PlayerController>().agent.Warp(dataStorage.spawnPoint);
            //Camera.main.transform.position = new Vector3(30,30,70);       erstmal auskommentiert? besser mit kamera port oder ohne???
            //Camera.main.transform.rotation = Quaternion.Euler(new Vector3(20f, 78f, 0f));
        }

        if (timer > 3)
        {
            Destroy(this.gameObject);
        }
    }
}
