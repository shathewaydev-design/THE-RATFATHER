using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cutscene", menuName = "Cutscenes/Comic Cutscene")]
public class ComicCutscene : ScriptableObject
{
    public List<ComicPanel> panels = new();

    public AudioClip bkgMusic;
}
