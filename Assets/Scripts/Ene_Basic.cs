using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ene_Basic : Enemys
{
    void Start()
    {

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        vidaAtual -= GameManager.LevarDano(gameObject, other.gameObject, ref velocidade, ref resistenciaVeneno, ref estaEnvenenado);
        Debug.Log(vidaAtual);
        if (vidaAtual <= 0) Destroy(gameObject);
    }
}
