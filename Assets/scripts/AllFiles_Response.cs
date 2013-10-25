using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.upload;
namespace AssemblyCSharp
{
public class AllFiles_Response : MonoBehaviour,App42CallBack
	{
		 private string url1 = "";
		
		
		 public void OnSuccess(object upload)
        {
			Debug.Log("::::::::::::: "+upload.ToString());
                	Upload uploadObj = (Upload)upload;
					IList<Upload.File> fileListt = uploadObj.GetFileList();
			for (int i = 0; i < fileListt.Count; i++)
                {
                    Debug.Log ("Url Is      : " + fileListt[i].GetUrl());
					Upload_Main.GetInstance().ExecuteShow(fileListt[i].GetUrl());
                }
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
