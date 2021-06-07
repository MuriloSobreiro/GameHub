using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotaoControlador : MonoBehaviour
{
    public Sprite imgIdle, imgClick;
    public TextMeshProUGUI txtIdle,txtClick;
    public void Clicar()
    {
        GetComponent<Image>().sprite = imgClick;
        txtIdle.gameObject.SetActive(false);
        txtClick.gameObject.SetActive(true);
        Invoke("Resetar", 0.1f);
    }
    private void Resetar()
    {
        txtClick.gameObject.SetActive(false);
        txtIdle.gameObject.SetActive(true);
        GetComponent<Image>().sprite = imgIdle;
    }
}
