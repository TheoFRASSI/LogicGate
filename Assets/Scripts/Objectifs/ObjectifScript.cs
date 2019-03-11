using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectifScript : MonoBehaviour {

    /// <summary> Les sprites du compteur de porte </summary>
    public Sprite spriteOn;
    public Sprite spriteOff;
    /// <summary> Le cable précédent du compteur de porte </summary>
    public CableScript previousCable;

    public bool isActif = false;

    /// <summary> Change le sprite en fonction du cable précédent et du sprite actif </summary>
    public void ChangeSprite()
    {
        if(previousCable != null)
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
        
    }

    /// <summary> Retourne vraie si l'objectif est actif, faux sinon </summary>
    public bool IsActif()
    {
        return isActif;
    }

    // Update is called once per frame
    private void Update()
    {
        if(previousCable != null)
        {
            isActif = previousCable.IsActif();
        } 
    }
}
