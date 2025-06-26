using UnityEngine;

public class MobMovinigth : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float tempoMinimoParaVirar = 0.5f;

    [SerializeField] private float velocidade = 3f;
    [SerializeField] private Transform peDoMob;
    [SerializeField] private LayerMask chaoLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float distanciaDeteccao = 5f;
    [SerializeField] private float alcanceAtaque = 1.5f;
    [SerializeField] private float cooldownAtaque = 2f;
    [SerializeField] private Transform sensorParede;
    [SerializeField] private HitboxAtaqueMob hitboxAtaque;

    private float direcao = 1f;
    private float tempoDesdeUltimaVirada = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform jogador;

    private bool detectouParede = false;
    private float timerCooldownAtaque = 0f;
    private bool atacando = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
{
    tempoDesdeUltimaVirada += Time.deltaTime;
    timerCooldownAtaque += Time.deltaTime;

    // 1. Se estiver no meio de um ataque, não faz mais nada.
    if (atacando)
    {
        animator.SetBool("walk", false);
        animator.SetBool("idle_1", false);
        return;
    }

    // 2. DETECÇÃO DE JOGADOR
    Collider2D jogadorDetectado = Physics2D.OverlapCircle(transform.position, distanciaDeteccao, playerLayer);

    if (jogadorDetectado != null)
    {
        jogador = jogadorDetectado.transform;
        float distanciaParaJogador = Vector2.Distance(jogador.position, transform.position);
        float direcaoParaJogador = Mathf.Sign(jogador.position.x - transform.position.x);

        // 3. PRIORIDADE MÁXIMA: LÓGICA DE ATAQUE
        bool podeAtacar = distanciaParaJogador <= alcanceAtaque && timerCooldownAtaque >= cooldownAtaque;

        if (podeAtacar)
        {
            // Garante que o mob esteja virado para o jogador ANTES de atacar
            if (direcao != direcaoParaJogador)
            {
                Virar();
            }
            Atacar();
            timerCooldownAtaque = 0f;
        }
        else // 4. LÓGICA DE PERSEGUIÇÃO (se não for atacar)
        {
            // Vira para seguir o jogador se necessário
            if (direcao != direcaoParaJogador && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
            {
                Virar();
            }
        }
    }
    else // 5. LÓGICA DE PATRULHA (se não detectou jogador)
    {
        jogador = null;

        // VERIFICAÇÃO DE CHÃO
        Vector2 origemRaycast = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
        bool temChao = Physics2D.Raycast(origemRaycast, Vector2.down, 0.5f, chaoLayer);

        if ((!temChao || detectouParede) && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            detectouParede = false; // Reseta o detector de parede após virar
        }
    }

    // 6. ANIMAÇÕES (executado sempre que não está atacando)
    float velocidadeX = Mathf.Abs(rb.linearVelocity.x);
    if (velocidadeX > 0.1f)
    {
        animator.SetBool("walk", true);
        animator.SetBool("idle_1", false);
    }
    else
    {
        animator.SetBool("walk", false);
        animator.SetBool("idle_1", true);
    }
}

    void FixedUpdate()
    {
        // Se estiver atacando, para de andar
        float velocidadeFinal = atacando ? 0f : direcao * velocidade;
        rb.linearVelocity = new Vector2(velocidadeFinal, rb.linearVelocity.y);
    }

    public void SensorParedeDetectou()
    {
        detectouParede = true;
    }

    public void Atacar()
    {
        Debug.Log("🟠 Mob atacando!");
        animator.SetTrigger("skill_1");

        atacando = true;
        Invoke(nameof(FimDoAtaque), 0.5f); // tempo da animação

        if (hitboxAtaque != null)
        {
            hitboxAtaque.AtivarHitbox();
            Invoke(nameof(DesativarHitbox), 0.3f);
        }
        else
        {
            Debug.LogWarning("⚠️ HitboxAtaqueMob não atribuída.");
        }
    }

    private void FimDoAtaque()
    {
        atacando = false;
    }

    private void DesativarHitbox()
    {
        hitboxAtaque?.DesativarHitbox();
    }

    public void ReceberHit()
    {
        animator.SetTrigger("hit_1");
    }

    public void Morrer()
    {
        animator.SetTrigger("death");
    }

    public void Evadir()
    {
        animator.SetTrigger("evade_1");
    }

    private void Virar()
    {
        tempoDesdeUltimaVirada = 0f;
        direcao *= -1f;

        Vector3 escala = transform.localScale;
        escala.x = Mathf.Abs(escala.x) * Mathf.Sign(direcao);
        transform.localScale = escala;
    }

    private void OnDrawGizmosSelected()
    {
        if (peDoMob != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);

            Vector2 origem = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origem, origem + Vector2.down * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, alcanceAtaque);
        }
    }
}
