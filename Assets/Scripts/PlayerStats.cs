using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int vida = 100;
    public bool IsAlive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool IsDamage;

    private float logTimer = 0f;

    void Update()
    {
        
        // Evita log spamming a cada frame
        logTimer += Time.deltaTime;
        if (logTimer >= 1f)

        // Checa morte
        if (vida <= 0 && IsAlive)
        {
            Death();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.K)) { vida = vida - 50; }
        Console.WriteLine(vida.ToString());
        if (vida <= 0) { Death(); }

        vida -= damage;
        if (!IsDamage) // impede m�ltiplas coroutines em paralelo
        {
            StartCoroutine(Delay());
        }
    }

    public void Death()
    {
        Debug.Log("Jogador morreu.");
        IsAlive = false;
        // Aqui voc� pode adicionar l�gica de desativar controles, anima��es, etc.
    }

    private IEnumerator Delay()
    {
        IsDamage = true;
        Debug.Log("Dano recebido. Esperando 1 segundo...");
        yield return new WaitForSeconds(1f);
        IsDamage = false;
        Debug.Log("Jogador pode agir novamente.");
    }
}
