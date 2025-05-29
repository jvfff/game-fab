using UnityEngine;

public class SensorParede : MonoBehaviour
{
    private MovimentoMob movimentoMob;

    void Start()
    {
        // Pega o componente do pai
        movimentoMob = GetComponentInParent<MovimentoMob>();
        if (movimentoMob == null)
        {
            Debug.LogError("MovimentoMob não encontrado no pai do SensorParede");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Parede") || collision.CompareTag("Limite"))
        {
            movimentoMob.SensorParedeDetectou();
        }
    }
}
