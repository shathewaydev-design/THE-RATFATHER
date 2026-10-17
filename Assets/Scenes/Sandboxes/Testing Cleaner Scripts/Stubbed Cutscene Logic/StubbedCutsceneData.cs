using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "StubbedCutsceneData", menuName = "Scriptable Objects/StubbedCutsceneData")]
public class StubbedCutsceneData : ScriptableObject
{
    public List<Sprite> panels = new List<Sprite>();

    [SerializeField] private bool teleportsPlayerOnFinish;
    public Vector3 teleport;

    public bool opensNewScene;
    [SerializeField] private string scenename;


    public bool TeleportsPlayer()
    {
        return teleportsPlayerOnFinish;
    }

    public bool OpensNewScene()
    {
        return opensNewScene;
    }

    public string GetSceneName()
    {
        if (!opensNewScene)
        {
            return null;
        }

        return scenename;
    }
}
