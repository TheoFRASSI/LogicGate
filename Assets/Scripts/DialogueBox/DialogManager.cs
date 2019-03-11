using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

    /// <summary> Nom du personnage </summary>
    public Text nameText;
    /// <summary> Boite de dialogue </summary>
    public Text dialogueText;

    /// <summary> FIFO permettant d'enchainer les boites de dialogues </summary>
    public Queue<string> sentences;
    /// <summary> True : dialogue de debut, False : dialogue de fin </summary>
    public bool type;
    
	void Awake () {
        sentences = new Queue<string>();      // Ne fonctionne pas avec Start, Awake permet de le creer des le lancement du programme
    }

    /// <summary> Lance les fonctions permettant d'afficher et d'interagir avec le dialogue </summary>
    public void StartDialog(Dialogue dial)    {
        
        // On recupere le nom du personnage du dialogue
        nameText.text = dial.name;
        
        // On vide la FIFO
        sentences.Clear();

        // Pour chaque phrase du dialogue on le place dans la FIFO
        foreach(string sentence in dial.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // On lance la premiere phrase de la FIFO
        DisplayNextSentence();
    }

    /// <summary> Permet de lancer la phrase suivante de la FIFO </summary>
    public void DisplayNextSentence()
    {
        // S'il n'y a pas de phrases on arrete le dialogue
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // On recupere la phrase et on la retire de la FIFO
        string sentence = sentences.Dequeue();

        // Pour arreter la coroutine de la phrase precedente
        StopAllCoroutines();
        // Lance la couroutine sur la phrase recupere
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary> Permet d'ecrire la phrase charactere par charactere </summary>
    IEnumerator TypeSentence (string sentence)
    {
        // Initialisation
        dialogueText.text = "";

        // Pour chaque lettre de la phrase ...
        foreach (char letter in sentence.ToCharArray())
        {
            // On ajoute la lettre au texte precedent
            dialogueText.text += letter;
            yield return null;
        }
    }

    /// <summary> Termine le dialogue </summary>
    void EndDialogue()
    {
        FindObjectOfType<LevelManagerScript>().CacheDialogue(this.type);
    }
	


}
