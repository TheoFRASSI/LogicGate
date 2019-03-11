using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDeroulant : MonoBehaviour {

    /// <summary> Bouton pour ouvrir et fermer le menu deroulant </summary>
    public Button Button;
    /// <summary> Etat affiche ou cache du menu deroulant, True = affiche </summary>
    private bool isDisplay;
    /// <summary> Animation d'affichage ou non </summary>
    private Animator anim;
    /// <summary> Differents sprite pour le bouton du menu </summary>
    public Sprite spriteHaut;
    public Sprite spriteBas;

    void Start()
    {
        // On initialise le panel en etat cache
        isDisplay = false;
        // On disabled l'animator
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    /// <summary> Permet d'alterner avec l'affichage et le fait de cacher le menu deroulant </summary>
    public void change()
    {
        if (isDisplay)
        {
            HidePanel();
        }
        else
        {
            DisplayPanel();
        }
    }

    /// <summary> Cache le menu deroulant </summary>
    public void HidePanel()
    {
        // On active les animations
        anim.enabled = true;
        // On lance l'animation pour cacher le menu deroulant
        anim.Play("MenuDeroulantCacher");
        // On change le sprite du bouton
        this.transform.GetChild(0).GetComponent<Image>().sprite = spriteHaut;

        // Et on passe l'etat du menu a false
        isDisplay = false;
    }

    /// <summary> Affiche le menu deroulant </summary>
    public void DisplayPanel()
    {
        // On active les animations
        anim.enabled = true;
        // On lance l'animation pour afficher le menu deroulant
        anim.Play("MenuDeroulantAfficher");
        // On change le sprite du bouton
        this.transform.GetChild(0).GetComponent<Image>().sprite = spriteBas;

        // Et on passe l'etat du menu a true
        isDisplay = true;
    }
}
