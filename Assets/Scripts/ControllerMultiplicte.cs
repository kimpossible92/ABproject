using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMultiplicte : MonoBehaviour {


    public Text mult;
    //[SerializeField]ControllerNivel controller;

    public void reiniciarMultiplicador()
    {
        transform.GetComponent<Animator>().SetInteger("mult", 1);
        mult.text = "x1";
        FindObjectOfType<ControllerNivel>().multiplictionPuntuation = 1;
    }
}
