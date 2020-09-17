using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;



public class speedTeller : MonoBehaviour
{
    //public GameObject weatherTextObject;
    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";
    private Vector3 scaleChange1;
    private Vector3 scaleChange2;
    private GameObject Arrow, tipOfArrow;
    public Transform Base;
    public Texture indicatorTexture;
    Renderer render;

    private float directionDegreeFloat;
    public float x,y,z;
   
    void Start()
    {

    // wait a couple seconds to start and then refresh every 900 seconds

       InvokeRepeating("GetDataFromWeb", 2f, 900f);

       //Creating texture for indicator
       render = GetComponent<Renderer> ();

       render.material.EnableKeyword ("_NORMALMAP");
       render.material.EnableKeyword ("_METALLICGLOSSMAP");

       render.material.SetTexture("indicator", indicatorTexture);

       //Creating the indicator
       Arrow = GameObject.CreatePrimitive(PrimitiveType.Cube);
       //tipOfArrow = GameObject.CreatePrimitive(PrimitiveType.Cube);

       //Settting the indicator as a child to image target (Base)
       Arrow.transform.SetParent(Base, true);
       //tipOfArrow.transform.SetParent(Base, true);
       //tipOfArrow.transform.parent = Arrow.transform;

       
       //       Arrow.transform.parent = weatherTextObject.transform;
       //Position the indicator at origin of base
       Arrow.transform.localPosition = new Vector3(-0.0f,0.0f,-1.0f);
       //tipOfArrow.transform.localPosition = new Vector3(-0.0f,0.0f,0.1f);


       //This controls the scale of the indicator
       scaleChange1 = new Vector3(-0.99f, -0.95f, -0.99f); //higher the number, the smaller
       scaleChange2 = new Vector3(-0.999f, -0.95f, -0.999f); //higher the number, the smaller

       //Applying the scale changes from above
       Arrow.transform.localScale += scaleChange1;
       //tipOfArrow.transform.localScale += scaleChange2;

       //Color changing
       //tipOfArrow.GetComponent<Renderer>().material.color = Color.red;
       Arrow.GetComponent<Renderer>().material.color = Color.black;
       Arrow.name = "Self";

       //Arrow.GetComponent<Renderer>().material.mainTexture = render.material.mainTexture;
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
                string directionData = getData(jsonData, "deg\":", "}");
                Debug.Log("Recieved Wind Direction degree: " + directionData);

                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                directionDegreeFloat = float.Parse(directionData, System.Globalization.CultureInfo.InvariantCulture);
                Debug.Log("Degrees Float: " + directionDegreeFloat);

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
        Arrow.transform.localRotation = Quaternion.Euler (x,directionDegreeFloat,z);
        //tipOfArrow.transform.localRotation = Quaternion.Euler (x,directionDegreeFloat,z);
        Debug.Log("Rotation Successful - In function Update()");
    }
}
