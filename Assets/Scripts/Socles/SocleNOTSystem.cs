using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class SocleNOTSystem : MonoBehaviour
{
    public bool actif = false;
    /// <summary> Les sprites </summary>
    public Sprite spriteOff;
    public Sprite spriteOn;

    public bool IsActif()
    {
        return actif;
    }

    public void SetActif(bool x)
    {
        this.actif = x;
    }

    /// <summary> Applique le sprite </summary>
    public void ChangeSprite()
    {
        if (IsActif())
        {
            this.GetComponent<Image>().sprite = spriteOn;
        }
        else
        {
            this.GetComponent<Image>().sprite = spriteOff;
        }
    }
}

class SocleNOT : ComponentSystem
{
    /// <summary> Liste des composants necessaires pour l'application du comportement </summary>
    struct Components
    {
        public RectTransform transform;
        public SocleScript socle;
        public SocleNOTSystem not;
    }

    /// <summary> Inverse la fonction de la porte </summary>
    protected override void OnUpdate()
    {

        // Pour chaques objets comprenant ces composants
        foreach (var e in GetEntities<Components>())
        {
            e.not.ChangeSprite();
            if (e.socle.IsPortePresente())
            {
                e.socle.transform.GetChild(0).GetComponent<PorteScript>().SetEntree1(e.socle.cableDroite.IsActif());

                e.socle.transform.GetChild(0).GetComponent<PorteScript>().SetEntree2(e.socle.cableGauche.IsActif());

                if (e.not.IsActif())
                {
                    e.socle.cableSortie.SetActif(false);
                    e.socle.cableSortie.SetActif(!e.socle.transform.GetChild(0).GetComponent<PorteScript>().IsSortie());
                } else
                {
                    e.socle.cableSortie.SetActif(false);
                    e.socle.cableSortie.SetActif(e.socle.transform.GetChild(0).GetComponent<PorteScript>().IsSortie());
                }
                
            }
            else
            {
                e.socle.cableSortie.SetActif(false);
            }
        }
    }
}