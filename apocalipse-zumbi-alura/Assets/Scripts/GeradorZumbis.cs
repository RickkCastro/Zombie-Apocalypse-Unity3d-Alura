using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi;
    public LayerMask LayerZumbi;
    private GameObject jogador;

    public float TempoMinGerarZumbi;
    public float TempoMaxGerarZumbi;
    
    private float TempoGerarZumbi;
    private float contadorTempo;

    public float distanciaDeGeracao;

    public float MaxZumbisVivos;
    private int QuantZumbisVivos;

    public float TempoProxDificuldade;
    private float ContadorDeAumentarDificuldade;

    private SphereCollider sphereCollider;

    private void Start()
    {
        jogador = GameObject.FindWithTag("Jogador");
        ContadorDeAumentarDificuldade = TempoProxDificuldade;

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = distanciaDeGeracao;

        for (int i = 0; i < MaxZumbisVivos; i++)
        {
            if (!ICanSee())
                StartCoroutine(GerarNovoZumbi());
        }

        TempoGerarZumbi = Random.Range(TempoMinGerarZumbi, TempoMaxGerarZumbi);
    }

    private bool ICanSee()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, sphereCollider.bounds);
    }

    // Update is called once per frame
    void Update () 
    {

        if (!ICanSee() && QuantZumbisVivos < MaxZumbisVivos)
        {
            contadorTempo += Time.deltaTime;

            if (contadorTempo >= TempoGerarZumbi)
            {
                StartCoroutine(GerarNovoZumbi());
                TempoGerarZumbi = Random.Range(TempoMinGerarZumbi, TempoMaxGerarZumbi);
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
