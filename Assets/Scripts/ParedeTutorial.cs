using UnityEngine;

public class ParedeTutorial : MonoBehaviour
{
    [SerializeField] public int index;
    private GameManager manager;

    private void Awake()
    {
        manager = FindAnyObjectByType<GameManager>();  // CORRIGIDO
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && manager != null)
        {
            manager.SetTutorialIndex(index);  // usa a função certa
        }
    }
}
