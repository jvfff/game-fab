using Unity.VisualScripting;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rb;

    [SerializeField] private int velocidade = 5;
    [SerializeField] private int velocidadeAtaque = 2;  // velocidade reduzida no ataque

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;
    /*
    public Collider2D ParryCollider;
    private int ParryHash = Animator.StringToHash("parry");*/

    private bool estaNoChao;
    private Animator animator;

    private int movendoHash = Animator.StringToHash("movendo");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int attackHash = Animator.StringToHash("atacando");
    private int ParryHash = Animator.StringToHash("parry");


    public Parry parryRef;
    private SpriteRenderer spriteRenderer;

    public bool estaAtacando = false;
    public bool NoParry = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        parryRef = GetComponent<Parry>();
    }

    void Update()
    {
        // Input de ataque - só se não estiver atacando
        if (!estaAtacando && !NoParry && Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))//O mouse0 é o botao direito no mouse ai vai da escolha oq preferem manter
        {
            animator.SetTrigger(attackHash);
            estaAtacando = true;
            return; // não executa mais ações nesse frame
        }

        if (NoParry)
        {
            //animator.SetTrigger(ParryHash);
            Debug.Log("Foi no movimento");
            return;
            
        }

    // Movimento lateral
    horizontalInput = Input.GetAxis("Horizontal");

        // Pulo permitido mesmo durante ataque (se quiser bloquear, coloque condição aqui)
        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        // Checagem de chão
        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        // Animações de movimento e pulo
        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        // Virar sprite conforme direção
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (estaAtacando || NoParry)
        {
            // Durante ataque, reduz a velocidade de movimento para dar fluidez
            rb.linearVelocity = new Vector2(horizontalInput * velocidadeAtaque, rb.linearVelocity.y);
        }
        else
        {
            // Movimento normal
            rb.linearVelocity = new Vector2(horizontalInput * velocidade, rb.linearVelocity.y);
        }
    }

    // Chamado via Animation Event no final da animação de ataque
    public void TerminarAtaque()
    {
        animator.ResetTrigger(attackHash);
        estaAtacando = false;
    }

    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
         if(collision.CompareTag("Ataque"))
          {
              Debug.Log("Parry Foi")
          }
      }*/
  
  
    }
