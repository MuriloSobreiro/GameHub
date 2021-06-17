using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CasaXadrezControlador : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject movPrefab;
    public Vector2 posicaGlob;
    public (int i, int j) coordenada;
    public Vector2 posDebug;
    private Image imgMov;
    public Color32 corHighligth = new Color32(190, 190, 190, 150), corComer = new Color32(190, 0, 0, 150);

    private void Start()
    {
        posDebug = new Vector2(coordenada.i, coordenada.j);
        imgMov = movPrefab.GetComponent<Image>();
        posicaGlob = (Vector2)transform.position + GetComponent<RectTransform>().sizeDelta * transform.lossyScale/2;
    }
    public void TrocarCor(bool comer)
    {
        movPrefab.SetActive(true);
        if (comer)
            imgMov.color = corComer;
        else
            imgMov.color = corHighligth;
    }

    public void ResetarMov()
    {
        movPrefab.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
