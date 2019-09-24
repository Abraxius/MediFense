using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Legt einige Standard Werte für die anderen Codes fest, diese können verwendet werden, wenn "using RTS;" importiert wird
namespace RTS
{
    public static class RessourceManager
    {
        //Steuerungswerte
        public static float ScrollSpeed { get { return 50.0f; } }       //Scroll Speed
        public static float RotateSpeed { get { return 100.0f; } }      //Rotations Speed
        public static float RotateAmount { get { return 10.0f; } }      //Drehstärke

        public static int ScrollWidth { get { return 15; } }            //Scrollbereich am Rand            

        public static float MinCameraHeight { get { return 5.0f; } }    //min Kamera Höhe
        public static float MaxCameraHeight { get { return 50.0f; } }   //max Kamera Höhe

        public static float MinCameraWidth { get { return -80.0f; } }
        public static float MaxCameraWidth { get { return 120.0f; } }

        public static float MinCameraLength { get { return -90.0f; } }
        public static float MaxCameraLength { get { return 100.0f; } }

    }
}

