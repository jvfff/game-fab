using UnityEngine;

public class MovimentoMob : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float velocidade = 3f;
    [SerializeField] private Transform peDoMob;
    [SerializeField] private LayerMask chaoLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float distanciaDeteccao = 5f;
    [SerializeField] private float tempoMinimoParaVirar = 0.5f;
    [SerializeField] private Transform sensorParede; // <- Adicionado

    private float direcao = 1f;
    private float tempoDesdeUltimaVirada = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform jogador;

    private int voandoHash = Animator.StringToHash("voando");
    private int atacandoHash = Animator.StringToHash("atacando");
    private int recebendoHitHash = Animator.StringToHash("recebendoHit");

    private bool detectouParede = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        tempoDesdeUltimaVirada += Time.deltaTime;

        // Detecta jogador próximo
        Collider2D jogadorDetectado = Physics2D.OverlapCircle(transform.position, distanciaDeteccao, playerLayer);
        if (jogadorDetectado != null)
        {
            jogador = jogadorDetectado.transform;
            float direcaoDesejada = Mathf.Sign(jogador.position.x - transform.position.x);

            if (direcao != direcaoDesejada && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
            {
                Virar();
                direcao = direcaoDesejada;
            }
        }

        // Verifica se tem chão à frente
        Vector2 origemRaycast = (Vector2)peDoMob.position + new Vector2(direcao * 0.3f, 0);
        bool temChao = Physics2D.Raycast(origemRaycast, Vector2.down, 0.5f, chaoLayer);

        if (!temChao && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            direcao *= -1f;
        }

        // Sensor parede detectou algo
        if (detectouParede && tempoDesdeUltimaVirada >= tempoMinimoParaVirar)
        {
            Virar();
            direcao *= -1f;
            detectouParede = false;
        }

        // Flip do sprite
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
        animator.SetTrigger(atacandoHash);
    }

    public void ReceberHit()
    {
        animator.SetTrigger(recebendoHitHash);
    }

    private void Virar()
    {
        tempoDesdeUltimaVirada = 0f;

        // Inverter a posição local do sensorParede
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
