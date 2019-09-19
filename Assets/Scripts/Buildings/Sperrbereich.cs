using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sperrbereich : MonoBehaviour
{
    private TowerBuild TowerBuild;

    // Start is called before the first frame update
    void Start()
    {
        TowerBuild = GameObject.Find("TowerPlacementController").GetComponent<TowerBuild>();
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.name == "Enemy Path" || collisionInfo.name == "Fluss" || collisionInfo.tag == "MainBase" || collisionInfo.tag == "Environment" || collisionInfo.tag == "Shop" || collisionInfo.tag == "Mine" || collisionInfo.tag == "Building")
        {
            //Debug.Log("Sperrbereich");
            TowerBuild.buildable = false;
        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.name == "Enemy Path" || collisionInfo.name == "Fluss" || collisionInfo.tag == "Environment" || collisionInfo.tag == "Shop" || collisionInfo.tag == "Mine" || collisionInfo.tag == "MainBase" || collisionInfo.tag == "Building")
        {
            TowerBuild.buildable = true;
        }
    }

}
