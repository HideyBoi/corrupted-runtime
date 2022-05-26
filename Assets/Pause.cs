using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public DeathManager dm;
    public GameObject pauseMenu;

    public void Unpause() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        Cursor.visible = false;
    }

    public void ExitLevel() {
        dm.MainMenu();
    }
}
