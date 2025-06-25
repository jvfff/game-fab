using UnityEngine;
using UnityEngine.SceneManagement;

public class passarFase : MonoBehaviour
{
    [SerializeField] public int indexT = 0;
    public Index index;
    private void Awake()
    {
        index = GetComponent<Index>();
        index.indexV = indexT;  
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colidiu");
        if (other.CompareTag("Player"))
        {
            Debug.Log("e passou");
            SceneManager.LoadScene("LoadingScene");
        }
    }


}
