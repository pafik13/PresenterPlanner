using System;
using System.IO;
using System.Net;

using Android.Net;
using Android.App;
using Android.Content;

namespace PresenterPlanner.Lib
{
	public class DownLoad
	{
		public static bool HasConnection (Activity activity) {
			ConnectivityManager connectivityManager = (ConnectivityManager)activity.GetSystemService(Context.ConnectivityService);
			NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
			return ((activeConnection != null)  && activeConnection.IsConnected);
		}

//		public static bool DownloadFile(string sSourceURL, string sDestinationPath, Activity activity)
//		{
//			if (!HasConnection(activity)) {
//				return false;
//			}
//
//			long contentTotalLength = 0;
//			FileStream saveFileStream;
//			if (File.Exists (sDestinationPath)) {
//				saveFileStream = new FileStream (sDestinationPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
//			} else {
//				saveFileStream = new FileStream (sDestinationPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
//			}
//
//			contentTotalLength = WebRequest.Create(sSourceURL).GetResponse().ContentLength;
//
//			HttpWebRequest hwRq = (HttpWebRequest)WebRequest.Create(sSourceURL);
//			hwRq.AddRange(saveFileStream.Length);
//			HttpWebResponse hwRes = (HttpWebResponse)hwRq.GetResponse();
//
//			if (saveFileStream.Length != hwRes.ContentLength) {
//				int loadedBytes = 0;
//				int  dlBufferSize= 1024 * 500; // 500 КБ
//				byte[] dlBuffer = new byte[dlBufferSize]; 
//				Stream smRespStream = hwRes.GetResponseStream();
//				while (HasConnection(activity) && ((loadedBytes = smRespStream.Read(dlBuffer, 0, dlBufferSize)) > 0)) {
//					saveFileStream.Write(dlBuffer, 0, loadedBytes);
//				}
//			}
//
//			if (saveFileStream.Length == contentTotalLength) {
//				saveFileStream.Close ();
//				return true;
//			}
//
//			saveFileStream.Close ();
//			return false;
//		}  
	}
}

