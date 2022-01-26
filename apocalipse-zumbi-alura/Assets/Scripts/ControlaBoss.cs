using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaBoss : MonoBehaviour, IMatavel
{
    private Transform jogador;
    private NavMeshAgent navAgent;
    private Status statusBoss;
    private AnimacaoPersonagem animBoss;
    private MovimentoPersonagem movimentoBoss;
    
    public GameObject KitMedico;
    public Slider BarraDeVida;
    public Image ImgBarraDeVida;
    public Color CorVidaMax, CorVidaMin;
    public GameObject ZombieBlood;

    private void Start()
    {
        jogador = GameObject.FindWithTag("Jogador").transform;
        navAgent = GetComponent<NavMeshAgent>();
        statusBoss = GetComponent<Status>();
        animBoss = GetComponent<AnimacaoPersonagem>();
        movimentoBoss = GetComponent<MovimentoPersonagem>();
        
        AtualizarInterface();

        navAgent.speed = statusBoss.Velocidade;
    }

    private void Update()
    {
        navAgent.SetDestination(jogador.position);
        animBoss.Movimentar(navAgent.velocity.magnitude);

        if (navAgent.hasPath)
        {
            bool PertoDoJogador = Vector3.Distance(transform.position, jogador.position) <= navAgent.stoppingDistance;
            if (PertoDoJogador)
            {
                Vector3 direcao = jogador.position - transform.position;
                movimentoBoss.Rotacionar(direcao);
                animBoss.Atacar(true);
            }
            else
            {
                animBoss.Atacar(false);
            }
        }
    }

    public void AtacaJogador()
    {
        int dano = Random.Range(30, 40);
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);
    }

    public void TomarDano(int dano)
    {
        statusBoss.Vida -= dano;
        AtualizarInterface();

        if(statusBoss.Vida <= 0)
        {
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ZombieBlood, posicao, rotacao);
    }

    public void Morrer()
    {
        Instantiate(KitMedico, transform.position, Quaternion.identity);

        animBoss.Morrer();
        movimentoBoss.Morrer(0.6f);
        this.enabled = false;
        navAgent.enabled = false;
        Destroy(gameObject, 2);
    }
    private void AtualizarInterface()
    {
        BarraDeVida.maxValue = statusBoss.VidaInicial;
        BarraDeVida.value = statusBoss.Vida;
        float PorcentagemDaVida = (float)statusBoss.Vida / statusBoss.VidaInicial;

        Color CorVida = Color.Lerp(CorVidaMin, CorVidaMax, PorcentagemDaVida);
        ImgBarraDeVida.color = CorVida;
    }
}
