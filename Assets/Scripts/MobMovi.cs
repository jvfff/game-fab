using UnityEngine;

public class MovimentoMob : MonoBehaviour
{
    private float direcao = 1f;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 3;

    [SerializeField] private Transform peDoMob;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaNoChao;
    private Animator animator;

    private int voandoHash = Animator.StringToHash("voando");
    private int atacandoHash = Animator.StringToHash("atacando");
    private int recebendoHitHash = Animator.StringToHash("recebendoHit");

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        estaNoChao = Physics2D.OverlapCircle(peDoMob.position, 0.2f, chaoLayer);

        // A abelha está sempre voando (você pode ligar/desligar conforme lógica)
        animator.SetBool(voandoHash, true);

        // Virar sprite conforme direção
        spriteRenderer.flipX = direcao < 0;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direcao * velocidade, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parede") || collision.CompareTag("Limite"))
        {
            direcao *= -1f;
        }
    }

    // Método público para disparar ataque, pode ser chamado pela IA ou outro script
    public void Atacar()
    {
        animator.SetTrigger(atacandoHash);
    }

    // Método público para disparar hit, pode ser chamado quando receber dano
    public void ReceberHit()
    {
        animator.SetTrigger(recebendoHitHash);
    }

    private void OnDrawGizmosSelected()
    {
        if (peDoMob != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(peDoMob.position, 0.2f);
        }
    }
}
