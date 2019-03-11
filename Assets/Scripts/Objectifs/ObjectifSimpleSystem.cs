using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

/// <summary> Permet de differencier un objectif d'un autre, permet aussi d'y ajouter des donnees si besoin est, necessaire pour l'Hybride  </summary>
public class ObjectifSimpleSystem : MonoBehaviour
{

}

/// <summary> Comportement de la classe ObjectifSimple </summary>
class ObjectifSimple : ComponentSystem
{

    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public ObjectifScript objectif;
        public ObjectifSimpleSystem simple;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {

        //Applique le sprite
        foreach (var e in GetEntities<Components>())
        {
            e.objectif.ChangeSprite();
        }
    }
}
