using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class arrowPointer : MonoBehaviour
{
    public GameObject pointer, lowerPointer;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";

    private float directionDegreeFloat, windSpeedFloat;
    public float x,y,z;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

    public static string getData(string source, string sourceStart, string sourceEnd)
    {
        if (source.Contains(sourceStart) && source.Contains(sourceEnd))
        {
            //Variables to hold start and end of the part we want (basically the middle part)
            int Start;
            int End;

        
            Start = source.IndexOf(sourceStart, 0) + sourceStart.Length;
            End = source.IndexOf(sourceEnd, Start);

            //Return specific data
            return source.Substring(Start, End - Start);
        }

    //Return nothing (shouldnt get to this point)
        return "";
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            //String to hold the json text
            string jsonData;

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                //Now we have the Json casted into a string
                jsonData = webRequest.downloadHandler.text;

                Debug.Log("***Recieved Information!***");
                Debug.Log(":\nReceived Json: " + webRequest.downloadHandler.text);
                //Get the data in between
                string directionData = getData(jsonData, "deg\":", "}"); //need to check again if it has "gust"
                //check to see if icon contains gust
                //if it does, re parse the data using getData() (directionData, "", "gust");
                //if (directionData.Contains())
                Debug.Log("Recieved Wind Direction degree2: " + directionData);

                string speedData = getData(jsonData, "\"speed\":", ".");
                Debug.Log("Recieved Wind Speed2: " + speedData);

                // print out the weather data to make sure it makes sense
                

                directionDegreeFloat = float.Parse(directionData, System.Globalization.CultureInfo.InvariantCulture);
                windSpeedFloat = float.Parse(speedData, System.Globalization.CultureInfo.InvariantCulture);
                Debug.Log("Degrees Float: " + directionDegreeFloat);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        pointer.transform.localRotation = Quaternion.Euler(x,directionDegreeFloat,z);
        lowerPointer.transform.localRotation = Quaternion.Euler(x,windSpeedFloat * 4.0f,z);
    }
    
}
