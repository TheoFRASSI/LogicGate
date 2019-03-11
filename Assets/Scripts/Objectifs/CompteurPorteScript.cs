using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompteurPorteScript : MonoBehaviour {

    /// <summary> Le cable d'entrée du compteur de porte </summary>
    public CableScript cableEntree;
    /// <summary> Le cable de sortie du compteur de porte </summary>
    public CableScript cableSortie;
    /// <summary> Les sprites du compteur de porte </summary>
    public Sprite spriteON;
    public Sprite spriteOFF;
    /// <summary> Les sprites des differents numeros du compteur de porte </summary>
    public Sprite[] numeros;

    /// <summary> Les socles necessaires pour activer le compteur de porte </summary>
    private GameObject[] socles;
    /// <summary> Un compteur pour les portes actives </summary>
    private int cpt;

    private void Start()
    {
        socles = GameObject.FindGameObjectsWithTag("Socle");
        this.transform.GetChild(0).GetComponent<Image>().sprite = numeros[socles.Length];
    }

    /// <summary> Compte le nombre de socles actif </summary>
    private int NombreSocleActif()
    {
        cpt = 0;
        for (int i = 0; i < socles.Length; i++)
        {
            if (socles[i].transform.childCount != 0 && socles[i].GetComponent<SocleScript>().cableSortie.IsActif())
            {
                cpt++;
            }
        }
        return cpt;
    }

    // Update is called once per frame
    void Update () {
        this.transform.GetChild(0).GetComponent<Image>().sprite = numeros[socles.Length-NombreSocleActif()];

        //Défini si un socle est actif ou non
        if (NombreSocleActif() == socles.Length)
        {
            this.GetComponent<Image>().sprite = spriteON;
            cableSortie.SetActif(cableEntree.IsActif());
        }
        else
        {
            this.GetComponent<Image>().sprite = spriteOFF;
        }
	}
}
