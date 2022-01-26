using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour {

    public float Velocidade = 20;
    private Rigidbody rigidbodyBala;
    public AudioClip SomDeMorte;

    private void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void FixedUpdate () {
        rigidbodyBala.MovePosition
            (rigidbodyBala.position + 
            transform.forward * Velocidade * Time.deltaTime);
	}

    void OnTriggerEnter(Collider objetoDeColisao)
    {
        Quaternion rotacaoOpostaBala = Quaternion.LookRotation(-transform.forward);
        switch(objetoDeColisao.tag)
        {
            case "Inimigo":
                ControlaInimigo inimigo = objetoDeColisao.GetComponent<ControlaInimigo>();
                inimigo.TomarDano(1);
                inimigo.ParticulaSangue(transform.position, rotacaoOpostaBala);
                break;
            case "Boss":
                ControlaBoss boss = objetoDeColisao.GetComponent<ControlaBoss>();
                boss.TomarDano(1);
                boss.ParticulaSangue(transform.position, rotacaoOpostaBala);
                break;
        }

        if (objetoDeColisao.tag != "Gerador")
            Destroy(gameObject);
    }
}
