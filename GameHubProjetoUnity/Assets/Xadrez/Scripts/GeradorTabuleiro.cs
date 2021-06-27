using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GeradorTabuleiro : MonoBehaviour
{
    public GameObject casaPrefab, pecaPrefab, pecasGO;
    public Sprite[] imgPecas;
    public int tam = 8;
    public GameObject[,] casasMatriz;
    public GameObject[,] pecasMatriz;
    private RectTransform rtTab;
    public RodadaXadrezControlador rodCont;
    public List<CasaXadrezControlador> casaHighligth = new List<CasaXadrezControlador>();
    public PecaXadrezControlador pecaAtiva;
    public PecaXadrezControlador[] reis = new PecaXadrezControlador[2];
    void Start()
    {
        casasMatriz = new GameObject[tam, tam];
        pecasMatriz = new GameObject[tam, tam];
        rtTab = GetComponent<RectTransform>();
        rodCont = GetComponentInParent<RodadaXadrezControlador>();
        CriarCasas();
        CriarPecas();
    }

    private void CriarCasas()
    {
        float tamanho;
        tamanho = rtTab.rect.width / tam;
        for (int i = 0; i < tam; i++)
        {
            for (int j = 0; j < tam; j++)
            {
                casasMatriz[i, j] = Instantiate(casaPrefab, transform);
                RectTransform rt = casasMatriz[i, j].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(tamanho, tamanho);
                rt.anchoredPosition = new Vector2(tamanho * i, tamanho * j);
                casasMatriz[i, j].name = "" + ((char)(i + 65)) + (j + 1);//Nomeando as Casas
                casasMatriz[i, j].GetComponent<CasaXadrezControlador>().coordenada = (i, j);
                if ((i + j) % 2 == 0)
                    casasMatriz[i, j].GetComponent<Image>().color = new Color32(235, 236, 208, 255);
            }
        }
    }

    private void CriarPecas()
    {
        pecasGO.GetComponent<RectTransform>().sizeDelta = rtTab.sizeDelta;
        int[,] output = conversorFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        float tamanho = rtTab.rect.width / tam;
        for (int i = 0; i < tam; i++)
        {
            for (int j = 0; j < tam; j++)
            {
                if (output[i,j] >= 0)
                {
                    pecasMatriz[i, j] = Instantiate(pecaPrefab, pecasGO.transform);
                    RectTransform rt = pecasMatriz[i, j].GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(tamanho, tamanho);
                    rt.anchoredPosition = new Vector2(tamanho * i, tamanho * j);
                    PecaXadrezControlador ctrl = pecasMatriz[i, j].GetComponent<PecaXadrezControlador>();
                    ctrl.coordenada = (i, j);
                    ctrl.gerTab = this;
                    ctrl.branco = output[i, j] > 5 ? true : false;
                    ctrl.tipoPeca = output[i, j];
                    ctrl.AtualizarPeca();
                    if (output[i, j] == 1)
                        reis[0] = ctrl;
                    if (output[i, j] == 7)
                        reis[1] = ctrl;
                    Image img = pecasMatriz[i, j].GetComponent<Image>();
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
            if (coord.i < 0 || coord.i > tam - 1 || coord.j < 0 || coord.j > tam - 1)
                continue;
            casaHighligth.Add(casasMatriz[coord.i, coord.j].GetComponent<CasaXadrezControlador>());
            casaHighligth[casaHighligth.Count - 1].TrocarCor(pecasMatriz[coord.i, coord.j] != null);
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
    private int[,] conversorFen(string input)
    {
        int[,] output = new int[tam, tam];
        string[] temp = input.Split('/');
        int i;
        for (int j = 0; j < tam; j++)
        {
            i = 0;
            foreach (char letra in temp[j])
            {
                int val = FenParaInt(letra);
                Debug.Log(i + "," + j);
                if(val >= 0)
                    output[i, j] = val;
                else
                {
                    for (int a = 0; a < (int)Char.GetNumericValue(letra); a++)
                    {
                        output[i+a, j] = val;
                    }
                    i += (int)Char.GetNumericValue(letra)-1;
                }
                i++;
            }
        }
        return output;
    }

    private int FenParaInt(char a)
    {
        int result = -7;
        switch (Char.ToUpper(a))
        {
            case 'Q':
                result = 0;
                break;
            case 'K':
                result = 1;
                break;
            case 'R':
                result = 2;
                break;
            case 'N':
                result = 3;
                break;
            case 'B':
                result = 4;
                break;
            case 'P':
                result = 5;
                break;
            default:
                break;
        }
        result += Char.IsUpper(a) ? 0 : 6;
        return result;
    }
}