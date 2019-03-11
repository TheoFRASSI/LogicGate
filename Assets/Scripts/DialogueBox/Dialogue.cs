using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary> Correspond au nom du personnage et aux phrases qu'il dit </summary>
public class Dialogue {

    /// <summary> Nom du personnage </summary>
    public string name;

    [TextArea(3, 10)]
    /// <summary> Tableau contenant toutes les phrases de dialogue </summary>
    public string[] sentences;
	

}
