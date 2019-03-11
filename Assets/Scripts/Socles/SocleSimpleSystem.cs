using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier un socle d'un autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class SocleSimpleSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe SocleSimple </summary>
class SocleSimple : ComponentSystem
{

    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public SocleScript socle;
        public SocleSimpleSystem simple;
    }

    /// <summary> Allume le cable de sortie si les cables d'entrées sont allumés et qu'une porte verifiant les conditions est posée </summary>
    protected override void OnUpdate()
    {
        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            if (e.socle.IsPortePresente())
            {
                e.socle.transform.GetChild(0).GetComponent<PorteScript>().SetEntree1(e.socle.cableDroite.IsActif());
                
                e.socle.transform.GetChild(0).GetComponent<PorteScript>().SetEntree2(e.socle.cableGauche.IsActif());
               
                e.socle.cableSortie.SetActif(e.socle.transform.GetChild(0).GetComponent<PorteScript>().IsSortie());
            } else
            {
                e.socle.cableSortie.SetActif(false);
            }
        }
    }
}