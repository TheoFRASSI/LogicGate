using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level_list_manager : MonoBehaviour {

    /// <summary> Panel ou les boutons sont affiche </summary>
    public Transform panel;
    /// <summary> Prefab de bouton debloque </summary>
    public Button UnlockButton;
    /// <summary> Prefab du bouton bloque </summary>
    public Button NotUnlockButton;

    void Start()
    {
        // On fixe la resolution pour eviter des problemes d'interface
        Screen.SetResolution(720, 1440, true);

        // On recupere les noms des scenes et on rempli la liste
        List<string> scenes = new List<string>();
        scenes = countLevel();

        fillList(scenes);
        
        addListenerToAllButton();
    }

    /// <summary> Recupere les noms des scenes qui sont des niveaux de jeu et les renvoies sous forme de liste </summary>
    private List<string> countLevel()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;    // Nombre de scene du projet
        List<string>  scenes = new List<string>();     // Liste des scenes de jeu a retourner
        for (int i = 0; i < sceneCount; i++)    // Pour chaque scene ...
        {
            // On recupere le nom de la scene
            string sceneTemp = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            // Si c'est une scene contenant un niveau de jeu on l'ajoute a la liste
            if (sceneTemp.Contains("Level"))
            {
                scenes.Add(sceneTemp);
            }
        }
        return scenes;
    }

    /// <summary> Cree pour chaque scene un bouton qui permet de la rejoindre </summary>
    private void fillList(List<string> levels_name)
    {
        // Pour chaque level
        for (int i = 0; i < levels_name.Count; i++)
        {
            // Si la sauvegarde est inferieur au niveau on bloque le bouton
            if (PlayerPrefs.GetInt("level") < i + 1)
            {
                // On cree un clone du prefab
                Button newButton = Instantiate(NotUnlockButton);
                // Et on y ajoute les parametres voulus
                newButton.name = levels_name[i];
                newButton.GetComponentsInChildren<Text>()[0].text = (i + 1).ToString();
                // On ajoute le bouton au panel
                newButton.transform.SetParent(panel, false);
            }
            else
            {
                // On cree un clone du prefab
                Button newButton = Instantiate(UnlockButton);
                // Et on y ajoute les parametres voulus
                newButton.name = levels_name[i];
                newButton.GetComponentsInChildren<Text>()[0].text = (i + 1).ToString();
                // On ajoute le bouton au panel
                newButton.transform.SetParent(panel, false);
            }
            
            
        }
    }

    /// <summary> Ajoute un comportement a chaque bouton qui leur permet d'ouvrir la scene correspondante </summary>
    private void addListenerToAllButton()
    {
        // On recupere tous les bouton de la scene
        Button[] button = this.GetComponentsInChildren<Button>();
        // Et pour chacun on y ajoute un event OnClick() qui lance la scene
        foreach (Button but in button)
            but.onClick.AddListener(delegate { launchLevel(but.name); });
    }

    /// <summary> Lance la scene correspondante au nom passé en parametre </summary>
    /// <param name="nom"> Nom de la scene </param>
    private void launchLevel(string nom)
    {
        SceneManager.LoadScene(nom);
    }

}
