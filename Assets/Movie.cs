using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class Movie : MonoBehaviour
{
    public Subtitles subtitles;
    public TMP_Text subtitleText;
    public TMP_Text timecodeText;
    public VideoPlayer player;
    float time;
    float totalCharacters;
    float totalWords;

    IEnumerator Start()
    {
        time = 0;

        player.Prepare();
        yield return new WaitUntil(() => player.isPrepared);
        player.Play();
        StartCoroutine(ShowSubtitles());
        while (time < player.length)
        {
            time += Time.deltaTime;
            timecodeText.text = time.ToString();
            yield return null;
        }
    }

    IEnumerator ShowSubtitles()
    {
        foreach (Subtitle subtitle in subtitles.list)
        {
            yield return new WaitUntil(() => time > subtitle.show);
            subtitleText.text = subtitle.text;
            int words = subtitle.text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries).Length;
            int characters = subtitle.text.Length - words - 1;
            float duration = subtitle.hide - subtitle.show;
            if (words / duration * 60 > 190)
            {
                Debug.LogWarning($"More than 190 words per minute with line '{subtitle.text}' at {time}");
            }
            if (characters / duration > 16)
            {
                Debug.LogWarning($"More than 16 characters per second line '{subtitle.text}' at {time}");
            }
            totalCharacters += characters;
            totalWords += words;
            yield return new WaitUntil(() => time > subtitle.hide);
            subtitleText.text = "";
            float recommendedPause = 0.5f;
            yield return new WaitForSeconds(recommendedPause);
        }

        if (totalWords / time * 60 > 150)
        {
            Debug.LogWarning($"More than 150 words per minute on average");
        }
        if (totalCharacters / time > 12)
        {
            Debug.LogWarning($"More than 12 characters per second on average");
        }
    }
}
