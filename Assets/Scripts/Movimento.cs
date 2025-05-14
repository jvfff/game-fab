using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb; 

    [SerializeField] private int velocidade = 5;

    [SerializeField]private Transform peDoPersonagem;
    [SerializeField]private LayerMask chaoLayer;

    private bool estaNoChao;
    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");

    private SpriteRenderer spriteRenderer;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * velocidade, rb.linearVelocity.y);
    }
}
