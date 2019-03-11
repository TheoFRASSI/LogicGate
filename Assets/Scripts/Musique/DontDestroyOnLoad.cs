using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    /// <summary> Permet de savoir si une musique est deja en cours de lecture, True = une musique est en cours de lecture </summary>
    public bool listening = false;

    void Awake()
    {
        // On recupere l'instance "Musiques"
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        // Si il y a deux objets Musiques on en supprime un
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }else
        {
            // Sinon on le protege
            DontDestroyOnLoad(this.gameObject);
        }

        
    }
}
