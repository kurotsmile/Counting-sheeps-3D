using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Panel_loading : MonoBehaviour
{
    public Slider slider_download;
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void show_prossce(UnityWebRequest request)
    {
        this.slider_download.gameObject.SetActive(true);
        StartCoroutine(progress(request));
    }

    IEnumerator progress(UnityWebRequest req)
    {
        while (req.downloadProgress < 0.9f)
        {
            this.slider_download.value = req.downloadProgress;
            Debug.Log("pp:"+slider_download.value);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Show()
    {
        this.slider_download.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
