using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;


    public bool pauseInput;
    private bool paused = false;
    private void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        pauseInput = Input.GetButtonDown("Cancel");
        if (pauseInput)
        {

            if (paused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
                pausePanel.SetActive(true);
            }
        }
    }

    public void LoadEndless()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResumeGame() { Time.timeScale = 1; pausePanel.SetActive(false); }
    public void PauseGame() { Time.timeScale = 0; }

}
