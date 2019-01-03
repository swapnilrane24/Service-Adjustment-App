using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationPanel : MonoBehaviour, IPanel
{
    //API Key AIzaSyDYqI9UApRhiPOafLjqk4fZ8nRNNU4tvmY

    public Text caseNumber, loadingText;
    public RawImage map;
    public InputField mapNotes;
    public string apiKey = "AIzaSyDYqI9UApRhiPOafLjqk4fZ8nRNNU4tvmY";
    public float xCoord, yCoord;
    public int zoom;
    public int imageSize;
    public string url = "https://maps.googleapis.com/maps/api/staticmap?";

    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(mapNotes.text) == false)
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
    }

    private void Start()
    {
        Debug.Log("Location Panel");

        StartCoroutine(GetCoordinates());
    }

    IEnumerator GetCoordinates()
    {
        if (Input.location.isEnabledByUser == true)
        {
            Input.location.Start();

            int waitTime = 20;

            while (Input.location.status == LocationServiceStatus.Initializing && waitTime > 0)
            {
                yield return new WaitForSeconds(1f);
                waitTime--;
            }

            if (waitTime < 1)
            {
                Debug.Log("Timed Out");
                loadingText.text = "Timed Out";
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determin device location....");
                loadingText.text = "Failed to Locate";
            }
            else
            {
                xCoord = Input.location.lastData.latitude;
                yCoord = Input.location.lastData.longitude;
                loadingText.gameObject.SetActive(false);
            }

            Input.location.Stop();
        }
        else
        {
            Debug.Log("Location Services are not On");
            loadingText.text = "Location Service Disable";
        }

        StartCoroutine(GetMap());
    }

    IEnumerator GetMap()
    {
        //Download Static Map
        //https://maps.googleapis.com/maps/api/staticmap?center=40.714728,-73.998672&zoom=12&size=400x400&key=YOUR_API_KEY

        url += "center=" + xCoord + "," + yCoord + "&zoom=" + zoom + "&size=" + imageSize + "x" + imageSize + "&key=" + apiKey;

        using (WWW www = new WWW(url))
        {
            yield return www;

            if (www.error != null)
            {
                Debug.LogError("Map Error: " + www.error);
            }

            map.texture = www.texture;
        }
    }


    

}
