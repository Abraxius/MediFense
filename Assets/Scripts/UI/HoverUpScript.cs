using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverUpScript : MonoBehaviour
{
    private bool hoverBool;
    // Start is called before the first frame update
    void Start()
    {
        hoverBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        HoverAbfrage();
    }

    private void HoverAbfrage()
    {
        Ray rayHover = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hover;
        if (Physics.Raycast(rayHover, out hover))
        {
            if (hover.transform.gameObject.tag == "MainBase" || hover.transform.gameObject.tag == "Mine" || hover.transform.gameObject.tag == "Shop")
            {
                transform.position = hover.point;
                transform.rotation = Camera.main.transform.rotation;
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
