using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_manager : MonoBehaviour {

    /// <summary> Le slider permettant de modifier le volume </summary>
    public Slider slider;
    /// <summary> Le volume qui doit être modifier </summary>
    public new AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        //Récupère la précédente valeur du slider, s'il n'y en a pas renvoie 1
        slider.value = PlayerPrefs.GetFloat("slider", 1);
        slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        //Récupère la précédente valeur du volume, s'il n'y en a pas renvoie 1
        audio.volume = PlayerPrefs.GetFloat("volume");
    }

    /// <summary> Modifie le volume en même temps que le slider bouge </summary>
    public void OnValueChanged()
    {
        audio.volume = slider.value;
        PlayerPrefs.SetFloat("volume", audio.volume);
        PlayerPrefs.SetFloat("slider", slider.value);
    }

    // Update is called once per frame
    public void Update()
    {
        audio = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
    }
}
