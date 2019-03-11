using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier un cable d'un autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class CableSimpleSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe CableSimple </summary>
class CableSimple : ComponentSystem
{
    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public CableScript cable;
        public CableSimpleSystem simple;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            if (e.cable.previousCable != null)
            {
                // Si il y a un cable avant celui ci, on passe ce cable dans le meme etat
                if (e.cable.previousCable.IsActif())
                {
                    e.cable.SetActif(true);
                }
                else
                {
                    e.cable.SetActif(false);
                }
                
            } 
            e.cable.ChangeSprite();
        }
    }
}