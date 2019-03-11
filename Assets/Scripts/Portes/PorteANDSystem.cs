using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier une porte AND d'une autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class PorteANDSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe PorteAND </summary>
class PorteAND : ComponentSystem
{

    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public PorteScript porte;
        public PorteANDSystem and;
    }


    protected override void OnUpdate()
    {
        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            e.porte.ChangeSprite();
            e.porte.SetSortie(e.porte.IsEntree1() && e.porte.IsEntree2());
        }
    }
}
