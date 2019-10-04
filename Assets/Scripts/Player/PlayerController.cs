using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{

    private Camera cam;

    public NavMeshAgent agent;

    private Datenspeicherung dataStorage;
    private Animator animator;

    private float beatTime;  //Die Zeit zwischen denen beim Schlagen des Enemys pause ist
    private int damage;       //Der Schaden den der Enemy verursacht

    public float countdown;

    public GameObject mainBaseTriggerPoint;
    public GameObject mineTriggerPoint;
    public GameObject shopTriggerPoint;

    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;

    private int damageSkill;
    private int healSkill;
    public int portSkill;

    float actionCooldown1;
    float timeSinceAction1;

    float actionCooldown2;
    float timeSinceAction2;

    float actionCooldown3;
    float timeSinceAction3;

    public GameObject CooldownPanelS1;
    public GameObject CooldownPanelS2;
    public GameObject CooldownPanelS3;

    private bool SoundUsed;

    public GameObject clickAnimation;

    //Um sicherzustellen, dass er nicht läuft wenn man das UI anklickt
    public GraphicRaycaster m_Raycaster;
    public PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    private void Start()
    {
        dataStorage = this.gameObject.transform.parent.GetComponent<Datenspeicherung>();
        cam = Camera.main;

        beatTime = dataStorage.beatTimePlayer;
        damage = dataStorage.damagePlayer;

        animator = GetComponent<Animator>();

        SoundUsed = false;

        actionCooldown1 = 15f;
        actionCooldown2 = 120f;
        actionCooldown3 = 60f;

        timeSinceAction1 = 15f;
        timeSinceAction2 = 120f;
        timeSinceAction3 = 60f;
    }

    void Update()
    {
        //Skills------------------------------------------------------------
        // Cooldowns
        timeSinceAction1 += Time.deltaTime;
        timeSinceAction2 += Time.deltaTime;
        timeSinceAction3 += Time.deltaTime;


        damageSkill = PlayerPrefs.GetInt("damageSkill", damageSkill);
        healSkill = PlayerPrefs.GetInt("healSkill", healSkill);
        portSkill = PlayerPrefs.GetInt("portSkill", portSkill);
        // Cooldown Panel
        if (damageSkill == 1) // wenn gekauft
        {
            if (timeSinceAction1 < actionCooldown1)
            {
                CooldownPanelS1.SetActive(true);
            }
            else CooldownPanelS1.SetActive(false);
        }

        if (healSkill == 1) { 
            if (timeSinceAction2 < actionCooldown2)
            {
                CooldownPanelS2.SetActive(true);
            }
            else CooldownPanelS2.SetActive(false);
        }

        if(portSkill == 1) { 
            if (timeSinceAction3 < actionCooldown3)
            {
                CooldownPanelS3.SetActive(true);
            }
            else CooldownPanelS3.SetActive(false);
        }
            //-Steuerung----------------------------------------------------------------------
            if (dataStorage.pauseButton == false)
        {
            agent.enabled = true;

            if (Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);    //wo haben wir hingeklickt
                RaycastHit hit; //was haben wir angeklickt
                
                if (dataStorage.playerDied == false)
                {
                    //Set up the new Pointer Event
                    m_PointerEventData = new PointerEventData(m_EventSystem);
                    //Set the Pointer Event Position to that of the mouse position
                    m_PointerEventData.position = Input.mousePosition;

                    //Create a list of Raycast Results
                    List<RaycastResult> results = new List<RaycastResult>();

                    //Raycast using the Graphics Raycaster and mouse click position
                    m_Raycaster.Raycast(m_PointerEventData, results);

                    results.Remove(results.Find(x => x.gameObject.name == "GamePanel")); //Löscht alle GamePanel treffer aus der Liste
                    if (results.Count < 1) //Guckt ob etwas vom UI angeklickt wurde (Außer das GamePanel) Wenn Ja, darf er laufen, wenn Nein, darf er nicht
                    {
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.layer == 5)
                            {
                                Debug.Log("UI angeklickt");
                            }
                            if (hit.transform.gameObject.tag == "MainBase")
                            {
                                //Debug.Log("Haupthaus angeklickt");
                                agent.SetDestination(mainBaseTriggerPoint.transform.position);
                            }
                            else if (hit.transform.gameObject.tag == "Mine")
                            {
                                agent.SetDestination(mineTriggerPoint.transform.position);
                            }
                            else if (hit.transform.gameObject.tag == "Shop")
                            {
                                agent.SetDestination(shopTriggerPoint.transform.position);
                            }
                            else
                            {
                                //Lässt den Character laufen und startet die Klickanimation
                                clickAnimation.transform.position = hit.point;
                                clickAnimation.GetComponent<ClickAnimation>().abspielen = true;
                                clickAnimation.GetComponent<ClickAnimation>().timer = 0;
                                clickAnimation.GetComponent<ClickAnimation>().particle.Play(true);
                                agent.SetDestination(hit.point);
                            }
                        }
                    }
                }
            }

            died(); //Sterbe Abfrage

            //Die Fähigkeiten des Charakters
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DamageSkill();
            }

            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                HealSkill();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PortSkill();
            }
        } else
        {
            agent.enabled = false;
        }
        //-------------------------------------------------------------------------------
    }
    private void OnTriggerStay(Collider collisionInfo)      //Baustelle
    {
         if (collisionInfo.tag == "Enemy")
         {
            if (dataStorage.pauseButton == false)
            {
                //animator.SetBool("isAttacking", true);
                countdown = countdown + 1 * Time.deltaTime;
                if (countdown > beatTime)
                {
                    bool tmp = false;
                    animator.SetInteger("attackNr", Random.Range(0, 2));
                    animator.SetTrigger("isAttackingT");
                    //Debug.Log("Stirb Enemy");
                    //collisionInfo.transform.position += Vector3.up;    //an dieser Stelle die Schadens animation des Helden einfügen
                    try
                    {
                    collisionInfo.gameObject.GetComponent<Enemy>().enemyHp -= damage;
                    } catch
                    {
                        tmp = true;
                    }

                    if (tmp == true)
                    {
                        collisionInfo.gameObject.GetComponent<Enemy_Golem>().enemyHp -= damage;
                    }

                    FindObjectOfType<AudioManager>().Play("MonsterHurtFX");

                    //animator.SetBool("isAttacking", false);
                    countdown = 0;
                }
            }
        }
    }

    private void died()     //Methode fürs sterben des Helden
    {
        if (dataStorage.playerHp < 1)
        {
            animator.SetBool("death", true);

            if(SoundUsed == false)
            {
                FindObjectOfType<AudioManager>().Play("HeroDeathFX");
                SoundUsed = true;
            }
            

            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Death"))   //wurde die animation abgespielt?
            {
                dataStorage.playerDied = true;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
                if (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2)   //wurde die animation abgespielt?
                {
                    this.gameObject.transform.position = dataStorage.spawnPoint;
                    dataStorage.playerDied = true;
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    animator.SetBool("death", false);
                    this.gameObject.SetActive(false);

                    SoundUsed = false;
                }
            }
        }
    }

    public void DamageSkill()
    {
        if(timeSinceAction1 > actionCooldown1 && PlayerPrefs.GetInt("damageSkill") == 1)
            {
            agent.SetDestination(this.transform.position);
            animator.SetTrigger("SmashTime");
            Instantiate(Skill1, this.transform.position, Quaternion.identity);

            timeSinceAction1 = 0f;
        }
    }

    public void HealSkill()
    {
        if (timeSinceAction2 > actionCooldown2 && PlayerPrefs.GetInt("healSkill") == 1)
        {
            agent.SetDestination(this.transform.position);
            animator.SetTrigger("SmashTime");
            Instantiate(Skill2, this.transform.position, Quaternion.identity);

            timeSinceAction2 = 0f;
        }
    }

    public void PortSkill()
    {
        if (timeSinceAction3 > actionCooldown3 && PlayerPrefs.GetInt("portSkill") == 1)
        {
            agent.SetDestination(this.transform.position);
            animator.SetTrigger("SmashTime");
            Instantiate(Skill3, this.transform.position, Quaternion.identity);

            timeSinceAction3 = 0f;
        }
    }
}
