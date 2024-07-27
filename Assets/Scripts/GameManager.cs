using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] inimigos = new GameObject[3];
    public GameObject jogador;
    public float intervaloDeDrops = 2f;
    public int quantidadeMaxDeInimigos = 10;

    public bool podeDroparMainInimigos = true;
    float intervaloAtualDeDrops = 20;

    int quantidadeAtualDeInimigos = 0;
    void Start()
    {
        intervaloAtualDeDrops = intervaloDeDrops;
    }

    // Update is called once per frame
    void Update()
    {
        if (podeDroparMainInimigos) intervaloAtualDeDrops -= Time.deltaTime;
        if (intervaloAtualDeDrops <= 0)
        {
            int indexInimigo = Random.Range(0, inimigos.Length);
            float coordenadaX = Random.Range(0, 10) / 100;
            float coordenadaY = Random.Range(0, 10) / 100;

            DroparInimigo(indexInimigo, coordenadaX, coordenadaY);
            intervaloAtualDeDrops = intervaloDeDrops;
        }

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

        if (atirador.CompareTag("Player"))
        {
            Material corDoTiroDoJogador = Resources.Load<Material>("Assets/Materials/colors/PlayerBullet.mat");
            Renderer tiroRender = tiroInstanciado.GetComponent<Renderer>();
            tiroRender.material = corDoTiroDoJogador;

            tiroInstanciado.name += " Jogador";
        }

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
    public void DroparInimigo(int indexInimigo, float coordenadaX, float coordenadaY)
    {

        Vector3 posicaoDeDrop = Camera.main.ViewportToWorldPoint(new Vector3(coordenadaY, coordenadaX, 100));

        GameObject inimigoInstanciado = Instantiate(inimigos[indexInimigo], posicaoDeDrop, Quaternion.identity);
        quantidadeAtualDeInimigos++;

    }

}
