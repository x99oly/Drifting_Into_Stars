using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Definidas na Unity
    public float vida = 100f, stamina = 100.0f, velocidade = 10.0f, cadenciaDeDisparos = 10.0f,  tempoDoDash = 0.5f, velocidaDoDash = 3;
    public GameObject projetil, arma;

    //Definidas em código
    float intervalo, energia;
    bool podeMovimentar = true;
    float tiro;


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
        // Vars
        tiro = Input.GetAxis("Fire1");
        intervalo--;

        // Execuções constantes
        RotacionarPersonagem();

        // Execuções Condicionais
        if (podeMovimentar) MovimentarPersonagem(); //Dash impede movimento
        if (Input.GetKeyUp(KeyCode.Q) && energia >= (stamina / 2)) StartCoroutine(Dash(-1));
        if (Input.GetKeyUp(KeyCode.E) && energia >= (stamina / 2)) StartCoroutine(Dash(1));
        if (tiro > 0) Disparar();

        // Execuções de códigos externos
        GameManager.ManterNaTela(gameObject);
    }

    void Disparar()
    {
        if (intervalo > 0) return;

        GameManager.Atirar(gameObject, arma, projetil);
        intervalo = cadenciaDeDisparos;
    }
    void MovimentarPersonagem()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(moveX * -1, moveY) * velocidade;

        transform.Translate(movimento * Time.deltaTime);
    }

    IEnumerator Dash(int direcao)
    {
        podeMovimentar = false;

        jogadorRB.velocity = transform.right * (velocidade * velocidaDoDash * direcao);
        yield return new WaitForSeconds(tempoDoDash);

        jogadorRB.velocity = Vector3.zero;

        energia -= stamina / 2; if (energia < 0) energia = 0;
        podeMovimentar = true;
    }

    float ContagemRegressiva(float tempo)
    {
        if (tempo == 1) return 0;
        return ContagemRegressiva(tempo - 1);
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
