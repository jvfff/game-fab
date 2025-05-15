using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int vida = 100;
    public bool IsAlive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K)) { vida = vida - 50; }
        Console.WriteLine(vida.ToString());
        if (vida <= 0) { Death(); }
    }

    public void Death()
    {
        IsAlive = false;
    }
}
