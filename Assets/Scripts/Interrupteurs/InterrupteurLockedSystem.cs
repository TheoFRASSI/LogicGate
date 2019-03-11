using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier un interrupteur d'un autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class InterrupteurLockedSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe InterrupteurLocked </summary>
class InterrupteurLocked : ComponentSystem
{
    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public InterrupteurScript interrupteur;
        public InterrupteurLockedSystem locked;
    }

    protected override void OnUpdate()
    {
        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            // On raffraichit son etat et son sprite
            e.interrupteur.Refresh();
            e.interrupteur.ChangeSprite();
        }
    }
}