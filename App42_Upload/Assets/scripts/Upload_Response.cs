using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.upload;
namespace AssemblyCSharp
{
public class Upload_Response : MonoBehaviour,App42CallBack
	{
		 private string url1 = "";
		
		
		 public void OnSuccess(object upload)
        {
			Debug.Log("::::::::::::: "+upload.ToString());
                	Upload uploadObj = (Upload)upload;
					Debug.Log ("Upload Response : " + uploadObj);
					Debug.Log ("Name Is     : " + uploadObj.GetFileList()[0].GetName());
					Debug.Log ("UserName Is : " + uploadObj.GetFileList()[0].GetUserName());
					Debug.Log ("Url Is      : " + uploadObj.GetFileList()[0].GetUrl());
						url1 = uploadObj.GetFileList()[0].GetUrl();
					Debug.Log ("TinyUrl Is  : " + uploadObj.GetFileList()[0].GetTinyUrl());
					Debug.Log ("Description Is : " + uploadObj.GetFileList()[0].GetDescription());
				    Upload_Main.GetInstance().ExecuteGet(url1);
		}
	
		
        public void OnException(Exception e)
        {
			Debug.Log ("EXCEPTION  :  " + e);
		}
		
		public string GetURL() {
			return url1;
		}
		
		
	}
      
}
