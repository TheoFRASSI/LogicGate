using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterrupteurScript : MonoBehaviour {

    /// <summary> Cable de sortie par lequel le signal de l'interrupteur est transmis </summary>
    public CableScript cableSortie;

    /// <summary> Sprite du cable actif </summary>
    public Sprite spriteOn;
    /// <summary> Sprite du cable inactif </summary>
    public Sprite spriteOff;

    /// <summary> L'etat de l'interrupteur, true lorsque le courant est produit, False par defaut </summary>
    public bool actif = false;

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

    /// <summary> Setter de l'etat actif d'un interrupteur </summary>
    public void SetActif(bool x)
    {
        this.actif = x;
    }

    /// <summary> Raffraichi l'etat du cable de sortie de l'interrupteur </summary>
    public void Refresh()
    {
        this.cableSortie.SetActif(this.actif);
    }

    /// <summary> Getter de l'etat actif d'un interrupteur </summary>
    public bool IsActif()
    {
        return this.actif;
    }
}
