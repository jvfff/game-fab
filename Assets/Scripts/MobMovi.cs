using UnityEngine;

public class MovimentoMob : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float tempoMinimoParaVirar = 0.5f;

    [SerializeField] private float velocidade = 3f;
    [SerializeField] private Transform peDoMob;
    [SerializeField] private LayerMask chaoLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float distanciaDeteccao = 5f;
    [SerializeField] private float alcanceAtaque = 1.5f;         // alcance do ataque
    [SerializeField] private float cooldownAtaque = 2f;           // tempo entre ataques
    [SerializeField] private Transform sensorParede;
    [SerializeField] private HitboxAtaqueMob hitboxAtaque; 

    private float direcao = 1f;
    private float tempoDesdeUltimaVirada = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform jogador;

    private int voandoHash = Animator.StringToHash("voando");
    private int atacandoHash = Animator.StringToHash("atacando");
    private int recebendoHitHash = Animator.StringToHash("recebendoHit");

    private bool detectouParede = false;

    private float timerCooldownAtaque = 0f;     // contador do cooldown

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

        // Detecta jogador
        Collider2D jogadorDetectado = Physics2D.OverlapCircle(transform.position, distanciaDeteccao, playerLayer);
        if (jogadorDetectado != null)
        {
            jogador = jogadorDetectado.transform;
            float distanciaParaJogador = Vector2.Distance(jogador.position, transform.position);

            //Debug.Log("Jogador detectado a distância: " + distanciaParaJogador);
            //Debug.Log($"Distância para jogador: {distanciaParaJogador}, Cooldown: {timerCooldownAtaque}");

            float direcaoDesejada = Mathf.Sign(jogador.position.x - transform.position.x);

            if (direcao != direcaoDesejada && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
            {
                Virar();
                direcao = direcaoDesejada;
            }

            bool podeAtacar = distanciaParaJogador <= alcanceAtaque && timerCooldownAtaque >= cooldownAtaque;
            //Debug.Log($"Condição para atacar: {podeAtacar} (Distância: {distanciaParaJogador} <= {alcanceAtaque}, Cooldown: {timerCooldownAtaque} >= {cooldownAtaque})");

            if (podeAtacar)
            {
                Atacar();
                timerCooldownAtaque = 0f;
            }

        }
        else
        {
            //Debug.Log("Jogador não detectado");
            jogador = null;  // jogador saiu do alcance
        }

        // Verifica chão à frente
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
        animator.SetBool(voandoHash, true);
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
        animator.SetTrigger(atacandoHash);

        if (hitboxAtaque != null)
        {
            hitboxAtaque.AtivarHitbox();
            Invoke(nameof(DesativarHitbox), 0.3f);
        }
        else
        {
            Debug.LogWarning("⚠️ HitboxAtaqueMob não atribuída no Inspector.");
        }
    }

    private void DesativarHitbox()
    {
        hitboxAtaque?.DesativarHitbox();
    }

    public void ReceberHit()
    {
        animator.SetTrigger(recebendoHitHash);
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
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(peDoMob.position, 0.2f);

            Vector2 origem = (Vector2)peDoMob.position + Vector2.right * direcao * 0.3f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origem, origem + Vector2.down * 0.5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);
        }
    }
}
