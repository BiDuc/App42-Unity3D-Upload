App42-Unity3D-Upload
====================

App42 Unity3D SDK Upload Sample.

This is a sample for uploading a local file to the http through unity.

_Features List :_

1. Upload File On the Cloud.
2. Get All Files which are priviously Uploaded.
3. Support all unity3D supported platforms, i.e ANDROID, STANDALONE_WIN, STANDALONE_OSX, IPHONE etc.

# Running Sample:

1. [Register] (https://apphq.shephertz.com/register) with App42 platform
2. Go to dashboard and click on the Create App.
3. Fill all the mandatory fields to get your APIKey and SecretKey.
4. Download App42 Unity3D Upload Sample App and unzip it on your machine.
5. Import project in unity editor.

__Initialize App42 :__

In order to use the various functions available in a specific API. 
A developer has to create an instance of ServiceAPI by passing the apiKey and secretKey which will be created after the app creation from AppHQ dashboard.

Edit Upload_Main.cs script and put your API_KEY and SECRET_KEY (which were received in step#2 & #3) as shown below.
```
void Start () 
{
ServiceAPI sp = new ServiceAPI("YOUR_API_KEY","YOUR_SECRET_KEY");
}
```
__Import Statement__
```
using com.shephertz.app42.paas.sdk.csharp;  
using com.shephertz.app42.paas.sdk.csharp.upload;  
```

#Design Details:

__App42 Upload:__

Initialize App42Upload
To build an instance of UploadService, BuildUploadService() method needs to be called.

```
  UploadService uploadService = sp.BuildUploadService(); // Initializing Upload Service.
```

Upload File On the Cloud.
For uploading a file, we need at least a filename, filepath from where we want to upload files, filetype i.e IMAGE or VIDEO etc, description about file, and a callback.

```
public class Upload_Main : MonoBehaviour 
{
    public static string fileName; // The name of the file which has to be saved.
    public string filePath;         // The local path for the file, for this I use "Improved file browser" by Daniel Brauer.
    public string fileType = "IMAGE"; //The type of the file. File can be either Audio, Video, Image, Binary, Txt, xml, json, csv or other Use the static constants e.g. Upload.AUDIO, Upload.XML etc.
    public string description = "Image Description"; // Description of the file to be uploaded.
    Upload_Response callBack = new Upload_Response();   // Making callBack Object for Upload_Response.
  void OnGUI()
  {		
    if (showAllFilesBtn == true && showUploadBtn == true && GUILayout.Button("Upload File" , GUILayout.Width(150), GUILayout.Height(30)))   
    {
      showUploadBtn = false; // For making the button hide when clicked.
      Debug.Log("PATH IS ::: " + path);
      loadingMessage2 = "Please Wait...";
      if(path ==null || path.Equals(""))
      {
        loadingMessage2 = "Please Select A File";
      }
      fileName = "ImageFile"+DateTime.Now.Millisecond; // So that FileName changes when the button clicks.
      uploadService = sp.BuildUploadService(); // Initializing Upload Service.
      uploadService.UploadFile(fileName, path, "IMAGE", "Description", callBack); //Using App42 Unity UploadService.
    }
  }
}
```
Here is the CallBack for Response of Uploaded File :-
```
public class Upload_Response : MonoBehaviour, App42CallBack
{
  private string imageUrl = "";
  		
  public void OnSuccess(object upload)
  {
    Upload uploadObj = (Upload)upload;
    Debug.Log ("Upload Response : " + uploadObj);
    Debug.Log ("Name Is     : " + uploadObj.GetFileList()[0].GetName());
    Debug.Log ("Url Is      : " + uploadObj.GetFileList()[0].GetUrl());
    imageUrl = uploadObj.GetFileList()[0].GetUrl();  // fetching imageURL for Later Use Whenever/Whereever I Want.
    Debug.Log ("TinyUrl Is  : " + uploadObj.GetFileList()[0].GetTinyUrl());
    Debug.Log ("Description Is : " + uploadObj.GetFileList()[0].GetDescription());
    Upload_Main.GetInstance().ExecuteGet(imageUrl);   // Showing Uploaded Image.
  }
  		
  public void OnException(Exception e)
  {
    Debug.Log ("EXCEPTION  :  " + e);
  }
}	
```
Now We have to show the Uploaded Image. So, assign image to a Texture2D.
For this Here is a simple Unity Script :-	
```
IEnumerator execute (string url)
{
  WWW www = new WWW (url);
  while (!www.isDone) 
  {
    yield return null;  
  }
  if (www.isDone) 
  {
    loadingMessage2 = "";
    showBtn = true;
    uploadedImage = www.texture; // showing the image on UnityEngine.Texture2D i.e uploadedImage Texture2D defined above.
  }
}
```
This task is done successfully now we have to fetch all the Images Which are Priviously uploaded :-
The GetAllFiles method of App42 Unity3D Upload Service lets you Gets all the files for your App, which are previously uploaded through same API_KEY and SECRET_KEY.
```
AllFiles_Response allFilesCallBack = new AllFiles_Response();   // Making callBack Object for AllFiles_Response.
void OnGUI()
{
    if (showAllFilesBtn == true && GUILayout.Button("Get All Files" , GUILayout.Width(150), GUILayout.Height(30)))   
  {
    loadingMessage = "Loading...";
    uploadService = sp.BuildUploadService(); // Initializing Upload Service.
    uploadService.GetAllFiles(allFilesCallBack); //Getting All Files.
  }
}
```
Here is the CallBack for Response of Getting All Files :-
```
public class AllFiles_Response : MonoBehaviour, App42CallBack
{
  public void OnSuccess(object upload)
  {
    Upload uploadObj = (Upload)upload;
    IList<Upload.File> fileList = uploadObj.GetFileList();
    for (int i = 0; i < fileList.Count; i++)
    {
      Debug.Log ("Url Is      : " + fileList[i].GetUrl()); // Getting Urls Of Files.
      Upload_Main.GetInstance().ExecuteShow(fileList[i].GetUrl()); // Showing These Files On UI.
    }
  }
  
  public void OnException(Exception e)
  {
    Debug.Log ("EXCEPTION  :  " + e);
  }
}
```
Now We have to show the All Images Which Are Fetched, On the Unity GUILayout.Scrollview.
For this Here is a simple Unity Script:-
```
IList<object> listOfImages = new List<object>();
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
```
