using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier une porte OR d'une autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class PorteORSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe PorteOR </summary>
class PorteOR : ComponentSystem
{

    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public PorteScript porte;
        public PorteORSystem or;
    }

    protected override void OnUpdate()
    {
        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            e.porte.ChangeSprite();
            e.porte.SetSortie(e.porte.IsEntree1() || e.porte.IsEntree2());
        }
    }
}
