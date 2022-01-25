using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaInimigo : MonoBehaviour, IMatavel
{

    public GameObject Jogador;
    public AudioClip SomDeMorte;
    public GameObject KitMedicoPrefab;

    private MovimentoPersonagem movimentaInimigo;
    private AnimacaoPersonagem animacaoInimigo;
    private Status statusInimigo;
    private Vector3 posicaoAleatoria;
    private Vector3 direcao;

    private float contadorVagar = 0f;
    private float TempoEntrePosicoesAleatorias = 4f;

    private float ChanceGerrarKitMedico = 0.08f;

    [HideInInspector]
    public GeradorZumbis MeuGerrador;

    private ControlaInterface ScriptControlaInterface;

	// Use this for initialization
	void Start () {
        Jogador = GameObject.FindWithTag("Jogador");
        animacaoInimigo = GetComponent<AnimacaoPersonagem>();
        movimentaInimigo = GetComponent<MovimentoPersonagem>();
        AleatorizarZumbi();
        statusInimigo = GetComponent<Status>();

        ScriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
    }

    void FixedUpdate()
    {
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);

        if(distancia > 18) //Nao vendo o jogador
        {
            Vagar ();
        }
        else if (distancia > 2.5) //vendo o jogador
        {
            direcao = Jogador.transform.position - transform.position;

            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);

            animacaoInimigo.Atacar(false);
        }
        else //Atacando
        {
            direcao = Jogador.transform.position - transform.position;

            animacaoInimigo.Atacar(true);
        }

        movimentaInimigo.Rotacionar(direcao);
        animacaoInimigo.Movimentar(direcao.magnitude);
    }

    void Vagar ()
    {
        contadorVagar -= Time.deltaTime;

        if(contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao();
            contadorVagar += (TempoEntrePosicoesAleatorias + Random.Range(-2f, 2f));
        }

        bool ficouPertoOSuficiente = Vector3.Distance(transform.position, posicaoAleatoria) <= 0.05;

        if (ficouPertoOSuficiente == false)
        {
            direcao = posicaoAleatoria - transform.position;
            movimentaInimigo.Movimentar(direcao, statusInimigo.Velocidade);
            //Debug.Log(direcao);
        }           
    }

    Vector3 AleatorizarPosicao ()
    {
        Vector3 posicao = Random.insideUnitSphere * 10;
        posicao += transform.position;
        posicao.y = transform.position.y;

        return posicao;
    }

    void AtacaJogador ()
    {
        int dano = Random.Range(20, 30);
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    void AleatorizarZumbi ()
    {
        int geraTipoZumbi = Random.Range(1, transform.childCount);
        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true);
    }

    public void TomarDano(int dano)
    {
        statusInimigo.Vida -= dano;
        if(statusInimigo.Vida <= 0)
        {
            Morrer();
        }
    }

    public void Morrer()
    {
        animacaoInimigo.Morrer();
        movimentaInimigo.Morrer(0.6f);
        Destroy(gameObject, 2);
        this.enabled = false;

        ControlaAudio.instancia.PlayOneShot(SomDeMorte);
        VerrificarGerKitMedico(ChanceGerrarKitMedico);
        ScriptControlaInterface.AtualizarZumbisMortos();
        MeuGerrador.DiminuirQuantZumbisVivos();
    }

    private void VerrificarGerKitMedico(float Chance)
    {
        if(Random.value <= Chance)
        {
            Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
        }
    }
}
