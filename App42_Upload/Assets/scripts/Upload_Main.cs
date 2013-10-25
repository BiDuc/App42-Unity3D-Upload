using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System;
using System.Net.Security;
using AssemblyCSharp;		
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.upload;
using System.Security.Cryptography.X509Certificates;

public class Upload_Main : MonoBehaviour {
	
	
	//===========================================================================================
	Upload_Response callBack = new Upload_Response();   // Making callBack Object for Upload_Response.
	AllFiles_Response allFilesCallBack = new AllFiles_Response();   // Making callBack Object for AllFiles_Response.
	ServiceAPI sp = null;                            // Initializing Service API.
	UploadService uploadService = null;             // Initialising Upload Service.
	//===========================================================================================
	
	//===========================================================================================
	public string userName = "UploadUserName"; 
	public static string fileName = "testFile11"+DateTime.Now.Millisecond; 
	public static string filePath =  "E:/a.jpg";
	public string fileType = "IMAGE"; 
	public string description = "Image Description";
	public string imageURL;
	public string path;
	public string loadingMessage;
	public string loadingMessage2;
	private static Upload_Main con = null;
	
	public Vector2 scrollPosition = Vector2.zero;
	public static int j = j+10;
	public static bool showBtn = true;
	//===========================================================================================
	
		public GameObject cubeFace;
	    public static Texture2D t_dynamic_tx;
	    IList<object> listOfImages = new List<object>();
	
	public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
                                      SslPolicyErrors sslPolicyErrors)
	    {
       	 return true;
        }
	
	// Use this for initialization
	void Start () 
	{
		ServicePointManager.ServerCertificateValidationCallback = Validator;
		sp = new ServiceAPI("d794ed6fd8fa49da69e8cb6f3e19ac4a63a22f92d19f1aa7e658ba1d09b645be","3421b54ec141f0a7605662577a6aea355ba3b97f4d7143697888fa606f7a852b");
	    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
		uploadService.GetAllFiles(allFilesCallBack);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{	
		GUI.Label(new Rect(1020, 50, 200, 200), loadingMessage);
		GUI.Label(new Rect(660, 100, 200, 200), loadingMessage2);
		//======================================{{{{******UploadFile******}}}}==========================================	
		 
		if (showBtn == true && GUI.Button(new Rect(600, 400, 200, 30), "UploadFile"))
	        {
			    showBtn = false;
			    path = EditorUtility.OpenFilePanel("Select Wallpaper","","jpg");
			    Debug.Log("PATH IS ::: " + path);
				loadingMessage2 = "Please Wait...";
				uploadService = sp.BuildUploadService(); // Initializing Upload Service.
	            uploadService.UploadFile(fileName, path, "IMAGE", "Description", callBack);
			}
		
//		if (GUI.Button(new Rect(50, 100, 200, 30), "GetAllFiles"))
//	        {
//			    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
//	            uploadService.GetAllFiles(allFilesCallBack);
//			}
		scrollPosition = GUI.BeginScrollView(new Rect(1000, 40, 140, 400), scrollPosition, new Rect(0, 0, 120, 3000));
		loadingMessage = "Loading...";
		if(listOfImages.Count > 1){
			for(int i=0; i<listOfImages.Count; i++){
				Texture2D myImage = (Texture2D)listOfImages[i];
				GUILayout.Label(myImage,GUILayout.MaxHeight(100),GUILayout.MaxWidth(100));
			}
			loadingMessage = "";
		}
		
         GUI.EndScrollView();
	}

	public static Upload_Main GetInstance ()
		{
			if (con == null) {
				con = (new GameObject ("Upload_Main")).AddComponent<Upload_Main> ();
				return con;
			} else {
				return con;
			}

		}
	
	 IEnumerator WaitForRequest (string uri)
	{
		IEnumerator e = execute (uri);
		while (e.MoveNext())
		{
			yield return e.Current;

		}
		
	}
	
	IEnumerator ShowAllImages (string uri)
	{
		IEnumerator e = executeShowAll (uri);
		while (e.MoveNext())
		{
			yield return e.Current;
		}
		
	}
	
	public string ExecuteGet (string url)
	{
		string responseFromServer = null;
		StartCoroutine (WaitForRequest (url));
		return responseFromServer;
	}
	
	public string ExecuteShow (string url)
	{
		string responseFromApp42 = null;
		StartCoroutine (ShowAllImages (url));
		return responseFromApp42;
	}
	
	void Awake ()
		{
			// First we check if there are any other instances conflicting
			if (con != null && con != this) {
				// If that is the case, we destroy other instances
				Destroy (gameObject);
			}
 
			// Here we save our singleton instance
			con = this;
 
			// Furthermore we make sure that we don't destroy between scenes (this is optional)
			DontDestroyOnLoad (gameObject);
		}
	
	IEnumerator execute (string url)
	{
		WWW www = new WWW (url);
		while (!www.isDone) {
				
				yield return null;  
			}
			if (www.isDone) {
			loadingMessage2 = "";
			showBtn = true;
		   cubeFace.renderer.material.mainTexture = www.texture;
		}
	}
	
	IEnumerator executeShowAll (string url)
	{
		WWW www = new WWW (url);
		while (!www.isDone) 
			{
				
				yield return null;  
			}
			if (www.isDone)
		{
		  // t_dynamic_tx = www.texture;
		  listOfImages.Add(www.texture);	
		}
		
	}
	
}