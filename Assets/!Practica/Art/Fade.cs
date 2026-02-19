using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fade : MonoBehaviour
{
    public GameObject objetoPadre;

    public Image Fondo;
    public Image FondoRefuerco;
    public TextMeshProUGUI Controles;

    public float esperaInicial = 1f;
    public float tiempoFade = 2f;
    public float tiempoVisible = 2f;

    void Start()
    {
        StartCoroutine(Rutina());
    }

    IEnumerator Rutina()
    {
        yield return new WaitForSeconds(esperaInicial);

        objetoPadre.SetActive(true);

        CambiarAlpha(0);

        float t = 0;
        while (t < tiempoFade)
        {
            t += Time.deltaTime;
            float a = t / tiempoFade;
            CambiarAlpha(a);
            yield return null;
        }

        CambiarAlpha(1);

        yield return new WaitForSeconds(tiempoVisible);

        t = 0;
        while (t < tiempoFade)
        {
            t += Time.deltaTime;
            float a = 1 - (t / tiempoFade);
            CambiarAlpha(a);
            yield return null;
        }

        CambiarAlpha(0);

        objetoPadre.SetActive(false);
    }

    void CambiarAlpha(float a)
    {
        if (Fondo != null)
        {
            Color c = Fondo.color;
            c.a = a;
            Fondo.color = c;
        }

        if (FondoRefuerco != null)
        {
            Color c = FondoRefuerco.color;
            c.a = a;
            FondoRefuerco.color = c;
        }

        if (Controles != null)
        {
            Color c = Controles.color;
            c.a = a;
            Controles.color = c;
        }
    }
}
