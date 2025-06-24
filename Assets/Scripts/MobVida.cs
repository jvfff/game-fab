using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField] private int vida = 3;

    public void TomarDano(int dano)
    {
        vida -= dano;
        Debug.Log("Inimigo tomou dano! Vida restante: " + vida);

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }
}
