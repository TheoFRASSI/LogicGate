using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CableScript : MonoBehaviour {

    /// <summary> Sprite du cable actif </summary>
    public Sprite spriteOn;
    /// <summary> Sprite du cable inactif </summary>
    public Sprite spriteOff;
    /// <summary> Optionel : Permet de savoir quel cable precede celui ci lorsqu'il y en a un </summary>
    public CableScript previousCable;


    /// <summary> L'etat du cable, true lorsque le courant passe </summary>
    private bool actif;


    /// <summary> Permet au cable de changer de sprite selon son etat </summary>
    public void ChangeSprite()
    {
        if (this.IsActif())
        {
            this.GetComponent<Image>().sprite = spriteOn;
        }
        else
        {
            this.GetComponent<Image>().sprite = spriteOff;
        }
    }

    /// <summary> Getter de l'etat actif du cable </summary>
    public bool IsActif()
    {
        return this.actif;
    }

    /// <summary> Setter de l'etat actif du cable </summary>
    public void SetActif(bool x)
    {
        this.actif = x;
    }
}
