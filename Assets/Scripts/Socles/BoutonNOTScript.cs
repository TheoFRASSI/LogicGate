using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonNOTScript : MonoBehaviour {

    /// <summary> Le socle </summary>
    public GameObject socle;

    /// <summary> Inverse la fonction de la porte </summary>
    public void ChangeNOTState()
    {
        socle.GetComponent<SocleNOTSystem>().SetActif(!socle.GetComponent<SocleNOTSystem>().IsActif());
    }
}
