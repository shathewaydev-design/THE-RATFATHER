using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject // data accesible and editable form unity editor
{
    // instance variables
    public string characterName;

    [TextArea(3, 10)]
    public string[] lines;


    // helper methods below
    public int LineCount()
    {
        return lines.Length;
    }

}
