using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Configurações de Vida")]
    [SerializeField] private int vida = 3;

    private BossDemonController bossController; // Referência para o script do boss
    private bool estaMorto = false;             // Para evitar que a morte seja chamada várias vezes

    void Awake()
    {
        // Pega o componente do controlador no mesmo GameObject
        bossController = GetComponent<BossDemonController>();
    }

    public void TomarDano(int dano)
    {
        // Se já está morto, não faz mais nada
        if (estaMorto) return;

        vida -= dano;
        Debug.Log($"🗡️ {gameObject.name} tomou {dano} de dano! Vida restante: {vida}");

        // Chama o método Morrer uma única vez quando a vida acaba
        if (vida <= 0)
        {
            estaMorto = true;
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log($"💀 {gameObject.name} está iniciando a sequência de morte...");
        // Avisa o BossDemonController para iniciar a animação e o processo de morte
        if (bossController != null)
        {
            bossController.Morrer();
        }
        else
        {
            Debug.LogError("BossDemonController não encontrado! A animação de morte não pode ser executada.");
        }
    }
}