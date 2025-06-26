using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider barraDeProgresso;
    public Text textoPorcentagem;

    void Start()
    {
        switch (Index.Instance.indexFase)
        {
            case 0:
                CarregarCena("Caverna");
                break;
            case 1:
                CarregarCena("Deserto");
                break;
            case 2:
                CarregarCena("TelaFinal");
                break;
        }

        Index.Instance.indexFase++; // avança para a próxima
    }

    public void CarregarCena(string nomeCena)
    {
        StartCoroutine(CarregarCenaAsync(nomeCena));
    }

    IEnumerator CarregarCenaAsync(string nomeCena)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeCena);

        while (!operacao.isDone)
        {
            float progresso = Mathf.Clamp01(operacao.progress / 0.9f);
            barraDeProgresso.value = progresso;

            if (textoPorcentagem != null)
                textoPorcentagem.text = (progresso * 100f).ToString("F0") + "%";

            yield return null;
        }
    }
}
