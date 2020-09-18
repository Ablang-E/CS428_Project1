using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;

public class conditionController : MonoBehaviour
{

    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";

    private float directionDegreeFloat, windSpeedFloat, randomNumber;

    private string conditionVar;

    public GameObject clearSkyObj, fewCloudsObj, scatteredCloudsObj, brokenCLoudsObj, showerRainObj, rainObj, thunderObj, snowObj, mistObj;

    //Function to turn all condition indicators off - this is always called before checking to see what the parsing is. This helps with reset
    void resetCondition() {
        clearSkyObj.GetComponent<Renderer>().enabled = true;

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
                //Get the data in between
                string conditionData = getData(jsonData, "icon\":\"", "\"");
                Debug.Log("Recieved Condition Info: " + conditionData);
                clearSkyObj.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f); //originally .001 .001 .001
                
                //resetCondition();

                //textObject.GetComponent<TextMeshPro>().text = conditionData;

                //Now we want to run through here and check which condition displays 
                //Icons have night and day png****
                /*
                if (conditionData == "01d" || conditionData == ".01n") {
                    sunWidget = enabled
                }
                */



                // print out the weather data to make sure it makes sense
                

                //directionDegreeFloat = float.Parse(directionData, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }
    void Start()
    {
        InvokeRepeating("GetDataFromWeb", 2f, 900f);
    }

    void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

   void Update() {
       clearSkyObj.transform.Rotate (0.0f, 1.0f, 0.0f, Space.Self);
       
   }
}

