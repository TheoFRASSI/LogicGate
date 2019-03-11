using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    /// <summary> Le panel avec les boutons du menu de base </summary>
    public GameObject panelMenu;
    /// <summary> Le panel avec les boutons concernant la campagne </summary>
    public GameObject panelCampagne;
    /// <summary> Le bouton pour continuer la campagne </summary>
    public Button continuer;
    /// <summary> Le bouton de la liste de niveaux </summary>
    public Button liste_niveaux;

    /// <summary> Permet de lancer une nouvelle campagne </summary>
    public void NewCampagne ()
    {
        // On initialise la sauvegarde au niveau "0"
        PlayerPrefs.SetInt("level", 0);
        // Et on lance le premier niveau
        SceneManager.LoadScene("Level1");
    }

    /// <summary> On recupere le niveau dans la sauvegarde et on lance le niveau correspondant </summary>
    public void ContinueCampagne()
    {
        int level = PlayerPrefs.GetInt("level");
        SceneManager.LoadScene("Level" + level);
    }

    private void Start()
    {
        // On fixe la resolution pour eviter des problemes d'interface
        Screen.SetResolution(720, 1440, true);
    }

    /// <summary> Charge la scene de selection de niveau </summary>
    public void LevelSelector()
    {
        SceneManager.LoadScene("Liste_niveaux");
    }

    /// <summary> Charge la scene du menu principal </summary>
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary> Change de panel vers celui de la campagne </summary>
    public void PanelCampagne()
    {
        // On active le bouton continuer et choix de niveau seulement si le joueur a fini le 1er niveau
        if (PlayerPrefs.GetInt("level") > 0)
        {
            continuer.interactable = true;
            liste_niveaux.interactable = true;
        }

        // Par securite
        if (!panelCampagne.activeSelf)
        {
            panelCampagne.SetActive(true);
        }
    }

    /// <summary> Charge la scene des options </summary>
    public void OpenOptions()
    {
        SceneManager.LoadScene("MenuOptions");
    }

    /// <summary> Quite et ferme l'application (Lors de test sur unity n'a aucun effet) </summary>
    public void ExitGame()
    {
        //Debug.Log("Application fermée");
        Application.Quit();
    }
}
