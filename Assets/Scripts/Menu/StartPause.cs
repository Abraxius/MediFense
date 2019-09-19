using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPause : MonoBehaviour
{
    public GameObject PlayerController;
    public GameObject PlayerInput;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        PlayerController.GetComponent<PlayerController>().enabled = false;
        PlayerInput.GetComponent<UserInput>().enabled = false;

    }

    public void LosGehts()
    {
        Time.timeScale = 1f;
        PlayerController.GetComponent<PlayerController>().enabled = true;
        PlayerInput.GetComponent<UserInput>().enabled = true;
    }
}
