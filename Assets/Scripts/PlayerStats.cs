using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float vida = 100;
    public bool IsAlive = true;
    public int potion = 3;
    private int danoOriginal;

    public Movimento movimento;

    public float tempoDuracao = 15f;
    private bool pocaoAtiva = false;

    [SerializeField] public Transform HealthBar;

    private void Awake()
    {
        movimento = GetComponent<Movimento>();
    }
    void Start()
    {

        danoOriginal = movimento.dano;
    }

    void Update()
    {
        AtualizarBarraVida(vida);
        // Apenas para teste: tecla K causa dano
        if (Input.GetKeyDown(KeyCode.K))
        {
            TomarDano(50);
        }

        if (Input.GetKeyDown(KeyCode.E)) TomarPocao();
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

    //Efeito da poção
    /*Tem pot? se tiver dobra o dano do personagem durante 15seg tempo se n tiver nada ocorre*/
    public void TomarPocao()
    {
        if (potion > 0 && !pocaoAtiva)
        {
            potion--;
            StartCoroutine(EfeitoPocao());
        }
        else if (pocaoAtiva)
        {
            Debug.Log("Poção já está ativa.");
        }
        else
        {
            Debug.Log("Sem poções!");
        }
    }

    private IEnumerator EfeitoPocao()
    {
        pocaoAtiva = true;
        movimento.dano *= 3;
        float tempoRestante = tempoDuracao;

        while (tempoRestante > 0)
        {
            Debug.Log("Poção ativa por mais " + tempoRestante.ToString("F1") + "s");
            yield return new WaitForSeconds(1f);
            tempoRestante -= 1f;
        }

        movimento.dano = danoOriginal;
        pocaoAtiva = false;
        Debug.Log("Efeito da poção acabou.");
    }

}
