using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControlaInterface : MonoBehaviour {

    private ControlaJogador scriptControlaJogador;
    public Slider SliderVidaJogador;
    public GameObject PainelDeGameOver;

    public TextMeshProUGUI TextoTempoDeSobrevivencia;
    public TextMeshProUGUI TextoPontuacaoMaxima;
    private float tempoPontuacaoSalvo;

    private int QuantZumbisMortos;
    public TextMeshProUGUI TxtQuantZumbisMortos;

    public Text TextoBoss;

    // Use this for initialization
    void Start () {
        scriptControlaJogador = GameObject.FindWithTag("Jogador")
                                .GetComponent<ControlaJogador>();

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida;
        AtualizarSliderVidaJogador();
        Time.timeScale = 1;
        tempoPontuacaoSalvo = PlayerPrefs.GetFloat("PontuacaoMaxima");
    }

    public void AtualizarSliderVidaJogador ()
    {
        SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
    }

    public void GameOver ()
    {
        PainelDeGameOver.SetActive(true);
        Time.timeScale = 0;

        int minutos = (int)(Time.timeSinceLevelLoad / 60);
        int segundos = (int)(Time.timeSinceLevelLoad % 60);
        TextoTempoDeSobrevivencia.text = 
            "Você sobreviveu por " + minutos + "min e " + segundos + "s";

        AjustarPontuacaoMaxima(minutos, segundos);
    }

    void AjustarPontuacaoMaxima (int min, int seg)
    {
        if(Time.timeSinceLevelLoad > tempoPontuacaoSalvo)
        {
            tempoPontuacaoSalvo = Time.timeSinceLevelLoad;
            TextoPontuacaoMaxima.text = 
                string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalvo);
        }
        if(TextoPontuacaoMaxima.text == "")
        {
            min = (int)tempoPontuacaoSalvo / 60;
            seg = (int)tempoPontuacaoSalvo % 60;
            TextoPontuacaoMaxima.text =
                string.Format("Seu melhor tempo é {0}min e {1}s", min, seg);
        }
    }

    public void Reiniciar ()
    {
        SceneManager.LoadScene("Game");
    }

    public void AtualizarZumbisMortos()
    {
        QuantZumbisMortos++;
        TxtQuantZumbisMortos.text = "x" + QuantZumbisMortos.ToString("000");
    }

    public void MostrarTxtBoss()
    {
        TextoBoss.gameObject.SetActive(true);
        StartCoroutine(DesaparecerTexto(3f, TextoBoss));
    }

    IEnumerator DesaparecerTexto(float TempoNaTela, Text TextoParaSumir)
    {
        TextoParaSumir.gameObject.SetActive(true);
        Color CorTexto = TextoParaSumir.color;
        CorTexto.a = 1;
        TextoParaSumir.color = CorTexto;

        yield return new WaitForSeconds(1);
        
        float contador = 0;

        while(TextoParaSumir.color.a > 0)
        {
            contador += Time.deltaTime / TempoNaTela;
            CorTexto.a = Mathf.Lerp(1, 0, contador);
            TextoParaSumir.color = CorTexto;

            if(TextoParaSumir.color.a <= 0)
            {
                TextoParaSumir.gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
