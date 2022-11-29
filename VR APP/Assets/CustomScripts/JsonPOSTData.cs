using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;


public class JsonPOSTData : MonoBehaviour
{
    // Start is called before the first frame update
    // public string userName = "New";
    private TMP_Text m_userName;
	TMP_Text m_commentText;
	TMP_Text m_dropdownComment;
	TMP_Text ComponentName;

    [System.Serializable]
    public class PlayerInfo{
        public string playerName;
		public string commentText;
		public string componentName;
		public string dropDownText;
    }
    
    public void SendNameData()
    {
        
        doPost();
    }

  
	public void doPost(){
		string URL = "https://1t5f803peg.execute-api.us-west-2.amazonaws.com/default/pupVR";
		
		//Auth token for http request
		string accessToken;
		//Our custom Headers
		Dictionary<string,string> parameters = new Dictionary<string, string>();
		parameters.Add( "Content-Type", "application/json" );
		parameters.Add( "AnotherHeader", "AnotherData" );

        Debug.Log("Sending Data!" + gameObject.name);
		PlayerInfo plInfo = new PlayerInfo();
        m_userName = GameObject.FindWithTag("UserName").GetComponent<TMP_Text>();
		plInfo.playerName = m_userName.text;
		Debug.Log("User Name: " + m_userName.text);

		Debug.Log("gamobject parent: " + transform.parent.gameObject.name);

		ComponentName = transform.parent.Find("Title").GetComponent<TMP_Text>();
		m_commentText = transform.parent.Find("typedComment").GetComponent<TMP_Text>();

		m_dropdownComment = transform.parent.Find("Dropdown/Label").GetComponent<TMP_Text>();
		if (m_commentText != null)
			{
				Debug.Log("Comment Text: " + m_commentText.text);
				plInfo.commentText = m_commentText.text;

			}
		else{
			plInfo.commentText = "No Comments";
			Debug.Log("Comment Object Not Found!");
		}

		if (m_dropdownComment != null)
			{
				Debug.Log("Dropdown Text: " + m_dropdownComment.text);
				plInfo.dropDownText = m_dropdownComment.text;

			}
		else{
			plInfo.dropDownText = "No Comments";
			Debug.Log("Dropdown Object Not Found!");
		}

        
        plInfo.componentName = ComponentName.text;
		
        string playerPayload = JsonUtility.ToJson(plInfo);

		parameters.Add ("Content-Length", playerPayload.Length.ToString());
		//Replace single ' for double " 
		//This is usefull if we have a big json object, is more easy to replace in another editor the double quote by singles one
		playerPayload = playerPayload.Replace("'", "\"");
		//Encode the JSON string into a bytes
		byte[] postData = System.Text.Encoding.UTF8.GetBytes (playerPayload);
		//Now we call a new WWW request
		WWW www = new WWW(URL, postData, parameters);
		//And we start a new co routine in Unity and wait for the response.
		StartCoroutine(WaitForRequest(www));
	}
	//Wait for the www Request
	IEnumerator WaitForRequest(WWW www){
		yield return www;
		if (www.error == null){
			//Print server response
			Debug.Log(www.text);
		} else {
		  //Something goes wrong, print the error response
			Debug.Log(www.error);
		}
	}
}
