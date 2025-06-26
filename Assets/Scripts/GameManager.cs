using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    PlayerStats status;
    GameObject player;
    public GameObject tutorialAttack;
    public GameObject tutorialMove;
    public GameObject tutorialPot;
    public GameObject tutorialJump;

    public int index = 0;


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
    void Start()
    {
        
        index = 1;
    }

    void Update()
    {
        if (status != null && !status.IsAlive)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
    }

    public void AtualizarTutorial()
    {
        // Desativa todos primeiro
        tutorialAttack.SetActive(false);
        tutorialMove.SetActive(false);
        tutorialPot.SetActive(false);
        tutorialJump.SetActive(false);

        // Ativa o correto com base no index
        switch (index)
        {
            case 0:
                tutorialAttack.SetActive(true);
                break;
            case 1:
                tutorialMove.SetActive(true);
                break;
            case 2:
                tutorialPot.SetActive(true);
                break;
            case 3:
                tutorialJump.SetActive(true);
                break;
            default:
                tutorialAttack.SetActive(false);
                tutorialMove.SetActive(false);
                tutorialPot.SetActive(false);
                tutorialJump.SetActive(false);
                break;
        }
    }
    public void SetTutorialIndex(int novoIndex)
    {
        index = novoIndex;
        AtualizarTutorial();
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
        AtualizarTutorial();
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
