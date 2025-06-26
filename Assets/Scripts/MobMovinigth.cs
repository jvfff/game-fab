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

        // DETECÇÃO DE JOGADOR
        Collider2D jogadorDetectado = Physics2D.OverlapCircle(transform.position, distanciaDeteccao, playerLayer);
        if (jogadorDetectado != null)
        {
            jogador = jogadorDetectado.transform;
            float distanciaParaJogador = Vector2.Distance(jogador.position, transform.position);

            Debug.Log("👀 Jogador detectado: " + jogador.name + ", distância: " + distanciaParaJogador);

            float direcaoDesejada = Mathf.Sign(jogador.position.x - transform.position.x);
            Debug.Log("Direção desejada: " + direcaoDesejada);

            if (direcao != direcaoDesejada && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
            {
                Virar();
                direcao = direcaoDesejada;
            }

            bool podeAtacar = distanciaParaJogador <= alcanceAtaque && timerCooldownAtaque >= cooldownAtaque;
            Debug.Log("✅ Pode atacar? " + podeAtacar);

            if (podeAtacar)
            {
                Atacar();
                timerCooldownAtaque = 0f;
            }

            // Definir animação de movimento
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
            animator.SetBool("idle_1", false);
        }
        else
        {
            jogador = null;

            // Define como parado
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("idle_1", true);
        }

        // VERIFICAÇÃO DE CHÃO
        Vector2 origemRaycast = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
        bool temChao = Physics2D.Raycast(origemRaycast, Vector2.down, 0.5f, chaoLayer);

        if (!temChao && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            direcao *= -1f;
        }

        if (detectouParede && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            direcao *= -1f;
            detectouParede = false;
        }

        spriteRenderer.flipX = direcao < 0;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direcao * velocidade, rb.linearVelocity.y);
    }

    public void SensorParedeDetectou()
    {
        detectouParede = true;
    }

    public void Atacar()
    {
        Debug.Log("🟠 Mob atacando!");
        animator.SetTrigger("skill_1"); // use "skill_2" se quiser variar

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

    private void DesativarHitbox()
    {
        hitboxAtaque?.DesativarHitbox();
    }

    public void ReceberHit()
    {
        animator.SetTrigger("hit_1"); // ou "hit_2"
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

        if (sensorParede != null)
        {
            Vector3 localPos = sensorParede.localPosition;
            localPos.x *= -1f;
            sensorParede.localPosition = localPos;
        }
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
