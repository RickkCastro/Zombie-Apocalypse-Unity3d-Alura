using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorBoss : MonoBehaviour
{
    public float DitanciaJogadorGerracao;

    public float TempoMaxGeracoes;
    public float TempoMinGeracoes;

    private ControlaInterface ScriptcontrolaInterface;

    public GameObject BossPrefab;
    public Transform[] SpawnsBoss;

    private Transform Jogador;

    private void Start()
    {
        ScriptcontrolaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
        Jogador = GameObject.FindWithTag("Jogador").transform;

        StartCoroutine(GerrarBoss());
    }

    IEnumerator GerrarBoss()
    {
        int MargemInicial = 10;
        while (true)
        {
            float TempoProxGeracao = Random.Range(TempoMinGeracoes + MargemInicial, TempoMaxGeracoes);
            MargemInicial = 0;
            yield return new WaitForSeconds(TempoProxGeracao);

            Vector3 Spawn = AleatorizarSpawn();
            Instantiate(BossPrefab, Spawn, Quaternion.identity);
            ScriptcontrolaInterface.MostrarTxtBoss();
        }
    }

    private void Update()
    {
        if (TempoMaxGeracoes <= TempoMinGeracoes)
            TempoMaxGeracoes = TempoMinGeracoes;
        else
            TempoMaxGeracoes -= Time.deltaTime / 6;
    }

    //private bool ICanSee(GameObject gameObject)
    //{
    //    Collider colliderSpawn = gameObject.GetComponent<Collider>();
    //    Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    //    return GeometryUtility.TestPlanesAABB(planes, colliderSpawn.bounds);
    //}

    Vector3 AleatorizarSpawn()
    {
        Vector3 posicaoSpawn = Vector3.zero;
        List<Transform> PossiveisSpawn = new List<Transform>();

        foreach(Transform Spawn in SpawnsBoss)
        {
            if(Vector3.Distance(Spawn.position, Jogador.position) > DitanciaJogadorGerracao)
            {
                PossiveisSpawn.Add(Spawn);
            }
        }

        int NumSpawn = Random.Range(0, PossiveisSpawn.Count);
        posicaoSpawn = PossiveisSpawn[NumSpawn].position;

        return posicaoSpawn;
    }
}
