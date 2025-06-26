using UnityEngine;

public class ParedeTP : MonoBehaviour
{
    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 novaPos = playerTransform.position;
            novaPos.y = 0;
            playerTransform.position = novaPos;
        }
    }
}
