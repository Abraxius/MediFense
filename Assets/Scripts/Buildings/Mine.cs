using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private Datenspeicherung dataStorage;
    private PlayerController movement;
    public GameObject minenCanvas;
    private bool abbauen;

    public TriggerPoint TriggerPoint;

    private float nextActionTime;
    public float period;

    public GameObject aktiverMinenAbbau;
    // Start is called before the first frame update
    void Start()
    {
        movement = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerController>();
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();

        nextActionTime = 0.0f;
        period = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        if (dataStorage.pauseButton == false)
        {
            if (abbauen == true)
            {
                dataStorage.money = dataStorage.money + dataStorage.abbauRate * Time.deltaTime;

                if (Time.time > nextActionTime)
                {
                    nextActionTime += period;
                    FindObjectOfType<AudioManager>().Play("MoneyFX");
                }
            }
        }
    }

    public void AbbrechenButton()
    {
        abbauen = false;
        movement.enabled = true;
        TriggerPoint.erstKontakt = false;
        minenCanvas.SetActive(false);
        aktiverMinenAbbau.SetActive(false);
    }

    public void AbbauenButton()
    {
        abbauen = true;
        minenCanvas.SetActive(false);
        aktiverMinenAbbau.SetActive(true);
    }
}
