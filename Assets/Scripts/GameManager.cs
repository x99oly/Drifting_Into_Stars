using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float danoBaseDosTiros = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ManterNaTela(GameObject personagem)
    {
        Vector3 posicaoDoPersonagem = personagem.transform.position;
        Vector3 posicaoDoPersonagemNaTela = Camera.main.WorldToViewportPoint(posicaoDoPersonagem);
        if (posicaoDoPersonagem.z != 1)
        {
            posicaoDoPersonagem.z = 1;
        }

        if (posicaoDoPersonagemNaTela.x < 0)
        {
            posicaoDoPersonagemNaTela.x = 0.9f;
        }
        else if (posicaoDoPersonagemNaTela.x > 1)
        {
            posicaoDoPersonagemNaTela.x = 0.1f;
        }

        if (posicaoDoPersonagemNaTela.y < 0)
        {
            posicaoDoPersonagemNaTela.y = 0.9f;
        }
        else if (posicaoDoPersonagemNaTela.y > 1)
        {
            posicaoDoPersonagemNaTela.y = 0.1f;
        }

        posicaoDoPersonagem = Camera.main.ViewportToWorldPoint(posicaoDoPersonagemNaTela);
        posicaoDoPersonagem.z = personagem.transform.position.z;

        personagem.transform.position = posicaoDoPersonagem;
    }
    public static void DestruirAoSairDaTela(GameObject personagem)
    {
        Vector3 posicaoDoPersonagem = personagem.transform.position;

        Vector3 posicaoDoPersonagemNaTela = Camera.main.WorldToViewportPoint(personagem.transform.position);


        if (posicaoDoPersonagem.z != 1)
        {
            posicaoDoPersonagem.z = 1;
        }

        if (posicaoDoPersonagemNaTela.x < 0 || posicaoDoPersonagemNaTela.x > 1)
        {
            Destroy(personagem);
        }
        if (posicaoDoPersonagemNaTela.y < 0 || posicaoDoPersonagemNaTela.y > 1)
        {
            Destroy(personagem);
        }
    }

    public static void Atirar(GameObject atirador, GameObject tiroDroper, GameObject municao)
    {
        float velocidadeDoDisparo = 100.0f;

        GameObject tiroInstanciado = Instantiate(municao, tiroDroper.transform.position, Quaternion.identity);

        // Jogador e inimigo compartilham mesma munição, mas jogador não pode dar dano em sí mesmo

        if (atirador.CompareTag("Player")) tiroInstanciado.name += " Jogador";

        tiroInstanciado.GetComponent<Rigidbody>().velocity = tiroDroper.transform.up * velocidadeDoDisparo;
    }

    public static float LevarDano(GameObject quemLevouDano, GameObject tiro, ref float velocidade, ref float resistenciaDeVeneno, ref bool envenenado)
    {
        // Impede auto infligir de dano
        if (quemLevouDano.CompareTag("Player") && tiro.name.Contains("Jogador")) return 0;
        if (!quemLevouDano.CompareTag("Player") && !tiro.name.Contains("Jogador")) return 0;

        Shoots shot = new Shoots();

        if (tiro.CompareTag("Bullet Fire")) return shot.dano * 2;

        if (tiro.CompareTag("Bullet Freeze")) velocidade = GameManager.CalcularReducaoDeVelocidade(velocidade);
        else if (tiro.CompareTag("Bullet Poison")) envenenado = GameManager.VerificarSeEstaEnvenenado(ref resistenciaDeVeneno);

        return shot.dano;
    }

    public static float CalcularReducaoDeVelocidade(float velocidadeDoObjeto)
    {
        float velocidade = velocidadeDoObjeto / 10;
        return velocidade;
    }

    public static bool VerificarSeEstaEnvenenado(ref float resistenciaDeVeneno)
    {
        // out requer que inicialize no método por isso não funcionava aqui
        Shoots shot = new Shoots();

        resistenciaDeVeneno -= shot.TiroEnvenenado;
        return resistenciaDeVeneno <= 0; // == if (resistenciaDeVeneno <= 0) return true; return false;

    }
}
