using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    PlayerStats status;
    GameObject player;

    public GameObject panelMenu;
    public GameObject panelPause;
    public GameObject panelGameOver;
    

    void Awake()
    {
        CarregarMenu();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            status = player.GetComponent<PlayerStats>();
        }
    }


    void Update()
    {
        if (status != null && !status.IsAlive)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
    }


    private void GameOver()
    {
        panelGameOver.SetActive(true);
        panelPause.SetActive(false);
        panelMenu.SetActive(false);
        Debug.Log("Você morreu");
        ResetGame();
    }

    private void ResetGame()
    {
        
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        panelPause.SetActive(true);
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
    }

    public void CarregarMenu()
    {
        Time.timeScale = 0;
        panelMenu.SetActive(true);
        panelPause.SetActive(false);
        panelGameOver.SetActive(false);
    }

    public void IniciarJogo()
    {
        //essa função tem q carregar a cena
        panelMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGameOver.SetActive(false);
        Time.timeScale = 1;
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Fecha o modo Play no Editor
        #else
        Application.Quit();  // Fecha o jogo compilado
        #endif
    }
}
