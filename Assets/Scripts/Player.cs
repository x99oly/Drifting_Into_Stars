using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Definidas na Unity
    public float velocidade = 10.0f, cadenciaDeDisparos = 10.0f, stamina = 100.0f, dash = 20.0f;
    public GameObject projetil, arma;

    //Definidas em código
    float intervalo, energia;
    bool podeMovimentar = true;


    //Girar personagem
    Rigidbody jogadorRB;
    float cameraRayLength = 100f;
    void Awake()
    {
        energia = stamina;
        intervalo = cadenciaDeDisparos;

        jogadorRB = GetComponent<Rigidbody>();
    }
    void Update()
    {    

    }
    private void FixedUpdate()
    {
        GameManager.ManterNaTela(gameObject);

        if (podeMovimentar) MovimentarPersonagem();
        DashPersonagem();
        RotacionarPersonagem();
        

        float tiro = Input.GetAxis("Fire1");
        intervalo--;
        if(tiro > 0 && intervalo <= 0)
        {
            GameManager.Atirar(arma, projetil);
            intervalo = cadenciaDeDisparos;
        }

    }
    void MovimentarPersonagem()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(moveX*-1, moveY) * velocidade;

        transform.Translate(movimento * Time.deltaTime);
    }

    void DashPersonagem()
    {
        float novaPos = 0f;

        if (Input.GetKeyUp(KeyCode.Q))
        {
            novaPos = dash;
            if (energia >= (stamina / 2)) Dash(novaPos);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            novaPos = dash * -1;
            if (energia >= (stamina / 2)) Dash(novaPos);
        }
    }
    
    void Dash(float novaPos)
    {
        podeMovimentar = false;
        Vector3 posicaoAtual = transform.position;
        transform.position = new Vector3(posicaoAtual.x + novaPos, 0f, posicaoAtual.z);
        energia -= stamina / 2;
        if (energia < 0) energia = 0;
        podeMovimentar = true;

        Debug.Log(energia);
    }

    void RotacionarPersonagem()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(cameraRay, out hitInfo, cameraRayLength))
        {
            // Verifica se o objeto atingido está na camada do jogador
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return;
            }

            Vector3 playerToMouse = hitInfo.point - transform.position;
            playerToMouse.z = 0;

            // Calcula o ângulo de rotação necessário para que o lado do personagem siga o mouse
            float angulo = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg - 90f;  // Subtrai 90 graus para alinhar a frente com mouse

            Quaternion novaRotacao = Quaternion.Euler(new Vector3(0, 0, angulo));
            jogadorRB.MoveRotation(novaRotacao);
        }
    }

}
