using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GeradorTabuleiro : MonoBehaviour
{
    public GameObject casaPrefab, pecaPrefab, pecasGO;
    public Sprite[] imgPecas;
    public GameObject[,] casasMatriz = new GameObject[8,8];
    public GameObject[,] pecasMatriz = new GameObject[8,8];
    private RectTransform rtTab;
    public RodadaXadrezControlador rodCont;
    public List<CasaXadrezControlador> casaHighligth = new List<CasaXadrezControlador>();
    public PecaXadrezControlador pecaAtiva;
    public PecaXadrezControlador[] reis = new PecaXadrezControlador[2];
    void Start()
    {
        rtTab = GetComponent<RectTransform>();
        rodCont = GetComponentInParent<RodadaXadrezControlador>();
        CriarCasas();
        CriarPecas();
    }

    private void CriarCasas(){
        float tamanho;
        tamanho = rtTab.rect.width / 8;
        bool preto = false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                casasMatriz[i, j] = Instantiate(casaPrefab, transform);
                RectTransform rt = casasMatriz[i, j].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(tamanho, tamanho);
                rt.anchoredPosition = new Vector2(tamanho*i, tamanho*j);
                casasMatriz[i, j].name = ""+((char)(i+65))+(j+1);//Nomeando as Casas
                casasMatriz[i, j].GetComponent<CasaXadrezControlador>().coordenada = (i, j);
                if (preto)
                    casasMatriz[i, j].GetComponent<Image>().color = new Color32(235,236,208,255);
                preto = !preto;
            }
            preto = !preto;
        }
    }

    private void CriarPecas()
    {
        pecasGO.GetComponent<RectTransform>().sizeDelta = rtTab.sizeDelta;
        float tamanho = rtTab.rect.width / 8;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var peca = TipoDePeca(i, j);
                if (peca.valor > 0)
                {
                    peca.valor--;
                    pecasMatriz[i,j] = Instantiate(pecaPrefab, pecasGO.transform);
                    Image img = pecasMatriz[i,j].GetComponent<Image>();
                    img.sprite = imgPecas[peca.valor];
                    RectTransform rt = pecasMatriz[i,j].GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(tamanho, tamanho);
                    rt.anchoredPosition = new Vector2(tamanho * i, tamanho * j);
                    pecasMatriz[i,j].name = peca.tipo;
                    PecaXadrezControlador ctrl = pecasMatriz[i, j].GetComponent<PecaXadrezControlador>();
                    ctrl.coordenada = (i, j);
                    ctrl.gerTab = this;
                    ctrl.branco = peca.valor > 5 ?  true : false;
                    if (peca.valor == 1)
                        reis[0] = ctrl;
                    if (peca.valor == 7)
                        reis[1] = ctrl;
                    if (ctrl.branco)
                        rodCont.pecasB.Add(img);
                    else
                        rodCont.pecasP.Add(img);

                }
            }
        }
        rodCont.TrocarRodada();
    }

    public void CriarMovimentos((int i, int j)[] coords)
    {
        ResetarMovimentos();
        foreach (var coord in coords)
        {
            if (coord.i < 0 || coord.i > 7 || coord.j < 0 || coord.j > 7)
                continue;
            casaHighligth.Add(casasMatriz[coord.i, coord.j].GetComponent<CasaXadrezControlador>());
            casaHighligth[casaHighligth.Count-1].TrocarCor(pecasMatriz[coord.i,coord.j]!=null);
        }
    }
    public void ResetarMovimentos()
    {
        foreach (var item in casaHighligth)
        {
            item.ResetarMov();
        }
        casaHighligth.Clear();
    }

    private (int valor,string tipo) TipoDePeca(int i, int j)
    {//1&7 Rainhas, 2&8 Reis, 3&9 Torres, 4&10 Cavalos, 5&11 Bispos, 6&12 Peões
        if(j>1 && j<6)//Não há peças nas linhas de idex 2 a 5
            return (0,"");
        if (j == 1)//As linhas de index 1 e 6 são paredes de peões
            return (12, "PeaoB");
        if (j == 6)
            return (6,"PeaoP");
        if ((i == 0 || i == 7) && j == 0)//Posição das Torres
            return (9, "Torre");
        if ((i == 0 || i == 7) && j == 7)
            return (3, "Torre");
        if ((i == 1 || i == 6) && j == 0)//Posição dos Cavalos
            return (10, "Cavalo");
        if ((i == 1 || i == 6) && j == 7)
            return (4, "Cavalo");
        if ((i == 2 || i == 5) && j == 0)//Posição dos Bispos
            return (11, "Bispo");
        if ((i == 2 || i == 5) && j == 7)
            return (5, "Bispo");
        if (i == 3 && j == 0)//Posição da Rainha Branca
            return (7, "Rainha");
        if (i == 3 && j == 7)//Posição da Rainha Preta
            return (1, "Rainha");
        if (i == 4 && j == 0)//Posição do Rei Branco
            return (8, "Rei");
        if (i == 4 && j == 7)//Posição do Rei Preto
            return (2, "Rei");
        return (0,"");
    }
}
