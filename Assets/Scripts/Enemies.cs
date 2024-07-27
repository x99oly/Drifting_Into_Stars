using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class Enemys : MonoBehaviour
{
    // Definidas na Unity
    public float vida = 30f;
    public float velocidade = 10.0f;
    public float danoDoDisparo = 5f;
    public float cadenciaDeDisparos = 10.0f;
    public float resistenciaVeneno = 10f;

    public GameObject projetil;
    public GameObject arma;
    public GameObject jogador;

    // Definidas em código
    protected Rigidbody inimigoRB;
    protected float vidaAtual;
    protected float dano;
    protected float intervalo;

    protected bool podeMovimentar = true;
    protected bool estaEnvenenado = false;

    protected virtual void Awake()
    {
        // Inicialização das variáveis
        vidaAtual = vida;
        dano = danoDoDisparo;
        intervalo = cadenciaDeDisparos;

        inimigoRB = GetComponent<Rigidbody>();

        // Atribui o jogador encontrado com a tag "Player"
        jogador = GameObject.FindWithTag("Player");
    }



    protected virtual void Update()
    {

        GameManager.ManterNaTela(gameObject);

    }

    protected virtual void FixedUpdate()
    {
        MesmoZDoJogador();
        VirarParaJogador();
        if(podeMovimentar) Mover();

        intervalo -= Time.deltaTime;
        if(intervalo < 0)
        {
            Atirar();
            intervalo = cadenciaDeDisparos;
        }
    }
    protected virtual void Mover()
    {

        float moveX = 1f;
        float moveY = 0f;

        Vector3 movimento = new Vector3(moveX * -1, moveY) * velocidade;

        transform.Translate(movimento * Time.deltaTime);
    }

    protected virtual void Atirar()
    {
        GameManager.Atirar(gameObject, arma, projetil);
    }

    protected void VirarParaJogador()
    {
        Vector3 posAtual = transform.position;
        Vector3 posicaoJogador = jogador.transform.position;

        Vector3 distancia = posAtual - posicaoJogador;
        distancia.z = 0f;

        float angulo = Mathf.Atan2(distancia.y, distancia.x) * Mathf.Rad2Deg + 90f;

        Quaternion novaRotacao = Quaternion.Euler(new Vector3(0, 0, angulo));
        inimigoRB.MoveRotation(novaRotacao);
    }

   protected void MesmoZDoJogador()
    {
        Vector3 escalaDoJogador = jogador.transform.position;

        transform.position = new Vector3(transform.position.x, transform.position.y, escalaDoJogador.z);
    }

}
