using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi;
    public LayerMask LayerZumbi;
    private GameObject jogador;

    public float TempoGerarZumbi = 1;
    private float contadorTempo = 0;

    public float distanciaDeGeracao = 3;
    public float DistanciaDoJogadorParaGeracao = 20;

    public float MaxZumbisVivos;
    private int QuantZumbisVivos;

    public float TempoProxDificuldade;
    private float ContadorDeAumentarDificuldade;

    private void Start()
    {
        jogador = GameObject.FindWithTag("Jogador");
        ContadorDeAumentarDificuldade = TempoProxDificuldade;

        bool PossoGerrarZumbisPelaDist =
            Vector3.Distance(transform.position, jogador.transform.position) > DistanciaDoJogadorParaGeracao;

        for (int i = 0; i < MaxZumbisVivos; i++)
        {
            if(PossoGerrarZumbisPelaDist)
                StartCoroutine(GerarNovoZumbi());
        }
    }

    // Update is called once per frame
    void Update () {

        bool PossoGerrarZumbisPelaDist =
            Vector3.Distance(transform.position, jogador.transform.position) > DistanciaDoJogadorParaGeracao;

        if (PossoGerrarZumbisPelaDist && QuantZumbisVivos < MaxZumbisVivos)
        {
            contadorTempo += Time.deltaTime;

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0;
            }
        }
        
        if(Time.timeSinceLevelLoad >= ContadorDeAumentarDificuldade)
        {
            MaxZumbisVivos++;
            ContadorDeAumentarDificuldade = Time.timeSinceLevelLoad + TempoProxDificuldade;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeGeracao);
    }

    IEnumerator GerarNovoZumbi ()
    {
        Vector3 posicaoDeCriacao = AleatorizarPosicao();
        Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);

        while(colisores.Length > 0)
        {
            posicaoDeCriacao = AleatorizarPosicao();
            colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi);
            yield return null;
        }

        if (Zumbi.TryGetComponent(out ControlaInimigo zumbi))
        {
            zumbi = Instantiate(Zumbi, posicaoDeCriacao, transform.rotation).GetComponent<ControlaInimigo>();
            zumbi.MeuGerrador = this;
            QuantZumbisVivos++;
        }
        else
            Instantiate(Zumbi, posicaoDeCriacao, transform.rotation);
    }

    Vector3 AleatorizarPosicao ()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao;
        posicao += transform.position;
        posicao.y = 0;

        return posicao;
    }

    public void DiminuirQuantZumbisVivos()
    {
        QuantZumbisVivos--;
    }
}
