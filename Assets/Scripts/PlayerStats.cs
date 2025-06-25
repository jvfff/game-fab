using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float vida = 100;
    public bool IsAlive = true;
    

    [SerializeField] public Transform HealthBar;

    void Update()
    {
        AtualizarBarraVida(vida);
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
    public void AtualizarBarraVida(float novaVida)
    {
        vida = Mathf.Clamp(novaVida, 0, 100); // Garante que fique entre 0 e 100
        float porcentagem = vida / 100f; // Converte para valor entre 0 e 1
        HealthBar.localScale = new Vector3(porcentagem, 1f, 1f); // Atualiza a escala em X
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
