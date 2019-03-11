using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Gestion des clics sur les interrupteurs </summary>
public class InterrupteurMouseController : MonoBehaviour {

    public void clic()
    {
        // Au clic on inverse son etat
        if (this.GetComponent<InterrupteurScript>().IsActif())
        {
            this.GetComponent<InterrupteurScript>().SetActif(false);
        }
        else
        {
            this.GetComponent<InterrupteurScript>().SetActif(true);
        }

        // Et on change son sprite
        this.GetComponent<InterrupteurScript>().ChangeSprite();
        this.GetComponent<InterrupteurScript>().Refresh();
    }
}
