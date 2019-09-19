using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private Datenspeicherung dataStorage;
    private PlayerController movement;
    public GameObject shopCanvas;
    public Collider col;
    private bool erstKontakt;

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
                shopCanvas.SetActive(true);
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
            shopCanvas.SetActive(false);
            erstKontakt = true;
        }
    }

    public void AbbrechenButton()
    {
        movement.enabled = true;
        erstKontakt = false;
        shopCanvas.SetActive(false);
    }
}
