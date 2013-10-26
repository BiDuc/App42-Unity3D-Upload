using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Security;
using AssemblyCSharp;		
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.upload;

public class Upload_Main : MonoBehaviour {
	
	//=============================FILE_BROWSER_CONTENT========================================
	public GUISkin gameSkin;
	protected FileBrowser m_fileBrowser;
	[SerializeField]
	protected Texture2D	m_directoryImage,
						m_fileImage;
	//=============================FILE_BROWSER_CONTENT========================================
	
	
	//=============================INITIALIZING_SERVICES==========================================
	Upload_Response callBack = new Upload_Response();   // Making callBack Object for Upload_Response.
	AllFiles_Response allFilesCallBack = new AllFiles_Response();   // Making callBack Object for AllFiles_Response.
	ServiceAPI sp = null;                            // Initializing Service API.
	UploadService uploadService = null;             // Initialising Upload Service.
	private static Upload_Main con = null;
	//===========================================================================================
	
	//=============================DEFINING-PARAMETERS==========================================
	public static string fileName = "testFile11"+DateTime.Now.Millisecond; 
	public string fileType = "IMAGE"; 
	public string description = "Image Description";
	protected string path;
	//=============================DEFINING-PARAMETERS===========================================
	
	
	//=============================GUI_CONTENT===========================================
	public Vector2 scrollPosition = Vector2.zero;
	public static bool showBtn = true;
	public static bool showUploadBtn = false;
	public string loadingMessage;
	public string loadingMessage2;
	public GameObject cubeFace;
    IList<object> listOfImages = new List<object>();
	//=============================GUI_CONTENT===========================================
		
	// Use this for initialization
	void Start () 
	{
		sp = new ServiceAPI("d794ed6fd8fa49da69e8cb6f3e19ac4a63a22f92d19f1aa7e658ba1d09b645be","3421b54ec141f0a7605662577a6aea355ba3b97f4d7143697888fa606f7a852b");
	    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
    }
	
	
  	 protected void OnGUIMain() 
		{
 			if (GUI.Button(new Rect(400, 390, 200, 30), "Select File"))
	        	{
					m_fileBrowser = new FileBrowser(
							new Rect(100, 100, 600, 500),
							"Choose Image File",
							FileSelectedCallback
						);
						m_fileBrowser.SelectionPattern = "*.jpg";
						m_fileBrowser.DirectoryImage = m_directoryImage;
						m_fileBrowser.FileImage = m_fileImage;
				}
		}
	
	
	
	void OnGUI()
	{	
		//======================================Loading_Messages==========================================	
		GUI.Label(new Rect(720, 50, 200, 200), loadingMessage);
		GUI.Label(new Rect(460, 100, 200, 200), loadingMessage2);
		//======================================Loading_Messages==========================================	
		
		//======================================File_Browser_Content==========================================	
		if (m_fileBrowser != null) 
		{
			GUI.skin = gameSkin;
	        m_fileBrowser.OnGUI();
	       GUI.skin = null;
		} 
		else 
		{
			OnGUIMain();
		}
		//======================================File_Browser_Content==========================================
		
		
		//======================================UploadFile==========================================	
		GUI.Label(new Rect(400, 420, 200, 20), path ?? "None selected");
		
		if (showUploadBtn == true && showBtn == true && GUI.Button(new Rect(400, 440, 200, 30), "UploadFile"))
	        {
			showUploadBtn = false;
			    showBtn = false;
			  //  path = EditorUtility.OpenFilePanel("Select Wallpaper","","jpg"); // for Unity editor only.
			    Debug.Log("PATH IS ::: " + path);
				loadingMessage2 = "Please Wait...";
			if(path ==null || path.Equals(""))
			{
				loadingMessage2 = "Please Select A File";
			}
				uploadService = sp.BuildUploadService(); // Initializing Upload Service.
	            uploadService.UploadFile(fileName, path, "IMAGE", "Description", callBack); //Using App42 Unity UploadService.
			}
		//================================================================================	
		
		//======================================GetAllFiles===============================
		if (GUI.Button(new Rect(700, 465, 130, 30), "GetAllFiles"))
	        {
			    loadingMessage = "Loading...";
			    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
	            uploadService.GetAllFiles(allFilesCallBack); //Using App42 Unity UploadService
			}
		//================================================================================	
		
		//========Setting Up ScrollView====================================================	
		scrollPosition = GUI.BeginScrollView(new Rect(700, 40, 140, 400), scrollPosition, new Rect(0, 0, 120, listOfImages.Count*100));
		if(listOfImages.Count > 1){
			for(int i=0; i<listOfImages.Count; i++){
				Texture2D myImage = (Texture2D)listOfImages[i];
				GUILayout.Label(myImage,GUILayout.MaxHeight(100),GUILayout.MaxWidth(100));
			}
			loadingMessage = "";
		}
         GUI.EndScrollView();
		//========ScrollView===============================================================	
	
	} //OnGUI() Closed.

	
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
		  listOfImages.Add(www.texture);	
		}
		
	}
	
	protected void FileSelectedCallback(string pathImage)
	{
		m_fileBrowser = null;
		path = pathImage;
		showUploadBtn = true;
	}
	
}