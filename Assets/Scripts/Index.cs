using UnityEngine;

public class Index : MonoBehaviour
{
    public static Index Instance;
    public int indexFase = 0; // 0 = Floresta, 1 = Caverna, 2 = Deserto

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persiste entre cenas
        }
        else
        {
            Destroy(gameObject); // evita duplicados
        }
    }
}
