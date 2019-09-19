using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPoint : MonoBehaviour
{
    public GameObject buildingCanvas;
    public GameObject mainCanvas;

    private Datenspeicherung dataStorage;
    private PlayerController movement;
    public bool erstKontakt;

    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerController>();
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        erstKontakt = true;
    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.name == "character")
        {
            if (erstKontakt)
            {
                buildingCanvas.SetActive(true);
                Debug.Log("Im Bereich");
                movement.enabled = false;
            }

        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.name == "character")
        {
            movement.enabled = true;
            buildingCanvas.SetActive(false);
            erstKontakt = true;
        }
    }
}
