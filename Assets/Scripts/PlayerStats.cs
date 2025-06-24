using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int vida = 100;
    public bool IsAlive = true;

    void Update()
    {
        // Apenas para teste: tecla K causa dano
        if (Input.GetKeyDown(KeyCode.K))
        {
            TomarDano(50);
        }
    }

    // Método público para reduzir vida
    public void TomarDano(int dano)
    {
        if (!IsAlive) return;  // evita dano se já estiver morto

        vida -= dano;
        Debug.Log("Player tomou dano! Vida atual: " + vida);

        if (vida <= 0)
        {
            vida = 0;
            Death();
        }
    }

    // Método que define a morte
    public void Death()
    {
        if (!IsAlive)
            return;

        IsAlive = false;
        Debug.Log("Player morreu!");

        // Aqui você pode adicionar lógica para game over, animação, reset, etc.
    }
}
