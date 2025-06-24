using UnityEngine;

public class HitboxAtaqueMob : MonoBehaviour
{
    [SerializeField] private int dano = 1;
    private bool ativo = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entrou algo: " + other.name); // Já apareceu, ótimo

        if (!ativo)
        {
            Debug.Log("⚠️ Hitbox está inativa, ignorando colisão.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player detectado, tentando aplicar dano.");

            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TomarDano(dano);
                Debug.Log("💥 Dano aplicado ao jogador!");
            }
            else
            {
                Debug.LogWarning("❌ PlayerStats.cs não encontrado no objeto: " + other.name);
            }
        }
    }

    public void AtivarHitbox()
    {
        ativo = true;
        gameObject.SetActive(true);
        Debug.Log("🟥 Hitbox ativada");
    }

    public void DesativarHitbox()
    {
        ativo = false;
        gameObject.SetActive(false);
        Debug.Log("⬛ Hitbox desativada");
    }
}
