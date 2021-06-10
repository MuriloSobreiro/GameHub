using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PecaXadrezControlador : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    RectTransform rt;
    Canvas cv;
    public GeradorTabuleiro GerTab;
    public (int i, int j) coordenada;
    public bool branco;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cv = GetComponentInParent<Canvas>();
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicou na peça");
        CalcularMovimentos();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Soltou a peça");
    }

    private (int i, int j)[] CalcularMovimentos()
    {
        (int i, int j)[] resultado = {(0, 0)};
        switch (transform.name)
        {
            case "peaoB":
                
                break;
            case "peaoP":
                break;
            case "Torre":
                break;
            case "Cavalo":
                break;
            case "Bispo":
                break;
            case "Rainha":
                break;
            case "Rei":
                break;
            default:
                break;
        }
        return resultado;
    }


}
