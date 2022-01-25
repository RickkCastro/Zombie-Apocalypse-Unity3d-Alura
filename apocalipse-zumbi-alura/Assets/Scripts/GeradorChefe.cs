using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{
    public float TempoProxGeracao = 0f;
    public float TempoEntreGeracoes;
    private ControlaInterface ScriptcontrolaInterface;

    public GameObject BossPrefab;
    public Transform[] SpawnsBoss;

    private Transform Jogador;

    private void Start()
    {
        TempoProxGeracao = TempoEntreGeracoes;
        ScriptcontrolaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        Jogador = GameObject.FindWithTag("Jogador").transform;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > TempoProxGeracao)
        {
            Vector3 Spawn = CalcularMelhorSpawn();
            Instantiate(BossPrefab, Spawn, Quaternion.identity);
            ScriptcontrolaInterface.MostrarTxtBoss();

            TempoProxGeracao = Time.timeSinceLevelLoad + TempoEntreGeracoes + Random.Range(-20, 10);
        }
    }

    Vector3 CalcularMelhorSpawn()
    {
        Vector3 posicaoSpawn = Vector3.zero;
        float MaiorDistancia = 0;

        foreach(Transform posicao in SpawnsBoss)
        {
            float distanciaJogador = Vector3.Distance(posicao.position, Jogador.position);
            if(distanciaJogador > MaiorDistancia)
            {
                MaiorDistancia = distanciaJogador;
                posicaoSpawn = posicao.position;
            }
        }

        return posicaoSpawn;
    }
}
