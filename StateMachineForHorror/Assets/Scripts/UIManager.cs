using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject PauseUI;
    public GameObject EndUI;

    public Text stamina;

    Movement playerMove;

    void Start()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        EndUI.SetActive(false);
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        stamina.text = playerMove.stamina + "";
    }

	public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        PauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowGameOver()
    {
        EndUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

}
