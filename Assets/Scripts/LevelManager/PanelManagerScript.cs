using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerScript : MonoBehaviour {

    /* NON FONCTIONNEL, EN DEVELOPPEMENT VOIR AMELIORATION FUTURE SUR WIKI */
    public GameObject[] panels;
    private int panelActu = 0;
    private int panelToSwitch;
    
    private GameObject objFinal;

    public List<ObjectifScript> etatsObjectifs;

    private void Start()
    {
        objFinal = GameObject.FindGameObjectWithTag("ObjMult");
    }

    public void OnClickNextPanel()
    {
        panelToSwitch = panelActu + 1;

        if(panelToSwitch == panels.Length)
        {
            panelToSwitch = 0;
        }

        panels[panelActu].SetActive(false);
        panels[panelToSwitch].SetActive(true);

        panelActu = panelToSwitch;
            
    }

    public void OnClickPrevPanel()
    {
        panelToSwitch = panelActu - 1;

        if (panelToSwitch < 0)
        {
            panelToSwitch = panels.Length-1;
        }

        panels[panelActu].SetActive(false);
        panels[panelToSwitch].SetActive(true);

        panelActu = panelToSwitch;

    }
    /*
    public bool IsObjectifsActifs()
    {

        for(int i = 0; i < etatsObjectifs.Count; i++)
        {
            if(!etatsObjectifs[i])
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if(IsObjectifsActifs())
        {
            objFinal.GetComponent<ObjectifScript>().isActif = true;
        }
        
    }*/
}
