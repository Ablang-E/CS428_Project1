using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;



public class directionTeller : MonoBehaviour
{
    //public GameObject weatherTextObject;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";
    private Vector3 scaleChange;
    private GameObject Arrow;
    public Transform Base;
    public float x,y,z;
   
    void Start()
    {

    // wait a couple seconds to start and then refresh every 900 seconds

       InvokeRepeating("GetDataFromWeb", 2f, 900f);
       Arrow = GameObject.CreatePrimitive(PrimitiveType.Cube);
       Arrow.transform.SetParent(Base, true);
       //       Arrow.transform.parent = weatherTextObject.transform;
       Arrow.transform.localPosition = new Vector3(-0.0f,0.0f,0.0f);
       scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
       Arrow.transform.localScale += scaleChange;
       Arrow.name = "Self";
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
                string tempData = getData(jsonData, "temp\":", ".");
                Debug.Log("Recieved Temp Data: " + tempData);

                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                //transform.Rotate(x,y,z, Space.Self);





                //transform.rotation = new Vector3(0,90,0);
                
                //Output to TMP in Unity
                //weatherTextObject.GetComponent<TextMeshPro>().text = "Temperature: " + tempData + "F";

                //scaleChange = new Vector3(0.0f, -90.0f, 0.0f);
                //weatherTextObject.transform.localScale += scaleChange;
                //directionTeller.transform.localScale += scaleChange;
            }
        }
    }

    void Update() {
        //Arrow.transform.Rotate(x,y,z, Space.Self);
        Arrow.transform.localRotation = Quaternion.Euler (x,y,z);
        Debug.Log("Rotation Successful");
    }
}
