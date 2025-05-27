using UnityEngine;

public class TesteAnimaçõesMob : MonoBehaviour
{
    [SerializeField] private MovimentoMob mob;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) // tecla A para atacar
        {
            mob.Atacar();
        }

        if (Input.GetKeyDown(KeyCode.H)) // tecla H para receber hit
        {
            mob.ReceberHit();
        }
    }
}
