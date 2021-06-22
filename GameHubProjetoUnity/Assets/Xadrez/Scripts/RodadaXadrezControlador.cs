using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RodadaXadrezControlador : MonoBehaviour
{
    private bool rodadaB = true;
    public CronometroXadrezControlador croB, croP;
    public List<Image> pecasB, pecasP;

    public void TrocarRodada()
    {
        TrocarRaycast(rodadaB);
        ResetEnPassant(rodadaB);
        croB.contar = rodadaB;
        croP.contar = !rodadaB;
        rodadaB = !rodadaB;
    }
    private void TrocarRaycast(bool rod)
    {
        foreach (Image img in pecasB)
        {
            if(img != null)
                img.raycastTarget = rod;
        }
        foreach (Image img in pecasP)
        {
            if (img != null)
                img.raycastTarget = !rod;
        }
    }
    private void ResetEnPassant(bool rod)
    {
        if (rod)
        {
            foreach (Image img in pecasP)
            {
                if (img != null)
                    img.GetComponent<PecaXadrezControlador>().enPassant = false;
            }
        }
        else
        {
            foreach (Image img in pecasB)
            {
                if (img != null)
                    img.GetComponent<PecaXadrezControlador>().enPassant = false;
            }
        }
    }
}
