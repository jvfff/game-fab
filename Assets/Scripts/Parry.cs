using UnityEngine;

public class Parry : MonoBehaviour
{
    public Movimento movimentoRef; // Referência ao script do personagem
    private Collider2D ParryCollider;
    public bool Acaba;
    public bool buffar = false;

    private void Awake()
    {
       
        ParryCollider = GetComponent<Collider2D>(); // ou GetComponentInChildren se necessário
        ParryCollider.enabled = false; // começa desativado
        
    }

    private void Update()
    {
        if (!movimentoRef.estaAtacando &&  Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            parry();
        }
        if (Acaba || Input.GetKeyDown(KeyCode.I)) { Fparry(); }

        OnTriggerEnter2D(ParryCollider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ataque"))
        {
            Debug.Log("Parry ativado!");
            // Ex: movimentoRef.ParryBemSucedido();
        }
    }

    public void parry()
    {
        ParryCollider.enabled = true;
        movimentoRef.NoParry = true;
        buffar = true;
    }

    // Chamado via Animation Event
    public void Fparry()
    {
        movimentoRef.NoParry = false;
        ParryCollider.enabled = false;
        buffar = false;
    }
}
