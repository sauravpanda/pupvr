using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;


public class CaptureCommentData : MonoBehaviour
{
    // Start is called before the first frame update
    public string userName = "Test";

    void OnMouseUpAsButton()
    {
        Debug.Log ("Started the ibj!");
        StartCoroutine(Upload());
    }

    void OnBecameInvisible()
    {
       
    }

    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("user_name=" + userName));
        // formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        UnityWebRequest www = UnityWebRequest.Post("https://1t5f803peg.execute-api.us-west-2.amazonaws.com/default/pupVR", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
