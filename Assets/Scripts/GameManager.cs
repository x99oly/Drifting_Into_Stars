using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
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

        if (posicaoDoPersonagemNaTela.x < 0 || posicaoDoPersonagemNaTela.x > 1)
        {
            Destroy(personagem);
        }
        if (posicaoDoPersonagemNaTela.y < 0 || posicaoDoPersonagemNaTela.y > 1)
        {
            Destroy(personagem);
        }
    }

    public static void Atirar(GameObject tiroDroper, GameObject municao)
    {
        float velocidadeDoDisparo = 100.0f;

        GameObject tiroInstanciado = Instantiate(municao, tiroDroper.transform.position, Quaternion.identity);

        tiroInstanciado.GetComponent<Rigidbody>().velocity = tiroDroper.transform.up * velocidadeDoDisparo;

    }
}
