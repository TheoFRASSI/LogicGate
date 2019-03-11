using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EcranFinDeNiveauScript : MonoBehaviour
{
    /// <summary> Niveau suivant </summary>
    private int nextLevel;

    /// <summary> Animation permettant l'apparition en fondu de l'ecran de fin </summary>
    public Animator animFondu;

    /// <summary> Permet de retourner sur la scene MainMenu </summary>
    public void ReturnToMenu()
    {
        // On coupe la musique
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length >= 1)
        {
            Destroy(objs[0].gameObject);
        }

        // On change de scene
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary> Lance le niveau suivant </summary>
    public void NextLevel()
    {

        // Pour savoir si on a atteind le dernier niveau ------------------------------------------
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;    // Nombre de scene du projet
        int scenes = 0;     // Compteur de scenes etant un niveau de jeu
        for (int i = 0; i < sceneCount; i++)    // Pour chaque scene ...
        {
            // On recupere le nom de la scene
            string sceneTemp = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            // Si c'est une scene contenant un niveau de jeu on incremente le compteur
            if (sceneTemp.Contains("Level"))
            {
                scenes++;
            }
        }
        //-----------------------------------------------------------------------------------------

        // Si on a fini le jeu
        if (nextLevel > scenes)
        {
            // On lance les credits
            SceneManager.LoadScene("Credits");
        }
        else
        {
            // Sinon on passe au niveau suivant
            SceneManager.LoadScene("Level" + nextLevel);
        }
        
    }

    /// <summary> On relance le niveau en rechargeant la scene actuelle </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary> Lance l'animation de l'ecran de fin </summary>
    public IEnumerator LaunchAnimFondu() {
        animFondu.Play("fonduEcranFin");
        yield return new WaitForSeconds(3);
    }

    /// <summary> Adapte l'ecran de fin selon si c'est une victoire ou une defaite </summary> 
    /// <param name="level"> Niveau actuel </param> <param name="victory"> Etat de victoire ou de defaite : True pour victoire </param>
    public void SetupEndScreen(bool victory, int level)
    {
        // On affiche l'ecran de fin
        this.gameObject.SetActive(true);
        // On lance l'animation avec une coroutine
        StartCoroutine("LaunchAnimFondu");

        // En cas de victoire :
        if (victory)
        {
            // On ecrit victoire dans le label texte correspondant
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "VICTOIRE";
            // On retire le bouton recommencer
            this.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
            // On affiche le bouton de niveau suivant
            this.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            // On change la valeur nextLevel
            this.nextLevel = level + 1;
        }
        else
        {
            // On ecrit defaite dans le label correspondant
            this.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "DEFAITE";
            // On retire le bouton de niveau suivant
            this.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
            // On affiche le bouton pour recommencer
            this.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
        }


    }
}