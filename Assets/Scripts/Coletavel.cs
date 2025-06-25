using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public GameObject hitbox;
    public enum TipoColetavel
    {
        Estrela,
        Pocao
    }

    public TipoColetavel tipo;
    public SpriteRenderer spriteRenderer;
    public Sprite spriteEstrela;
    public Sprite spritePocao;

    private void Start()
    {
        // Troca sprite baseado no tipo
        switch (tipo)
        {
            case TipoColetavel.Estrela:
                spriteRenderer.sprite = spriteEstrela;
                break;
            case TipoColetavel.Pocao:
                spriteRenderer.sprite = spritePocao;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (tipo)
        {
            case TipoColetavel.Estrela:
                Debug.Log("Pegou uma estrela!");
                // Adicionar ao contador de estrelas, por exemplo:
                // player.ColetarEstrela();
                break;

            case TipoColetavel.Pocao:
                Debug.Log("Pegou uma poção!");
                other.GetComponent<PlayerStats>().potion += 1;
                break;
        }

        Destroy(gameObject); // Destroi o item coletado
    }
}
