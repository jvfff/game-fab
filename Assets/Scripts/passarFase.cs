using UnityEngine;
using UnityEngine.SceneManagement;

public class passarFase : MonoBehaviour
{
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
