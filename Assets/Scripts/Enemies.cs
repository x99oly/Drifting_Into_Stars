using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public abstract class Enemys : MonoBehaviour
{
    //Definidas na Unity
    public float vida = 30f, velocidade = 10.0f, danoDoDisparo = 5f, cadenciaDeDisparos = 10.0f;
    public GameObject projetil, arma, jogador;

    //Definidas em código
    Rigidbody inimigoRB;
    float vidaAtual, dano, intervalo;
    bool podeMovimentar = true;

    protected virtual void Awake()
    {
        vidaAtual = vida;
        dano = danoDoDisparo;
        intervalo = cadenciaDeDisparos;
        
        inimigoRB = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {

        GameManager.ManterNaTela(gameObject);

    }

    protected virtual void FixedUpdate()
    {
        VirarParaJogador();

        intervalo -= Time.deltaTime;
        if(intervalo < 0)
        {
            Atirar();
            intervalo = cadenciaDeDisparos;
        }
    }

    

    protected virtual void Atirar()
    {
        GameManager.Atirar(arma, projetil);
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

    
}
