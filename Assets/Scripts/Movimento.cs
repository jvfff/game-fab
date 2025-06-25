// Movimento.cs
using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;
    public int dano = 1;

    [SerializeField] private int velocidade = 5;
    [SerializeField] private int velocidadeAtaque = 2;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    [SerializeField] private LayerMask mobLayer;

    [SerializeField] private Transform pontoDeAtaque; // Agora usando ponto de ataque visual

    private bool estaNoChao;
    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int attackHash = Animator.StringToHash("atacando");

    private SpriteRenderer spriteRenderer;
    private bool estaAtacando = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Input de ataque - só se não estiver atacando
       if (!estaAtacando && Input.GetKeyDown(KeyCode.Z))
{
    animator.SetTrigger(attackHash);
    estaAtacando = true;

    // Detecta inimigos na área de ataque
    Collider2D[] inimigosAcertados = Physics2D.OverlapCircleAll(pontoDeAtaque.position, 0.5f, mobLayer);
    foreach (Collider2D inimigo in inimigosAcertados)
    {
        inimigo.GetComponent<Inimigo>()?.TomarDano(dano);
    }
}


        // Movimento lateral (permitido sempre)
        horizontalInput = Input.GetAxis("Horizontal");

        // Pulo (permitido mesmo durante ataque)
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        // Virar sprite conforme direção
        if (horizontalInput > 0)
            spriteRenderer.flipX = false;
        else if (horizontalInput < 0)
            spriteRenderer.flipX = true;
    }

    void FixedUpdate()
    {
        // Movimento com velocidade reduzida durante ataque
        float velocidadeAtual = estaAtacando ? velocidadeAtaque : velocidade;
        rb.linearVelocity = new Vector2(horizontalInput * velocidadeAtual, rb.linearVelocity.y);
    }

    public void TerminarAtaque()
    {
        animator.ResetTrigger(attackHash);
        estaAtacando = false;
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
