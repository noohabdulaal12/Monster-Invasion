using System.Collections;
using UnityEngine;
using TMPro;

public class BreakManager : MonoBehaviour
{
    public float breakDuration = 30f;
    public TextMeshProUGUI breakTimerText;

    public bool IsBreakTime { get; private set; }

    void Start()
    {
        if (breakTimerText != null)
            breakTimerText.gameObject.SetActive(false);
    }

    public IEnumerator StartBreak()
    {
        IsBreakTime = true;

        float timer = breakDuration;

        if (breakTimerText != null)
            breakTimerText.gameObject.SetActive(true);

        while (timer > 0)
        {
            if (breakTimerText != null)
                breakTimerText.text = "Next Round In: " + Mathf.Ceil(timer);

            timer -= Time.deltaTime;
            yield return null;
        }

        if (breakTimerText != null)
            breakTimerText.gameObject.SetActive(false);

        IsBreakTime = false;
    }
}