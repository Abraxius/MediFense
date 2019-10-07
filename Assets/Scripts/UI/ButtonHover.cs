using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    private GameObject fensterSpeicher;
    private Text nameFeld;
    private Text infotextFeld;
    public string infotext;
    public string towerName;
    public int towerNr;

    private Datenspeicherung dataStorage;
    private GameObject gamePanel;
    // Update is called once per frame
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        gamePanel = GameObject.Find("GamePanel");

        //ordnet die Textfelder zu
        nameFeld = panel.transform.GetChild(0).GetComponent<Text>();
        infotextFeld = panel.transform.GetChild(1).GetComponent<Text>();
    }

    void Update()
    {
        Debug.Log(Input.mousePosition);
        if (fensterSpeicher != null)
        {
            fensterSpeicher.transform.position = Input.mousePosition + new Vector3(90, 55, 0);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (towerNr)
        {
            case 1:
                infotext = "Schaden: " + dataStorage.GetComponent<Datenspeicherung>().towerDamage1 + "\nTempo: " + dataStorage.GetComponent<Datenspeicherung>().towerAttackSpeed1 + "s \nBauzeit: " + dataStorage.GetComponent<Datenspeicherung>().tower1Bauzeit + "s \nReichweite: " + dataStorage.GetComponent<Datenspeicherung>().towerRange1 + "m";
                break;
            case 2:
                infotext = "Schaden: " + dataStorage.GetComponent<Datenspeicherung>().towerDamage2 + "\nTempo: " + dataStorage.GetComponent<Datenspeicherung>().towerAttackSpeed2 + "s \nBauzeit: " + dataStorage.GetComponent<Datenspeicherung>().tower2Bauzeit + "s \nReichweite: " + dataStorage.GetComponent<Datenspeicherung>().towerRange2 + "m";
                break;
            default:
                //Bei keiner Zahl Angabe wird der im GameObject selbst angegebene Text eingetragen und nicht verändert
                break;
        }

        infotextFeld.text = infotext;
        nameFeld.text = towerName;

        //Erstellt das Panel und 
        fensterSpeicher = Instantiate(panel, Input.mousePosition + new Vector3(50,50,0), Quaternion.identity);
        fensterSpeicher.transform.parent = gamePanel.gameObject.transform;


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(fensterSpeicher);
    }
}
