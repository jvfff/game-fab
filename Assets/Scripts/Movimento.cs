using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 5;
    [SerializeField] private int velocidadeAtaque = 2;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    [SerializeField] private LayerMask mobLayer;

    [SerializeField] private Transform pontoDeAtaque;

    private bool estaNoChao;
    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int attackHash = Animator.StringToHash("atacando");

    private bool estaAtacando = false;
    private bool danoJaAplicado = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ataque
        if (!estaAtacando && Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger(attackHash);
            estaAtacando = true;
            danoJaAplicado = false;
        }

        // Movimento lateral
        horizontalInput = Input.GetAxis("Horizontal");

        // Virar player (flip completo no eixo X)
        if (horizontalInput != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Sign(horizontalInput) * Mathf.Abs(escala.x);
            transform.localScale = escala;
        }

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        // Checar chão
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        // Animações
        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);
    }

    void FixedUpdate()
    {
        float velocidadeAtual = estaAtacando ? velocidadeAtaque : velocidade;
        rb.linearVelocity = new Vector2(horizontalInput * velocidadeAtual, rb.linearVelocity.y);
    }

    public void TerminarAtaque()
    {
        animator.ResetTrigger(attackHash);
        estaAtacando = false;
        danoJaAplicado = false;
    }

    // ⚔️ Chamado pelo Animation Event
    public void AtivarDano()
    {
        if (danoJaAplicado) return;

        Collider2D[] inimigosAcertados = Physics2D.OverlapCircleAll(pontoDeAtaque.position, 0.5f, mobLayer);

        if (inimigosAcertados.Length == 0)
        {
            Debug.Log("Nenhum inimigo na área de ataque");
        }

        foreach (Collider2D inimigo in inimigosAcertados)
        {
            Debug.Log($"🟥 Inimigo atingido: {inimigo.name}");

            var script = inimigo.GetComponent<Inimigo>();
            if (script != null)
            {
                Debug.Log("✅ Script Inimigo encontrado, aplicando dano");
                script.TomarDano(1);
            }
            else
            {
                Debug.LogWarning("⚠️ Script Inimigo não encontrado no objeto atingido");
            }
        }

        danoJaAplicado = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (pontoDeAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pontoDeAtaque.position, 0.5f);
        }
    }
}
