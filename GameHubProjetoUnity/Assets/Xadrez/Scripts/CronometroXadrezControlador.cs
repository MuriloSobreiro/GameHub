using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CronometroXadrezControlador : MonoBehaviour
{
    public bool contar = false;
    public float tempoInicial = 10;
    private TimeSpan tempoAtual;
    public TextMeshProUGUI cronometro;
    private void Start()
    {
        tempoAtual = TimeSpan.FromSeconds(tempoInicial * 60);
        AtualizarTempo(0f);
    }
    void Update()
    {
        if (contar)
            AtualizarTempo(Time.deltaTime);
    }

    private void AtualizarTempo(float qtd)
    {
        tempoAtual = tempoAtual.Subtract(TimeSpan.FromSeconds(qtd));
        cronometro.text = tempoAtual.ToString("mm':'ss'.'f");
    }
}
