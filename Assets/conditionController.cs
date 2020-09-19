using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;

/* References for sound clips
http://soundbible.com/636-Windy.html
http://soundbible.com/634-Wind-Blowing.html
http://soundbible.com/633-Snowing.html
http://soundbible.com/2083-Crickets-Chirping-At-Night.html
http://soundbible.com/1661-Sunny-Day.html
http://soundbible.com/1508-Background-Noise.html
http://soundbible.com/2006-Bird-In-Rain.html
http://soundbible.com/901-Rain-And-Thunder-Strikes.html
http://soundbible.com/2065-Rain-Inside-House.html
Sun texture: https://www.solarsystemscope.com/textures/

*/

public class conditionController : MonoBehaviour
{

    string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=8ad1c3f5b1132445dd295286a925fe22&units=imperial";

    private float directionDegreeFloat, windSpeedFloat, randomNumber;

    public GameObject clearSkyObj, fewCloudsObj, scatteredCloudsObj, brokenCLoudsObj, showerRainObj, rainObj, thunderObj, snowObj, mistObj;

    public GameObject[] objArray;

    public int counter;


    //Function to turn all condition indicators off - this is always called before checking to see what the parsing is. This helps with reset
    void debugTurnOffConditions() {
        clearSkyObj.gameObject.SetActive(false);
        fewCloudsObj.gameObject.SetActive(false);
        scatteredCloudsObj.gameObject.SetActive(false);
        brokenCLoudsObj.gameObject.SetActive(false);
        showerRainObj.gameObject.SetActive(false);
        rainObj.gameObject.SetActive(false);
        thunderObj.gameObject.SetActive(false);
        snowObj.gameObject.SetActive(false);
        mistObj.gameObject.SetActive(false); 
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
                
                //Now we want to run through here and check which condition displays 
                //Icons have night and day png****
                //This is for testing
                //conditionData = "10d";
                
                //Clear skys
                if (conditionData == "01d" || conditionData == "01n") {
                    clearSkyObj.gameObject.SetActive(true);
                }

                //Few clouds
                else if (conditionData == "02d" || conditionData == "02n") {
                    fewCloudsObj.gameObject.SetActive(true);
                }

                //Scatter clouds
                else if (conditionData == "03d" || conditionData == "03n") {
                    scatteredCloudsObj.gameObject.SetActive(true);
                }

                //Broken clouds
                else if (conditionData == "04d" || conditionData == "04n") {
                    brokenCLoudsObj.gameObject.SetActive(true);
                }

                //Shower rain
                else if (conditionData == "09d" || conditionData == "09n") {
                    showerRainObj.gameObject.SetActive(true);
                }

                //Rain 
                else if (conditionData == "10d" || conditionData == "10n") {
                    rainObj.gameObject.SetActive(true);
                }

                //Thunderstorm
                else if (conditionData == "11d" || conditionData == "11n") {
                    thunderObj.gameObject.SetActive(true);
                }

                //Snow
                else if (conditionData == "13d" || conditionData == "13n") {
                    snowObj.gameObject.SetActive(true);
                }

                //Mist
                else if (conditionData == "50d" || conditionData == "50n") {
                    mistObj.gameObject.SetActive(true);
                }
            }
        }
    }
    void Start()
    {
        InvokeRepeating("GetDataFromWeb", 2f, 900f);

        //Putting all gameobjects in an array for debugging purposes
        //objArray = new GameObject[] {}

        //Set all conditions as false (not visible when we run the program)
        clearSkyObj.gameObject.SetActive(false);
        fewCloudsObj.gameObject.SetActive(false);
        scatteredCloudsObj.gameObject.SetActive(false);
        brokenCLoudsObj.gameObject.SetActive(false);
        showerRainObj.gameObject.SetActive(false);
        rainObj.gameObject.SetActive(false);
        thunderObj.gameObject.SetActive(false);
        snowObj.gameObject.SetActive(false);
        mistObj.gameObject.SetActive(false);

        counter = 0;
    }

    void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

   void Update() {

       //Debugging
       //This will turn off all widgets and display first condition which should be 01d/clear skys condition
       if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("In Debug Mode");

            //Edge case
            if (counter > 8) {
                counter = 0;
            }
            debugTurnOffConditions(); //turn off
            Debug.Log("*Debug; Displaying gameObject" + objArray[counter]);
            objArray[counter].SetActive(true);
            counter++;

       }


       clearSkyObj.transform.Rotate (0.0f, 1.0f, 0.0f, Space.Self);
       //This is the animation code for the clouds. Side to side and Up to down
       fewCloudsObj.transform.position = new Vector3 (Mathf.PingPong(Time.time * 0.05f, 0.1f), fewCloudsObj.transform.position.y, fewCloudsObj.transform.position.z);
       scatteredCloudsObj.transform.position = new Vector3 (Mathf.PingPong(Time.time * 0.05f, 0.1f), scatteredCloudsObj.transform.position.y, scatteredCloudsObj.transform.position.z);
       brokenCLoudsObj.transform.position = new Vector3 (Mathf.PingPong(Time.time * 0.05f, 0.1f), brokenCLoudsObj.transform.position.y, brokenCLoudsObj.transform.position.z);
   }
}

