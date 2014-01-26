using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using Android.Widget;
using Android.Content;
using Android.Telephony;

using PresenterPlanner.Lib;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

namespace PresenterPlanner
{
	public class UploadFiles
	{
		static WebClient wc = null;
		static string imei = "";
		static SyncView sc = null;
		static List<string> fileTypes = null;
		static int pos = 0;
		static bool isNeedUploading = false;

		public UploadFiles (SyncView syncView){
			sc = syncView;
			wc = new WebClient ();

			TelephonyManager manager= (TelephonyManager)sc.GetSystemService(Context.TelephonyService);
			imei = manager.DeviceId;
			pos = 0;
			isNeedUploading = false;

			fileTypes = new List<string> ();
			fileTypes.Add ("HospitalDB");
			fileTypes.Add ("DoctorDB");
			fileTypes.Add ("Demonstration");

			wc.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => {
				//XmlDocument doc = new XmlDocument();
				//doc.LoadXml(e.Result);
				//() =>Toast.MakeText(sc, fileTypes[pos] + ": " + e.Result, ToastLength.Short).Show()); //doc.GetElementById ("message").InnerText
				Console.WriteLine (fileTypes[pos] + ": " + e.Result);
				Console.WriteLine ("FILE NEED UPLOAD: " + e.Result.Contains("FILE NEED UPLOAD").ToString());
				isNeedUploading = isNeedUploading || e.Result.Contains("FILE NEED UPLOAD");
				pos = pos + 1;
				if (fileTypes.Count == pos) {
					if (isNeedUploading) {
						sc.RunOnUiThread(() => sc.btnUploadFiles.Visibility = Android.Views.ViewStates.Visible);
						sc.RunOnUiThread(() => sc.btnUploadFiles.Text = "Необходима синхронизация");
					}
					sc.CheckNeWVersion();
					return;
				} else {
					CheckFiles ();
					return;
				}
			};
		}

		public void CheckFiles () {
			CheckFileAsync (fileTypes [pos]);
		}

		protected void CheckFileAsync (string type) {
			string filePath = "";
			switch (type) {
			case "HospitalDB":
				filePath = HospitalRepository.DatabaseFilePath;
				break;

			case "DoctorDB":
				filePath = DoctorRepository.DatabaseFilePath;
				break;

			case "Demonstration":
				filePath = DemonstrationRepository.DatabaseFilePath;
				break;
			}

			string fileText = File.ReadAllText (filePath);
			string hash = GetMd5Hash (fileText);

			var url = string.Format("http://sbl.webatu.com/index.php?r=uploadFiles/checkFile&imei={0}&type={1}&hash={2}", imei, type, hash);
			Console.WriteLine ("URL: " + url);
			wc.DownloadStringAsync (new Uri (url));
		}

		public static string LoadFile (string type) {
			string filePath = "";
			switch (type) {
			case "HospitalDB":
				filePath = HospitalRepository.DatabaseFilePath;
				break;

			case "DoctorDB":
				filePath = DoctorRepository.DatabaseFilePath;
				break;

			case "Demonstration":
				filePath = DemonstrationRepository.DatabaseFilePath;
				break;
			}

			string fileText = File.ReadAllText (filePath);
			string hash = GetMd5Hash (fileText);

			var url = string.Format("http://sbl.webatu.com/index.php?r=uploadFiles/checkFile&imei={0}&type={1}&hash={2}", imei, type, hash);
			Console.WriteLine ("URL: " + url);

			if (wc.DownloadString(url).Contains ("FILE NEED UPLOAD")) {
				Console.WriteLine ("Prepare to uploading...");
				return UpLoad.PostMultipart (
					@"http://sbl.webatu.com/index.php?r=uploadFiles/parseFile",
					new Dictionary<string, object> () {
						{ "imei", imei },
						{ "type", type },
						{ "hash", hash }, 
					    { "userfile", new FormFile() { Name = Path.GetFileName(filePath), ContentType = "text/xml", FilePath = filePath } },
				});
			} else return "NOT UPLOAD";
		}

		public static string DownLoadReport () {
			var url = string.Format("http://sbl.webatu.com/index.php?r=uploadFiles/report&imei={0}", imei);
			return wc.DownloadString (url);
		}

		static string GetMd5Hash (string input)
		{
			MD5 md5Hash = MD5.Create();
			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hash.ComputeHash (Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
	}
}

