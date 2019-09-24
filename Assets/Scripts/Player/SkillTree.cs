using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillTree : MonoBehaviour
{
    private Datenspeicherung dataStorage;

    //Helden Skillung
    private bool spielerDamageX2Bool = false;
    private bool spielerHpX2Bool = false;
    private bool towerDamageX2Bool = false;

    private bool damageSkillBool = false;
    private bool healSkillBool = false;
    private bool portSkillBool = false;
    
    //neue Variablen AKTUELL
    private int spielerHpPlus;
    private int spielerDamagePlus;
    private int towerDamagePlus;

    private int spielerDamageX2 = 1;
    private int spielerHpX2 = 1;
    private int towerDamageX2 = 1;

    private int damageSkill;
    private int healSkill;
    private int portSkill;

    public Button spielerDamageX2Button;
    public Button spielerHpX2Button;
    public Button towerDamageX2Button;

    public Button damageSkillButton;
    public Button healSkillButton;
    public Button portSkillButton;
    // Start is called before the first frame update
    void Start()
    {
        spielerDamageX2Bool = false;
        spielerHpX2Bool = false;
        towerDamageX2Bool = false;

        damageSkillBool = false;
        healSkillBool = false;
        portSkillBool = false;

        try
        {
            dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        } catch
        {
            Debug.Log("Konnte die Datenspeicherung nicht finden");
        }


        //Variablen Beschaffung
        spielerHpPlus = PlayerPrefs.GetInt("spielerHpPlusSave", spielerHpPlus);
        spielerDamagePlus = PlayerPrefs.GetInt("spielerDamagePlusSave", spielerHpPlus);
        towerDamagePlus = PlayerPrefs.GetInt("towerDamagePlusSave", spielerHpPlus);

        spielerDamageX2 = PlayerPrefs.GetInt("spielerDamageX2", spielerDamageX2);
        spielerHpX2 = PlayerPrefs.GetInt("spielerHpX2", spielerHpX2);
        towerDamageX2 = PlayerPrefs.GetInt("towerDamageX2", towerDamageX2);

        damageSkill = PlayerPrefs.GetInt("damageSkill", damageSkill);
        healSkill = PlayerPrefs.GetInt("healSkill", healSkill);
        portSkill = PlayerPrefs.GetInt("portSkill", portSkill);

        if (spielerDamageX2 == 2)
        {
            spielerDamageX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            spielerDamageX2Bool = true;
        } else
        {
            PlayerPrefs.SetInt("spielerDamageX2", 1);
        }
        if (spielerHpX2 == 2)
        {
            spielerHpX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            spielerHpX2Bool = true;
        } else
        {
            PlayerPrefs.SetInt("spielerHpX2", 1);
        }
        if (towerDamageX2 == 2)
        {
            towerDamageX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            towerDamageX2Bool = true;
        } else
        {
            PlayerPrefs.SetInt("towerDamageX2", 1);
        }

        if (damageSkill == 1)
        {
            damageSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            damageSkillBool = true;
        }
        if (healSkill == 1)
        {
            healSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            healSkillBool = true;
        }
        if (portSkill == 1)
        {
            portSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            portSkillBool = true;
        }
    }

    //Einmalig Kaufbare Skills
    public void SpielerDamageX2()
    {
        if (dataStorage.gen >= dataStorage.spielerDamageX2Kosten && spielerDamageX2Bool == false)
        {
            spielerDamageX2 = 2;
            spielerDamageX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("spielerDamageX2", spielerDamageX2);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.spielerDamageX2Kosten;
            spielerDamageX2Bool = true;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    public void SpielerHpX2()
    {
        if (dataStorage.gen >= dataStorage.spielerHpX2Kosten && spielerHpX2Bool == false)
        {
            spielerHpX2 = 2;
            spielerHpX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("spielerHpX2", spielerHpX2);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.spielerHpX2Kosten;
            spielerHpX2Bool = true;
            //Damit die HP nach Upgraden gleich auf Max ist
            dataStorage.playerHpMax = dataStorage.playerHpStart + PlayerPrefs.GetInt("spielerHpPlusSave") * PlayerPrefs.GetInt("spielerHpX2");
            dataStorage.playerHp = dataStorage.playerHpMax;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    public void TowerDamageX2()
    {
        if (dataStorage.gen >= dataStorage.towerDamageX2Kosten && towerDamageX2Bool == false)
        {
            towerDamageX2 = 2;
            towerDamageX2Button.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("spielerHpX2", towerDamageX2);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.towerDamageX2Kosten;
            towerDamageX2Bool = true;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    //Mehrmals kaufbare Skills
    public void SpielerHpPlus()
    {
            if (dataStorage.gen >= dataStorage.spielerHpPlusKosten)
            {
            spielerHpPlus += 20;
            PlayerPrefs.SetInt("spielerHpPlusSave", spielerHpPlus);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.spielerHpPlusKosten;
            //Damit die HP nach Upgraden gleich auf Max ist
            dataStorage.playerHpMax = dataStorage.playerHpStart + PlayerPrefs.GetInt("spielerHpPlusSave") * PlayerPrefs.GetInt("spielerHpX2");
            dataStorage.playerHp = dataStorage.playerHpMax;
            }
            else
            {
                Debug.Log("Zu wenig Erze");
            }
    }
    public void SpielerDamagePlus()
    {
        if (dataStorage.gen >= dataStorage.spielerDamagePlusKosten)
        {
            spielerDamagePlus += 10;
            PlayerPrefs.SetInt("spielerDamagePlusSave", spielerDamagePlus);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.spielerDamagePlusKosten;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    public void TowerDamagePlus()
    {
        if (dataStorage.gen >= dataStorage.towerDamagePlusKosten)
        {
            towerDamagePlus += 10;
            PlayerPrefs.SetInt("towerDamagePlusSave", towerDamagePlus);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.towerDamagePlusKosten;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    //Fähigkeiten zum kaufen
    public void FähigkeitAoE()
    {
        if (dataStorage.gen >= dataStorage.fähigkeitKostenAoE && damageSkillBool == false)
        {
            damageSkill = 1;
            damageSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("damageSkill", damageSkill);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.fähigkeitKostenAoE;
            damageSkillBool = true;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    public void FähigkeitHeal()
    {
        if (dataStorage.gen >= dataStorage.fähigkeitKostenHeal && healSkillBool == false)
        {
            healSkill = 1;
            healSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("healSkill", healSkill);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.fähigkeitKostenHeal;
            healSkillBool = true;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
    public void FähigkeitPort()
    {
        if (dataStorage.gen >= dataStorage.fähigkeitKostenPort && portSkillBool == false)
        {
            portSkill = 1;
            portSkillButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            PlayerPrefs.SetInt("portSkill", portSkill);
            Debug.Log("Wurde erhöt");
            dataStorage.gen -= dataStorage.fähigkeitKostenPort;
            portSkillBool = true;
        }
        else
        {
            Debug.Log("Zu wenig Erze");
        }
    }
}
