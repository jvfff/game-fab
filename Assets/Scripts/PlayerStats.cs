using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int vida = 100;
    public bool IsAlive = true;
    public bool IsDamage;
    private Parry parryRef;
    public int damage = 30;
    private float logTimer = 0f;


    private void Awake()
    {
        parryRef = GetComponent<Parry>();
    }

    void Update()
    {
        // Teste de dano
        if (Input.GetKeyDown(KeyCode.K))
        {
            TomeiDano(20);
        }

        // Evita log spamming a cada frame
        logTimer += Time.deltaTime;
        if (logTimer >= 1f)
        {
            Debug.Log($"Vida: {vida} | IsDamage: {IsDamage}");
            logTimer = 0f;
        }

        // Checa morte
        if (vida <= 0 && IsAlive)
        {
            Death();
        }

        if (parryRef.buffar)
        {
            IsBuffed();
        }else { NormalState(); }
    }

    public void TomeiDano(int damage)
    {
        if (!IsAlive) return;

        vida -= damage;
        if (!IsDamage) // impede múltiplas coroutines em paralelo
        {
            StartCoroutine(Delay());
        }
    }

    public void Death()
    {
        Debug.Log("Jogador morreu.");
        IsAlive = false;
        // Aqui você pode adicionar lógica de desativar controles, animações, etc.
    }

    private IEnumerator Delay()
    {
        IsDamage = true;
        Debug.Log("Dano recebido. Esperando 1 segundo...");
        yield return new WaitForSeconds(1f);
        IsDamage = false;
        Debug.Log("Jogador pode agir novamente.");
    }

    private void IsBuffed()
    {
        damage = damage + 20;
    }

    private void NormalState()
    {
        damage = 30;
    }
}
