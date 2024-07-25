using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float velocidade = 10.0f;

    public GameObject pivot;

    int seg = 300;
    int s;



    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void FixedUpdate()
    {
        RotacionarPersonagem();

        ManterJogadorNaTela();
        MovimentarPersonagem();

    }
    void MovimentarPersonagem()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(moveX*-1, moveY,0.0f) * velocidade;

        transform.Translate(movimento * Time.deltaTime);
    }

    void RotacionarPersonagem()
    {
        // Obtém a posição do mouse no mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcula a diferença entre a posição do pivot e a posição do mouse
        Vector3 diff = mousePosition - pivot.transform.position;

        // Normaliza o vetor de diferença para obter a direção
        diff.Normalize();

        // Calcula o ângulo de rotação em graus
        float rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // Aplica a rotação ao pivot
        pivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    void ManterJogadorNaTela()
    {
        Vector3 posicaoDoJogador = transform.position;

        Vector3 posicaoDoJogadorNaTela = Camera.main.WorldToViewportPoint(transform.position);

        if(posicaoDoJogadorNaTela.x < 0 || posicaoDoJogadorNaTela.x > 1)
        {
            posicaoDoJogador.x = -posicaoDoJogador.x;
        }
        if(posicaoDoJogadorNaTela.y < 0 || posicaoDoJogadorNaTela.y > 1)
        {
            posicaoDoJogador.y = -posicaoDoJogador.y;
        }

        transform.position = posicaoDoJogador;
    }

}
