using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuild : MonoBehaviour
{
    [SerializeField]
    private GameObject[] TowerPrefabs;
    public GameObject Sperrzone;
    private int currentindex;
    private GameObject currentPlaceableObject;
    private GameObject aktuellesRechteck;
    private float currentcosts;

    public LayerMask ignoreLayer;

    public float rotatespeed;
    public bool buildable;

    private Datenspeicherung dataStorage;

    //Visualisierung
    public GameObject rauch;
    private PlayerController movement;

    //baureihenfolge
    public List<GameObject> TowerBaureihenfolge = new List<GameObject>();
    public bool bautGerade;
    private int interneTurmNr;

    //Warnhinweis
    public GameObject geldMangelPanel;
    private bool geldMangel;
    private float timer;

    void Start()
    {
        interneTurmNr = 1;
        buildable = true;
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        movement = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerController>();
    }

    void Update()
    {
        for (int i = 0; i < TowerPrefabs.Length; i++) // Auswahl der Tower
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 +1 + i)) //es wird zB 1 gedrückt
            {
                TurmBauen(i); 
                break;
            }
        }

        //Geldmangel Hinweis
        if (geldMangel)
        {
            geldMangelPanel.SetActive(true);
            timer += Time.deltaTime;
            if(timer > 1.5)
            {
                geldMangel = false;
                geldMangelPanel.SetActive(false);
                timer = 0;
            }
        }

        if (currentPlaceableObject != null) { // Abfragen bei ausgewählten noch nicht platziertem Tower
            currentPlaceableObject.transform.GetChild(2).gameObject.SetActive(true);
            if (buildable) //Farbliche Bestätigung
            {
                currentPlaceableObject.transform.GetChild(2).transform.GetComponent<SpriteRenderer>().color = Color.green;
            } else
            {
                currentPlaceableObject.transform.GetChild(2).transform.GetComponent<SpriteRenderer>().color = Color.red;
            }
            moveCurrentObjectToMouse();
            RotateTower();
            ReleaseIfClicked();
        }

        BaureihenfolgeAblaufen();
    }


    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentindex == i && currentPlaceableObject != null;
    }

    private void ReleaseIfClicked()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //currentPlaceableObject.GetComponent<Collider>().enabled = true; // für collider abfrage kurz an, falls nicht platzierbar wieder ausstellen
            switch (currentindex)
            {
                case 0:
                    currentcosts = dataStorage.towerCosts1;
                    break;
                case 1:
                    currentcosts = dataStorage.towerCosts2;
                    break;
                default:
                    Debug.Log("Turm Bau Error");
                    break;
            }

            // Switchcase für verschiedene Türme mit verschidenen Skripten
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //wo haben wir hingeklickt
            RaycastHit hit; //was haben wir angeklickt

            if (buildable)
            {
                 if (GameObject.Find("Player").GetComponent<Datenspeicherung>().money >= currentcosts) // genügend Geld ?
                    {
                    // Platzieren
                    Debug.Log("bauen");
                    FindObjectOfType<AudioManager>().Play("BuildingFX");

                    TowerBaureihenfolge.Add(currentPlaceableObject); //Speichert den Bauplatz in die Reihenfolge

                    GameObject.Find("Player").GetComponent<Datenspeicherung>().money = GameObject.Find("Player").GetComponent<Datenspeicherung>().money - currentcosts;

                    currentPlaceableObject.transform.GetChild(2).gameObject.SetActive(false);
                    currentPlaceableObject.transform.GetChild(3).gameObject.SetActive(true);

                    switch (currentindex)
                    {
                        case 0:
                            currentPlaceableObject.GetComponent<Tower>().platziert = true;
                            break;
                        case 1:
                            currentPlaceableObject.GetComponent<HydraTower>().platziert = true;
                            break;
                        default:
                            break;
                    }

                    currentPlaceableObject = null;
                    aktuellesRechteck = null;

                    // hier später switchcase wenn mehrer Türme mit verschiedenen Skripten

                    movement.enabled = false;
                    //Destroy(aktuellesRechteck);
                }
                    else
                    {
                        geldMangel = true;
                        timer = 0;
                        Debug.Log("nicht genügend Geld");
                    }
            } else
            {
                Debug.Log("Du darfst hier nicht platzieren");
            }

            /*if(currentPlaceableObject != null) {
                currentPlaceableObject.GetComponent<Collider>().enabled = true;
            }*/
        }

        if (Input.GetMouseButtonDown(1)) //Bei rechtsklick bauen abbrechen
        {
            Destroy(currentPlaceableObject);    //Nein? dann zerstör es
            Destroy(aktuellesRechteck);
            currentindex = -1;
        }
    }

    private void RotateTower() // Rotation
    {

        if (Input.GetKey(KeyCode.E))
            currentPlaceableObject.transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q))
            currentPlaceableObject.transform.Rotate(-Vector3.up * rotatespeed * Time.deltaTime);
    }

    private void moveCurrentObjectToMouse() // Vorschau immer an aktueller Stelle
    {
        //currentPlaceableObject.GetComponent<Collider>().enabled = false; //während platzieren false, damit nicht übereinander raycastet wird

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitinfo;
        if(Physics.Raycast(ray, out hitinfo, ignoreLayer)) // ignoreLayer für raycast, damit nicht an collider hängenbleiben
        {
            currentPlaceableObject.transform.position = hitinfo.point;
            //currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);  // falls Turm senkrecht auf ebene sein soll, rotation funktioniert aber noch nicht damit
            aktuellesRechteck.transform.position = hitinfo.point;
        }
    }

    private void BaureihenfolgeAblaufen()
    {
        if (TowerBaureihenfolge.Count > 0 && bautGerade == false)
        {
            GameObject tmp;
            tmp = TowerBaureihenfolge.Find(x => x.gameObject);
            movement.agent.SetDestination(tmp.transform.position);
            movement.enabled = false;
            bautGerade = true;
        }
    }

    public void TurmBauen(int i)
    {
        if (PressedKeyOfCurrentPrefab(i)) //ist der tastenauswahl auch ein Turm zugewiesen?
        {
            Destroy(currentPlaceableObject);    //Nein? dann zerstör es
            Destroy(aktuellesRechteck);
            currentindex = -1;
        }
        else
        {
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
                Destroy(aktuellesRechteck);
            }

            currentPlaceableObject = Instantiate(TowerPrefabs[i]);
            aktuellesRechteck = Instantiate(Sperrzone); //Test
            currentindex = i;
        }
    }
}
