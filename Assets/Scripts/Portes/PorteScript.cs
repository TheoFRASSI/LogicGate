using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PorteScript : MonoBehaviour {

    private bool entree1;
    private bool entree2;
    private bool sortie;

    /// <summary> Les sprites du compteur de porte </summary>
    public Sprite spriteOn;
    public Sprite spriteOff;

    private void Start()
    {
        this.entree1 = false;
        this.entree2 = false;
        this.sortie = false;
    }

    /// <summary> Met le sprite sur la porte si les conditions sont réunies </summary>
    public void ChangeSprite()
    {
        if (this.transform.parent != null)
        {
            if (this.transform.parent.GetComponent<SocleScript>() != null)
            {
                if (this.transform.parent.GetComponent<SocleScript>().cableSortie.IsActif())
                {
                    this.GetComponent<Image>().sprite = spriteOn;
                }
                else
                {
                    this.GetComponent<Image>().sprite = spriteOff;
                }
            }
            else
            {
                this.GetComponent<Image>().sprite = spriteOff;
            }
        } else
        {
            this.GetComponent<Image>().sprite = spriteOff;
        }
    }

    public void SetEntree1(bool x)
    {
        this.entree1 = x;
    }

    public void SetEntree2(bool x)
    {
        this.entree2 = x;
    }

    public void SetSortie(bool x)
    {
        this.sortie = x;
    }

    public bool IsEntree1()
    {
        return this.entree1;
    }

    public bool IsEntree2()
    {
        return this.entree2;
    }

    public bool IsSortie()
    {
        return this.sortie;
    }

}