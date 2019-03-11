using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Radio_manager : MonoBehaviour {

    /// <summary> Liste de toutes les musiques</summary>
    public List<AudioClip> musics = new List<AudioClip>();
    /// <summary> Bouton de la radio </summary>
    public Button button;
    /// <summary> La reference à l'audio source </summary>
    private AudioSource musicSource;
    /// <summary> La musiques precedente </summary>
    private AudioClip musicPrev = null;
    
    void Start () {
        // On recupere la source de musique du niveau
        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        // On ajoute un event OnClick() sur le bouton
        if (button != null)
        {
            button.onClick.AddListener(PlaySting);
        }
        
        // Si il n'y a aucune musique en cours
        if(!GameObject.FindGameObjectWithTag("music").GetComponent<DontDestroyOnLoad>().listening)
        {
            // On en lance une et on dit que une musique est en cours
            PlaySting();
            GameObject.FindGameObjectWithTag("music").GetComponent<DontDestroyOnLoad>().listening = true;
        }
    }

    /// <summary> Permet de lancer la musique suivante </summary>
    void PlaySting()
    {
        // On recupere un nombre aleatoire entre 0 et le nombre de musiques
        int randClip = Random.Range(0, musics.Count);

        // On évite de jouer 2 fois la même musique à la suite
        if(musicPrev != null)
        {
            while (musicPrev == musics[randClip])
            {
                randClip = Random.Range(0, musics.Count);
            }
        }
        // On met a jour la musique precedente
        musicPrev = musics[randClip];

        // Et on lance la musique choisie
        musicSource.clip = musics[randClip];
        musicSource.Play();
    }

    private void Update()
    {
        // Des qu'il n'y a plus de musique on en lance une autre
        if(!musicSource.isPlaying)
        {
            PlaySting();
        }
    }
}