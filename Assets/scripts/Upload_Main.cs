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
	public Texture2D uploadedImage;
	
	//=============================FILE_BROWSER_CONTENT========================================
	
	
	//=============================INITIALIZING_SERVICES==========================================
	Upload_Response callBack = new Upload_Response();   // Making callBack Object for Upload_Response.
	AllFiles_Response allFilesCallBack = new AllFiles_Response();   // Making callBack Object for AllFiles_Response.
	ServiceAPI sp = null;                            // Initializing Service API.
	UploadService uploadService;             // Initialising Upload Service.
	private static Upload_Main con = null;
	//===========================================================================================
	
	//=============================DEFINING-PARAMETERS==========================================
	public static string fileName;
	public string fileType = "IMAGE"; 
	public string description = "Image Description";
	protected string path;
	//=============================DEFINING-PARAMETERS===========================================
	
	
	//=============================GUI_CONTENT===========================================
	public Vector2 scrollPosition = Vector2.zero;
	public static bool showBtn = true;
	public static bool showAllFilesBtn = true;
	public static bool showUploadBtn = false;
	public string loadingMessage;
	public string loadingMessage2;
	
    IList<object> listOfImages = new List<object>();
	//=============================GUI_CONTENT===========================================
		
	// Use this for initialization
	void Start () 
	{
		sp = new ServiceAPI("YOUR_API_KEY","YOUR_SECRET_KEY");
        uploadService = sp.BuildUploadService(); // Initializing Upload Service.
    }
	
	
  	 protected void OnGUIMain() 
		{
		    if (GUILayout.Button("Select File" , GUILayout.Width(150), GUILayout.Height(30)))
	        	{
			showAllFilesBtn = false;
					m_fileBrowser = new FileBrowser(
							new Rect(50, 50, 400, 500),
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
	    GUILayout.BeginVertical();
		
		GUILayout.Label(uploadedImage,GUILayout.Height(317),GUILayout.Width(150));
 			
		//======================================Loading_Message==========================================	
		GUILayout.Label(loadingMessage2);
		//======================================Loading_Message==========================================	
		
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
	
		GUILayout.Label(path ?? "None selected");
		GUILayout.BeginHorizontal();
	    //======================================UploadFile==========================================
		if (showAllFilesBtn == true && showUploadBtn == true && showBtn == true && GUILayout.Button("Upload File" , GUILayout.Width(150), GUILayout.Height(30)))   
		{
			
				showUploadBtn = false;
			    showBtn = false;
			    Debug.Log("PATH IS ::: " + path);
				loadingMessage2 = "Please Wait...";
			if(path ==null || path.Equals(""))
			{
				loadingMessage2 = "Please Select A File";
			}
			    fileName = "testFile11"+DateTime.Now.Millisecond; 
				uploadService = sp.BuildUploadService(); // Initializing Upload Service.
	            uploadService.UploadFile(fileName, path, "IMAGE", "Description", callBack); //Using App42 Unity UploadService.
			}
		//================================================================================
		GUILayout.EndVertical();
		GUILayout.Space(40);
		GUILayout.BeginArea(new Rect(155,5,200,371));
		GUILayout.BeginVertical();
		//========Setting Up ScrollView====================================================	
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(155));
		if(listOfImages.Count > 1)
		{
			for(int i=0; i<listOfImages.Count; i++)
			{
				Texture2D myImage = (Texture2D)listOfImages[i];
				GUILayout.Label(myImage,GUILayout.Height(100),GUILayout.Width(100));
			}
			loadingMessage = "";
		}
         GUILayout.EndScrollView();
		//========ScrollView===============================================================	
		
		//======================================Loading_Message==========================================	
		GUILayout.Label(loadingMessage);
		//======================================Loading_Message==========================================	
		
		//======================================GetAllFiles===============================
		if (showAllFilesBtn == true && GUILayout.Button("Get All Files" , GUILayout.Width(150), GUILayout.Height(30)))   
	        {
			    loadingMessage = "Loading...";
			    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
	            uploadService.GetAllFiles(allFilesCallBack);
			}
		//================================================================================	
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	
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
		   uploadedImage = www.texture;
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
		showAllFilesBtn = true;
	}
	
}