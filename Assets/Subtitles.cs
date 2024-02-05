using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subtitle
{
    public string text;
    public float show;
    public float hide;
}

[CreateAssetMenu(fileName = "Subtitles", menuName = "Subtitles", order = 1)]
public class Subtitles : ScriptableObject
{
    public List<Subtitle> list;
}
