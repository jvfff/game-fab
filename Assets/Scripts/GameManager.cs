using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    PlayerStats status;
    GameObject player;
    public bool menu;

    void Awake()
    {
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
    }


    private void GameOver()
    {
        Debug.Log("Você morreu");
        ResetGame();
    }

    private void ResetGame()
    {

    }
}
