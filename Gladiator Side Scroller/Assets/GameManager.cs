using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject pausePanel;

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
        if (playerInput.pauseInput)
        {
            pausePanel.SetActive(true);
            PauseGame();

        }
    }

    public void ResumeGame() { Time.timeScale = 1; pausePanel.SetActive(false); }
    public void PauseGame() { Time.timeScale = 0; }

}
