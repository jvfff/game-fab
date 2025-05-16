using UnityEngine;

public class Parry : MonoBehaviour
{
    public Movimento movimentoRef; // Refer�ncia ao script do personagem
    private Collider2D ParryCollider;
    public bool Acaba;
  

    private void Awake()
    {
       
        ParryCollider = GetComponent<Collider2D>(); // ou GetComponentInChildren se necess�rio
        ParryCollider.enabled = false; // come�a desativado
    }

    private void Update()
    {
        if (!movimentoRef.estaAtacando &&  Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            parry();
        }
        if (Acaba || Input.GetKeyDown(KeyCode.I)) { Fparry(); }
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
    }

    // Chamado via Animation Event
    public void Fparry()
    {
        movimentoRef.NoParry = false;
        ParryCollider.enabled = false;
    }
}
