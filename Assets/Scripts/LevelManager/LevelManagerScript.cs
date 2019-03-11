using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour {

    /// <summary> Niveau actuel </summary>
    public int currentLevel;
    /// <summary> Temps pour finir le niveau </summary>
    public int TimeLeft;
    /// <summary> L'objectif du niveau </summary>
    public ObjectifScript objectif;
    /// <summary> L'ecran de fin du niveau </summary>
    public GameObject ecranFin;
    /// <summary> L'animation de la vitre lors de la fin du niveau </summary>
    public Animator animationFin;
    /// <summary> Menu dereoulant du jeu </summary>
    public GameObject menuDeroulant;
    /// <summary> Chrono du niveau </summary>
    public GameObject chrono;
    public GameObject panelOption;

    /// <summary> Les boites de dialogues du niveau </summary>
    public GameObject dialogueDebut, dialogueFin;
    /// <summary> Les dialogues du niveau </summary>
    public Dialogue dialDebut, dialFin;
    /// <summary> Musiques de l'ecran de fin </summary>
    public AudioClip musicDefaite, musicVictoire;

    /// <summary> Source de musique du niveau (controlle par la radio) </summary>
    private AudioSource musicSource;

    /// <summary> Etat de la partie, True = partie terminee </summary>
    private bool endGame = false;

    /// <summary> Verifie si le joueur a termine le niveau </summary>
    public void CheckIfWin()
    {
        if(objectif != null)
        {
            // Si l'objectif est allume et que la partie n'est pas fini alors on termine la partie
            if (objectif.IsActif() && endGame == false)
            {
                endGame = true;
                GameOver(true);

            }
        }
        
    }

    /// <summary> Gere la fin de partie </summary>
    /// <param name="isWin"> True = victoire </param>
    public void GameOver(bool isWin)
    {
        // En cas de victoire
        if (isWin)
        {
            // On lance l'animation de fin de la vitre
            if(animationFin != null)
            {
                LaunchAnimationFin();
            }
            else
            {
                Debug.Log("animationFin manquant");
            }

            // On lance l'aniamtion de l'ecran de fin
            if (ecranFin != null)
            {
                AfficheEcranFin(isWin);
            }
            else
            {
                Debug.Log("ecranFin manquant");
            }
            
            // On lance la musique de victoire
            musicSource.clip = musicVictoire;
            musicSource.loop = true;
            musicSource.Play();
            GameObject.FindGameObjectWithTag("music").GetComponent<DontDestroyOnLoad>().listening = false;

            // On lance si il y a, le dialogue de fin
            if (dialFin != null && dialogueFin != null)
            {
                LanceDialogue(false);
            }


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

            // On met a jour la sauvegarde
            if (currentLevel == scenes)
            {
                PlayerPrefs.SetInt("level", 1);
            } else if (currentLevel >= PlayerPrefs.GetInt("level"))
            {
                PlayerPrefs.SetInt("level", currentLevel + 1);
            }
            
        }
        else
        { 
            // Sinon on affiche un ecran de fin de defaite
            AfficheEcranFin(isWin);
            // On lance la musique de defaite
            musicSource.clip = musicDefaite;
            musicSource.loop = true;
            musicSource.Play();
            GameObject.FindGameObjectWithTag("music").GetComponent<DontDestroyOnLoad>().listening = false;
        }
    }

    /// <summary> Affiche l'ecran de fin </summary>
    /// <param name="x"> True = victoire </param>
    public void AfficheEcranFin(bool x)
    {
        menuDeroulant.SetActive(false);
        ecranFin.transform.GetComponent<EcranFinDeNiveauScript>().SetupEndScreen(x, currentLevel);
    }

    /// <summary> Affiche le dialogue </summary>
    /// <param name="x"> True = dialogue de debut </param>
    public void LanceDialogue(bool x)
    {
        if (!x)
        {
            menuDeroulant.SetActive(false);
            dialogueFin.SetActive(true);
            dialogueFin.transform.GetChild(0).GetComponent<DialogManager>().StartDialog(dialFin);
        } else
        {
            dialogueDebut.SetActive(true);
            dialogueDebut.transform.GetChild(0).GetComponent<DialogManager>().StartDialog(dialDebut);
        }
    }

    /// <summary> Cache le dialogue </summary>
    /// <param name="x"> True = dialogue de debut </param>
    public void CacheDialogue(bool x)
    {
        if (!x)
        {
            dialogueFin.SetActive(false);
            
        } else
        {
            dialogueDebut.SetActive(false);
            // On lance le chrono des que le dialogue se termine
            if (chrono != null)
            {
                LaunchCountDown();
            }

        }
        
    }

    void Update () {
        // Des que la partie est fini on appelle CheckIfWin()
        if(endGame == false) {
            CheckIfWin();
        }
        else {
            //On arrete le chrono
            CancelInvoke("Count");
        }
        // On met le chrono a jour
        if(chrono != null)
        {
            chrono.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", TimeLeft);
        }
    }

    /// <summary> Lance le chrono </summary>
    public void LaunchCountDown()
    {
        chrono.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0}", TimeLeft);
        StartCounting();
    }

    void Start()
    {
        // On fixe la resolution de l'ecran
        Screen.SetResolution(720, 1440, true);
        // On lance la musique du niveau
        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        musicSource.volume = PlayerPrefs.GetFloat("volume");

        // On lance le dialogue et le chrono
        if (dialDebut != null && dialogueDebut != null)
        {
            LanceDialogue(true);
        } else
        {
            if (chrono != null)
            {
                LaunchCountDown();
            } else
            {
                Debug.Log("Chrono manquant");
            }
        }
    }

    /// <summary> Debut du comptage pour le chrono </summary>
    public void StartCounting()
    {
        InvokeRepeating("Count", 0, 1);
    }

    /// <summary> Permet de faire un compte a rebour et de lancer la defaite si celui ci atteint 0</summary>
    public void Count()
    {
        if(TimeLeft > 0)
        {
            TimeLeft--;
        }
        else
        {
            endGame = true;
            GameOver(false);
        }
    }

    /// <summary> Lance l'animation de fin de la vitre </summary>
    public void LaunchAnimationFin()
    {
        animationFin.Play("Decollage");        
    }


    public void OnClickOptionLevel()
    {
        if (panelOption.activeSelf)
        {
            panelOption.SetActive(false);
        } else
        {
            panelOption.SetActive(true);
        }
        
    }
}
