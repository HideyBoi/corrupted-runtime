using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject endEffect;
    public Animator deathAnimator;

    public AudioSource dead;

    public void KillPlayer() {
        StartCoroutine("RestartLevel");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Player>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        deathAnimator.SetBool("IsDead", true);
        Instantiate(deathEffect, transform.position, transform.rotation);
        dead.Play();
    }

    public void Win() {
        StartCoroutine("LoadScene");

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Player>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    public void MainMenu() {
        StartCoroutine("BackToMainMenu");
    }

    IEnumerator BackToMainMenu() {
        Time.timeScale = 1f;
        deathAnimator.SetBool("win", true);
        yield return new WaitForSecondsRealtime(1f);
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadScene() {
        Instantiate(endEffect, transform.position, transform.rotation);
        yield return new WaitForSecondsRealtime(4f);
        deathAnimator.SetBool("win", true);
        yield return new WaitForSecondsRealtime(1f);
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator RestartLevel() {
        yield return new WaitForSecondsRealtime(2f);
        deathAnimator.SetBool("IsLoading", true);
        yield return new WaitForSecondsRealtime(0.015f);
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
