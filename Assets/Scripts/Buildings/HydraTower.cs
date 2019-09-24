using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraTower : MonoBehaviour
{
    public GameObject enemy;
    private int costs;

    //public GameObject towerBody; einfach nur durch "this" ersetzt
    private float timer;        //variablen in Datenspeicherung.cs verschoben um alles an einem Ort festzulegen
    private float attackSpeed;
    private float range;
    private int damage;
    public SphereCollider sc;
    public TowerBuild TowerBuilld;

    private Datenspeicherung dataStorage;

    public float rotationSmoothing;
    private GameObject towerGun;
    public Transform PartToRotate;// Test
    //-----------------------------Pfeil
    public GameObject Arrow;
    private GameObject[] ArrowList;
    //------------------------------------- Animation
    public Animator animator;
    private bool attack = false;
    public bool platziert = true;
    public bool closeToPlayer = true;

    private bool fertigGebaut;
    private PlayerController movement;
    private float timer2;

    void Start()
    {
        timer = 0.0f;
        //towerGun = gameObject.transform.GetChild(0).GetChild(0).gameObject;

        TowerBuilld = GameObject.Find("TowerPlacementController").GetComponent<TowerBuild>();
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        movement = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerController>();

        costs = dataStorage.towerCosts2;
        attackSpeed = dataStorage.towerAttackSpeed2;
        range = dataStorage.towerRange2;
        damage = dataStorage.towerDamage2;

        sc = this.gameObject.AddComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = range;
        sc.enabled = false;
    }

    void Update()
    {

        if (platziert == false) //Damit erst die Bauanimation bei frisch gebauten türmen ausgeführt werden muss, danach können Sie erst schießen
        {
            bool tmpScript = false;
            if (enemy == null)  //stopt animation wenn kein gegner da ist
            {
                animator.SetBool("Attack", false);
            }
            else
            {
                //Abfrage ob Einheit schon tot ist, um den letzten Leichen Schuss zu verhindern
                if (enemy.name == "Enemy 2(Clone)")
                {
                    tmpScript = enemy.GetComponent<Enemy_Golem>().sterben;
                }
                else if (enemy.name == "Enemy 1(Clone)")
                {
                    tmpScript = enemy.GetComponent<Enemy>().sterben;
                }
                else if (enemy.name == "Boss 1(Clone)")
                {
                    tmpScript = enemy.GetComponent<EnemyBoss>().sterben;
                }
            }

            if (tmpScript == true)
            {
                enemy = null;
            }

            timer += Time.deltaTime;

            if (timer >= attackSpeed && enemy != null)
            {
                FindObjectOfType<AudioManager>().Play("BallistaShootingFX");

                animator.SetBool("Attack", true);
                if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))   //wurde die animation abgespielt?
                {
                    //Erzeugt den Arrow als neues GameObjekt

                    GameObject[] tmp = new GameObject[3];
                    float a = -1.5f;
                    for (int i = 0; i < 3; i++)    //Spawnt 3 Feuerbälle statt einem
                    {
                        tmp[i] = Instantiate(Arrow, this.transform.position + new Vector3(a, 1.5f, 0), gameObject.transform.GetChild(0).GetChild(0).transform.rotation);
                        tmp[i].GetComponent<Arrow>().target = enemy;
                        tmp[i].transform.parent = this.gameObject.transform;
                        a += 1.5f;
                    }
                    timer = 0f;
                    animator.SetBool("Attack", false);
                }
            }

            if (enemy != null)
            {
                if (Vector3.Distance(this.transform.position, enemy.transform.position) >= range)
                {
                    enemy = null;
                }
                else
                {
                    //Hier findet das Bewegen und Angucken des Feindes statt, kleine Abänderungen noch nötig, da die falschen Achsen rotiert werden!
                    Quaternion targetRotation = Quaternion.LookRotation((enemy.transform.position - PartToRotate.position).normalized, Vector3.up); //Das ist der Code vom Nico, aber ich weiß nemmer wie der richtitg funktioniert hat.. :D
                    PartToRotate.rotation = Quaternion.Lerp(PartToRotate.rotation, targetRotation, rotationSmoothing);

                    /*Vector3 dir = enemy.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = lookRotation.eulerAngles;    //lookRotation wird in Vector3 geändert, damit wir sagen können, das er sich nur um die Y Achse drehen soll
                    PartToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                    */

                }
            }
        }
    }

    private void OnTriggerStay(Collider other)  //Schuss reichweite
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (enemy == null)
            {
                enemy = other.gameObject;
 
            }
        }

        if (other.gameObject.name == "character" && platziert == true)  //bauaktion des Spielers
        {
            timer2 += Time.deltaTime;
            if (timer2 > dataStorage.tower2Bauzeit)
            {
                transform.GetChild(3).gameObject.SetActive(false);
                sc.enabled = true;
                movement.enabled = true;
                platziert = false;
                TowerBuilld.bautGerade = false;
                TowerBuilld.TowerBaureihenfolge.Remove(this.gameObject);
            }

        }
    }
}
