using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kitMedico : MonoBehaviour
{
    public int QuantCura;
    public float TempoDeDestruicao;

    private void Start()
    {
        Destroy(gameObject, TempoDeDestruicao);
    }

    private void OnTriggerEnter(Collider ObjetoColisao)
    {
        if(ObjetoColisao.tag == "Jogador")
        {
            Destroy(gameObject);
            ObjetoColisao.GetComponent<ControlaJogador>().CurarVida(QuantCura);
        }
    }
}
