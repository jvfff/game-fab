using UnityEngine;

public class MobVida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 3;
    private int vidaAtual;

    private Animator animator;
    private int recebendoHitHash;

    private bool morto = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
        animator = GetComponent<Animator>();
        recebendoHitHash = Animator.StringToHash("recebendoHit");
    }

    public void TomarDano(int dano)
    {
        Debug.Log($"TomarDano chamado, dano: {dano}");
        if (morto) return;

        vidaAtual -= dano;

        if (animator != null)
            animator.SetTrigger(recebendoHitHash);

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }


    private void Morrer()
    {
        morto = true;

        // Aqui você pode tocar uma animação de morte se tiver
        // ou diretamente destruir o mob:
        Destroy(gameObject, 0.5f); // espera meio segundo para destruir
    }
}
