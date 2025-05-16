using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    PlayerStats status;
    GameObject player;

    public GameObject panelMenu;
    public GameObject panelPause;
    public GameObject panelGameOver;

    private bool isGameOverTriggered = false;

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
        if (status != null && !status.IsAlive && !isGameOverTriggered)
        {
            isGameOverTriggered = true;
            StartCoroutine(GameOverDelay());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private IEnumerator GameOverDelay()
    {
        Debug.Log("Morte detectada. Esperando 1.5 segundos...");
        yield return new WaitForSeconds(1.5f);
        GameOver();
    }

    private void GameOver()
    {
        panelGameOver.SetActive(true);
        panelPause.SetActive(false);
        panelMenu.SetActive(false);
        Debug.Log("Você morreu");
        CarregarJogo();
    }

    /*private void ResetGame()
    {
        // Aqui você pode colocar lógica para reiniciar variáveis ou cenas
    }*/

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
        isGameOverTriggered = false;
    }
    public void CarregarJogo()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TestMove"); // Substitua pelo nome real da sua cena
    }

    public void IniciarJogo()
    {
        panelMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGameOver.SetActive(false);
        Time.timeScale = 1;
        isGameOverTriggered = false;
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
