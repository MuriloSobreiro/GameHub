using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PecaXadrezControlador : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rt;
    Canvas cv;
    public GeradorTabuleiro GerTab;
    public (int i, int j) coordenada;
    public bool branco;
    private int movimentos = 0;
    private float tamanhoLocal;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cv = GetComponentInParent<Canvas>();
    }
    private void Start()
    {
        tamanhoLocal = rt.sizeDelta.x;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / cv.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetarPeca();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        MostrarMovimentos();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        VerificarMovimento(eventData.pointerCurrentRaycast.worldPosition);
    }
    private void MostrarMovimentos()
    {
        GerTab.CriarMovimentos(CalcularMovimentos());
        GerTab.pecaAtiva = this;
    }
    private void VerificarMovimento(Vector2 posicao)
    {
        foreach (var item in GerTab.casaHighligth)
        {
            if (Vector2.Distance(posicao, item.posicaGlob) / rt.lossyScale.x<tamanhoLocal/2f)
            {
                MovimentarPeca(item);
                return;
            }
        }
    }
    private void MovimentarPeca(CasaXadrezControlador casaDest)
    {
        movimentos++;
        GerTab.pecasMatriz[coordenada.i, coordenada.j] = null;
        coordenada = casaDest.coordenada;
        GerTab.pecasMatriz[coordenada.i, coordenada.j] = this.gameObject;
        ResetarPeca();
        GerTab.ResetarMovimentos();
    }
    private void ResetarPeca()
    {
        rt.anchoredPosition = new Vector2(tamanhoLocal * coordenada.i, tamanhoLocal * coordenada.j);
    }
    private (int i, int j)[] CalcularMovimentos()
    {
        (int i, int j)[] resultado = { };
        switch (transform.name)
        {
            case "peaoB":
                resultado = MovimentoPeao(true);
                break;
            case "peaoP":
                resultado = MovimentoPeao(false);
                break;
            case "Torre":
                resultado = MovimentoTorre();
                break;
            case "Cavalo":
                resultado = MovimentoCavalo();
                break;
            case "Bispo":
                resultado = MovimentoBispo();
                break;
            case "Rainha":
                resultado = MovimentoRainha();
                break;
            case "Rei":
                resultado = MovimentoRei();
                break;
            default:
                break;
        }
        return resultado;
    }

    private void AdicionarPosicao(ref List<(int i, int j)> resultado,int i, int j, bool esp = false)
    {
        if (i < 0 || i > 7 || j < 0 || j > 7)
            return;
        if (esp && GerTab.pecasMatriz[i, j] == null)
            return;
        resultado.Add((i, j));
    }

    private (int i, int j)[] MovimentoPeao(bool branco)
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        if (movimentos >= 1)
        {
            if (branco)
            {
                if (GerTab.pecasMatriz[coordenada.i, coordenada.j + 1] == null)
                    AdicionarPosicao(ref resultado, coordenada.i, coordenada.j + 1);
            }
            else
            {
                if (GerTab.pecasMatriz[coordenada.i, coordenada.j - 1] == null)
                    AdicionarPosicao(ref resultado, coordenada.i, coordenada.j - 1);
            }
        }
        else
        {
            if (branco)
            {
                AdicionarPosicao(ref resultado, coordenada.i, coordenada.j + 1);
                AdicionarPosicao(ref resultado, coordenada.i, coordenada.j + 2);
            }
            else
            {
                AdicionarPosicao(ref resultado, coordenada.i, coordenada.j - 1);
                AdicionarPosicao(ref resultado, coordenada.i, coordenada.j - 2);
            } 
        }
        if (branco)
        {
            AdicionarPosicao(ref resultado, coordenada.i + 1, coordenada.j + 1, true);
            AdicionarPosicao(ref resultado, coordenada.i - 1, coordenada.j + 1, true);
        }
        else
        {
            AdicionarPosicao(ref resultado, coordenada.i + 1, coordenada.j - 1, true);
            AdicionarPosicao(ref resultado, coordenada.i - 1, coordenada.j - 1, true);
        }
        return resultado.ToArray();
    }

    private (int i, int j)[] MovimentoTorre()
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        resultado.AddRange(CalcCasasDir(1, 0));
        resultado.AddRange(CalcCasasDir(-1, 0));
        resultado.AddRange(CalcCasasDir(0, 1));
        resultado.AddRange(CalcCasasDir(0, -1));
        return resultado.ToArray();
    }
    private (int i, int j)[] MovimentoBispo()
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        resultado.AddRange(CalcCasasDir(1, 1));
        resultado.AddRange(CalcCasasDir(1, -1));
        resultado.AddRange(CalcCasasDir(-1, 1));
        resultado.AddRange(CalcCasasDir(-1, -1));
        return resultado.ToArray();
    }
    private (int i, int j)[] MovimentoRainha()
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        resultado.AddRange(MovimentoTorre());
        resultado.AddRange(MovimentoBispo());
        return resultado.ToArray();
    }
    private List<(int i, int j)> CalcCasasDir(int x, int y)
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        Vector2 variacao = new Vector2(x, y);
        while (VerCalcMov(coordenada.i + x, coordenada.j + y) && GerTab.pecasMatriz[coordenada.i + x, coordenada.j + y] == null)
        {
            AdicionarPosicao(ref resultado, coordenada.i + x, coordenada.j + y);
            x += (int)variacao.x;
            y += (int)variacao.y;
        }
        if(VerCalcMov(coordenada.i + x, coordenada.j + y) && PecaInimiga(coordenada.i + x, coordenada.j + y))
        {
            AdicionarPosicao(ref resultado, coordenada.i + x, coordenada.j + y);
        }
        return resultado;
    }
    private (int i, int j)[] MovimentoCavalo()
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                if(Mathf.Abs(i) + Mathf.Abs(j) == 3 && VerCalcMov(coordenada.i + i, coordenada.j + j) && (GerTab.pecasMatriz[coordenada.i + i, coordenada.j + j] == null ||PecaInimiga(coordenada.i + i, coordenada.j + j)))
                    AdicionarPosicao(ref resultado, coordenada.i + i, coordenada.j + j);
            }
        }
        return resultado.ToArray();
    }
    private (int i, int j)[] MovimentoRei()
    {
        List<(int i, int j)> resultado = new List<(int i, int j)>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(!(i==0 && j==0) && VerCalcMov(coordenada.i + i, coordenada.j + j) && (GerTab.pecasMatriz[coordenada.i + i, coordenada.j + j] == null || PecaInimiga(coordenada.i + i, coordenada.j + j)))
                    AdicionarPosicao(ref resultado, coordenada.i + i, coordenada.j + j);
            }
        }
        return resultado.ToArray();
    }
    private bool VerCalcMov(int x, int y)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7)
            return false;
        else
            return true;
    }
    private bool PecaInimiga(int i, int j)
    {
        if (GerTab.pecasMatriz[i, j].GetComponent<PecaXadrezControlador>().branco != branco)
            return true;
        else
            return false;
    }
    }
