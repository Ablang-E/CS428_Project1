using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;



public class directionInfoTeller : MonoBehaviour
{
    public GameObject weatherTextObject;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";
    private Vector3 scaleChange1;
    private Vector3 scaleChange2;
    private GameObject Arrow, tipOfArrow;
    public Transform Base;
    public Texture indicatorTexture;
    Renderer render;
    string direction;

    private float directionDegreeFloat;
    public float x,y,z;
   
    void Start()
    {

    // wait a couple seconds to start and then refresh every 900 seconds

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

                Debug.Log("*Recieved Information!*");
                //Get the data in between
                string directionData = getData(jsonData, "deg\":", ",");
                Debug.Log("Recieved Wind Direction degree111: " + directionData);

                string speedData = getData(jsonData, "\"speed\":", ".");
                Debug.Log("Recieved Wind Speed111: " + speedData);

                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                directionDegreeFloat = float.Parse(directionData, System.Globalization.CultureInfo.InvariantCulture);
                Debug.Log("Degrees Float1111: " + directionDegreeFloat);

                //Checking to for mainly NESW first
                if (directionDegreeFloat == 90) {
                    direction = "N";
                }

                else if (directionDegreeFloat == 0) {
                    direction = "E";
                }

                else if (directionDegreeFloat == 180) {
                    direction = "W";
                }

                else if (directionDegreeFloat == 270) {
                    direction = "S";
                }

                //Now checking NE, NW, SE, SW
                //NE Check
                else if (directionDegreeFloat > 0 && directionDegreeFloat < 90) {
                    direction = "NE";
                }

                //NW Check
                else if (directionDegreeFloat > 90 && directionDegreeFloat < 180) {
                    direction = "NW";
                }

                //SE Check
                else if (directionDegreeFloat > 270 && directionDegreeFloat < 360) {
                    direction = "SE";
                }

                //SW Check
                else if (directionDegreeFloat > 180 && directionDegreeFloat < 270) {
                    direction = "SW";
                }
                
                //Output to TMP in Unity
                Debug.Log("Recieved Accurate Direction Data: " + direction);
                weatherTextObject.GetComponent<TextMeshPro>().text = speedData + " mph " + direction;
            }
        }
    }
}
