using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class Haupthaus : MonoBehaviour
{
    private Datenspeicherung dataStorage;
    private PlayerController movement;
    public GameObject minenCanvas;
    public GameObject menuPanel;
    public GameObject ausbauPanel;

    public TriggerPoint TriggerPoint;

    public GameObject ausbauStufe1;
    public GameObject ausbauStufe2;
    public GameObject ausbauStufe3;
    //Werte
    public int ausbaustufeHaupthaus;

    public GameObject ausbauText1;
    public GameObject ausbauText2;
    public GameObject ausbauText3;
    // Start is called before the first frame update
    void Start()
    {
        ausbaustufeHaupthaus = 0;
        movement = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerController>();
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();    
    }

    public void AbbrechenButton()
    {
        movement.enabled = true;
        TriggerPoint.erstKontakt = false;
        menuPanel.SetActive(true);
        ausbauPanel.SetActive(false);
        minenCanvas.SetActive(false);
    }

    public void AusbauButton()
    {
        menuPanel.SetActive(false);
        ausbauPanel.SetActive(true);
    }

    public void AusbauStartenButton()
    {
        switch(ausbaustufeHaupthaus)
        {
            case 0: 
                if (dataStorage.zweiteAusbaustufe < dataStorage.money)
                {
                    
                    dataStorage.money -= dataStorage.zweiteAusbaustufe;
                    ausbaustufeHaupthaus += 1;
                    ausbauText1.SetActive(false);
                    ausbauText2.SetActive(true);
                    //Bonuse die Verfügbar werden:
                    dataStorage.enemyBossLoot += 1;
                    dataStorage.abbauRate += 1;
                    dataStorage.rathausBonusProduktion += 0.02f;
                    //---------------------------------------
                    Debug.Log("Haupthaus wurde ausgebaut.");
                    ausbauStufe1.SetActive(false);
                    ausbauStufe2.SetActive(true);
                    //-----------------------------------------
                    movement.enabled = true;
                    TriggerPoint.erstKontakt = false;
                    menuPanel.SetActive(true);
                    ausbauPanel.SetActive(false);
                    minenCanvas.SetActive(false);
                } else
                {
                    Debug.Log("Nicht genügend Gold vorhanden.");
                }
                break;
            case 1:
                if (dataStorage.dritteAusbaustufe < dataStorage.money)
                {
                    dataStorage.money -= dataStorage.dritteAusbaustufe;
                    ausbaustufeHaupthaus += 1;
                    ausbauText2.SetActive(false);
                    ausbauText3.SetActive(true);
                    //Bonuse die Verfügbar werden:
                    dataStorage.enemyBossLoot += 2;
                    dataStorage.abbauRate += 2;
                    dataStorage.rathausBonusProduktion += 0.03f;
                    //---------------------------------------
                    Debug.Log("Haupthaus wurde ausgebaut.");
                    ausbauStufe2.SetActive(false);
                    ausbauStufe3.SetActive(true);
                    //---------------------------------------
                    movement.enabled = true;
                    TriggerPoint.erstKontakt = false;
                    menuPanel.SetActive(true);
                    ausbauPanel.SetActive(false);
                    minenCanvas.SetActive(false);
                }
                else
                {
                    Debug.Log("Nicht genügend Gold vorhanden.");
                }
                break;
            case 2:
                Debug.Log("Höchste Stufe erreicht.");
                break;
            default:
                break;
        }
    }
}
