using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUI : MonoBehaviour {

    ControllerNivel c;

    public Text punct;
    public Text mult;
    public Image board;

    private Animator border;

    public void Start()
    {
        c = FindObjectOfType<ControllerNivel>();
        border = board.GetComponent<Animator>();

        //Iniacializamos la UI.
        punct.text = "0 pt";
        mult.text = "x1";
    }


    public void actualPunctuation(int punctuation)
    {
        punct.text = punctuation.ToString() + " pt";
    }

    public void actualmultiplicator(int multiplicator)
    {
        mult.text = "x" + multiplicator.ToString();

        border.SetInteger("mult", multiplicator);
        border.SetTrigger("activar");    

    }

}
