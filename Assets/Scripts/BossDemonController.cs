using UnityEngine;

public class BossDemonController : MonoBehaviour
{
    // --- Componentes e Variáveis Essenciais ---
    private Rigidbody2D rb;
    private Animator animator;
    private Transform jogador;

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 2.5f;
    [SerializeField] private Transform peDoMob;
    [SerializeField] private Transform sensorParede;
    [SerializeField] private LayerMask chaoLayer;
    [SerializeField] private float tempoMinimoParaVirar = 0.5f;

    [Header("Configurações de Combate")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform pontoCentroAtaque;
    [SerializeField] private float distanciaDeteccao = 8f;
    [SerializeField] private float alcanceAtaque = 2f;
    [SerializeField] private float cooldownAtaque = 2.5f;
    [SerializeField] private HitboxAtaqueMob hitboxAtaque;
    
    [Header("Timings da Animação de Ataque")]
    [Tooltip("Duração total da animação de ataque para travar o boss.")]
    [SerializeField] private float duracaoAnimacaoAtaque = 1.2f;
    [Tooltip("Tempo até a hitbox ser ativada durante a animação.")]
    [SerializeField] private float tempoParaAtivarHitbox = 0.4f;
    [Tooltip("Tempo até a hitbox ser desativada.")]
    [SerializeField] private float tempoParaDesativarHitbox = 0.8f;

    [Header("Configurações de Morte")]
    [Tooltip("Tempo em segundos após a animação de morte para o objeto ser destruído.")]
    [SerializeField] private float tempoParaDestruirAposMorte = 2f;

    // --- Controle de Estado Interno ---
    private float direcao = 1f;
    private float tempoDesdeUltimaVirada = 0f;
    private float timerCooldownAtaque = 0f;
    private bool atacando = false;
    private bool detectouParede = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        tempoDesdeUltimaVirada += Time.deltaTime;
        timerCooldownAtaque += Time.deltaTime;

        if (atacando)
        {
            return;
        }

        Collider2D jogadorDetectado = Physics2D.OverlapCircle(transform.position, distanciaDeteccao, playerLayer);

        if (jogadorDetectado != null)
        {
            HandleComportamentoComJogador(jogadorDetectado);
        }
        else
        {
            HandleComportamentoDePatrulha();
        }

        AtualizarAnimacaoMovimento();
    }

    void FixedUpdate()
    {
        // Se o mob está atacando, sua velocidade horizontal é forçada a ser zero.
        if (atacando)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; // Sai do método para garantir que nenhum outro movimento seja aplicado.
        }

        // Se NÃO está atacando, ele se move normalmente na 'direcao' atual.
        rb.linearVelocity = new Vector2(direcao * velocidade, rb.linearVelocity.y);
    }

    private void HandleComportamentoComJogador(Collider2D jogadorDetectado)
    {
        jogador = jogadorDetectado.transform;
        
        Vector3 centroDoAtaque = (pontoCentroAtaque != null) ? pontoCentroAtaque.position : transform.position;
        float distanciaParaJogador = Vector2.Distance(jogador.position, centroDoAtaque);
        
        float direcaoParaJogador = Mathf.Sign(jogador.position.x - transform.position.x);

        bool podeAtacar = distanciaParaJogador <= alcanceAtaque && timerCooldownAtaque >= cooldownAtaque;
        if (podeAtacar)
        {
            if (direcao != direcaoParaJogador)
            {
                Virar();
            }
            Atacar();
        }
        else 
        {
            if (direcao != direcaoParaJogador && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
            {
                Virar();
            }
        }
    }

    private void HandleComportamentoDePatrulha()
    {
        jogador = null;

        Vector2 origemRaycast = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
        bool temChao = Physics2D.Raycast(origemRaycast, Vector2.down, 0.5f, chaoLayer);

        if ((!temChao || detectouParede) && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            detectouParede = false;
        }
    }

    private void AtualizarAnimacaoMovimento()
    {
        float velocidadeX = Mathf.Abs(rb.linearVelocity.x);
        animator.SetBool("isWalking", velocidadeX > 0.1f);
    }
    
    public void SensorParedeDetectou()
    {
        detectouParede = true;
    }

    public void Atacar()
{
    // Não zeramos mais o cooldown aqui
    atacando = true;
    
    animator.SetTrigger("cleave");

    Invoke(nameof(AtivarHitbox), tempoParaAtivarHitbox);
    Invoke(nameof(DesativarHitbox), tempoParaDesativarHitbox);
    Invoke(nameof(FimDoAtaque), duracaoAnimacaoAtaque);
}
    
    private void FimDoAtaque()
    {
        atacando = false;
    }

    private void AtivarHitbox()
    {
        hitboxAtaque?.AtivarHitbox();
    }

    private void DesativarHitbox()
    {
        hitboxAtaque?.DesativarHitbox();
    }

    public void ReceberHit()
    {
        animator.SetTrigger("takeHit");
    }

    public void Morrer()
    {
        animator.SetTrigger("death");
        this.enabled = false;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        Destroy(gameObject, tempoParaDestruirAposMorte);
    }

    private void Virar()
    {
        tempoDesdeUltimaVirada = 0f;
        direcao *= -1f;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direcao, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 centroDoAtaque = (pontoCentroAtaque != null) ? pontoCentroAtaque.position : transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centroDoAtaque, alcanceAtaque);
        
        if (peDoMob != null)
        {
            Vector2 origem = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origem, origem + Vector2.down * 0.5f);
        }
    }
}