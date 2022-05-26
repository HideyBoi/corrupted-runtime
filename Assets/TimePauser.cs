using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauser : MonoBehaviour
{
    public GameObject sound;

    public float time;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.StopCorruption(time);
            Instantiate(sound, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
