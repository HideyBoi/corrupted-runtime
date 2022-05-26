using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public Animator coruptionPausedAnim;
    public TextMeshProUGUI timeRemainingText;

    public bool isCorruptionFrozen;
    float timeRemaining;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        timeRemaining -= Time.deltaTime;
        timeRemainingText.text = "Time Remaining: " + Mathf.Round(timeRemaining).ToString();
        if (timeRemaining < 0) {
            isCorruptionFrozen = false;
            coruptionPausedAnim.SetBool("Show", false);
        }
    }

    public static void StopCorruption(float time) {
        instance.isCorruptionFrozen = true;
        instance.timeRemaining = time;
        instance.coruptionPausedAnim.SetBool("Show", true);
    }
}
