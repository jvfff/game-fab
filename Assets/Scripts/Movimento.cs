// Movimento.cs
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
        if (!estaAtacando && Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetTrigger(attackHash);
            estaAtacando = true;
            CausarDano(); // Executa durante a animação, ou pode ser chamado via AnimationEvent
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            rb.AddForce(Vector2.up * 600);
        }

        estaNoChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animator.SetBool(movendoHash, horizontalInput != 0);
        animator.SetBool(saltandoHash, !estaNoChao);

        if (horizontalInput > 0) spriteRenderer.flipX = false;
        else if (horizontalInput < 0) spriteRenderer.flipX = true;

        // Aqui adicionamos a inversão do pontoDeAtaque de acordo com o flip do sprite
        if (pontoDeAtaque != null)
        {
            Vector3 localPos = pontoDeAtaque.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * (spriteRenderer.flipX ? -1 : 1);
            pontoDeAtaque.localPosition = localPos;
        }
    }

    private void FixedUpdate()
    {
        if (estaAtacando)
        {
            rb.linearVelocity = new Vector2(horizontalInput * velocidadeAtaque, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(horizontalInput * velocidade, rb.linearVelocity.y);
        }
    }

    private void CausarDano()
    {
        if (pontoDeAtaque == null)
        {
            Debug.LogWarning("Ponto de ataque não atribuído!");
            return;
        }

        Vector2 posAtaque = pontoDeAtaque.position;
        float raioAtaque = 0.5f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(posAtaque, raioAtaque, mobLayer);
        Debug.Log($"CausarDano: {hits.Length} colisores detectados.");

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Detectado: " + hit.gameObject.name);

            MobVida mobVida = hit.GetComponent<MobVida>();
            if (mobVida != null)
            {
                mobVida.TomarDano(1);
                Debug.Log("Dano aplicado ao mob!");
            }
            else
            {
                Debug.Log("MobVida não encontrado nesse objeto.");
            }
        }
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
