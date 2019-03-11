using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocleScript : MonoBehaviour {

    /// <summary> Le cable d'entrée gauche </summary>
    public CableScript cableGauche;
    /// <summary> Le cable d'entrée droit </summary>
    public CableScript cableDroite;
    /// <summary> Le cable de sortie </summary>
    public CableScript cableSortie;

    /// <summary> Renvoie vraie si la porte est présente, faux sinon </summary>
    public bool IsPortePresente()
    {
        return this.gameObject.transform.childCount == 0 ? false : true;
    }

}
