using UnityEngine;

[System.Serializable]
public class ComicPanel
{
    public Texture image;

    public AudioClip soundEffect;

    [TextArea(3, 6)]
    public string dialogue;


    [TextArea(2, 4)]
    public string speaker;

}
