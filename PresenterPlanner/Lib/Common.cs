using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Xml.Serialization;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

namespace PresenterPlanner.Lib
{
	public class Common
	{
		static string storeLocation;	
		static Setts settings;

		static Common ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			settings = new Setts ();

			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(Setts));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					settings = (Setts)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(Setts));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, settings);
			}
		}

		public static Setts GetSettings () {
			return settings;
		}

		public static void SetSettings (Setts sett) {
			settings = sett;
			WriteXml ();
		}

		public static string DatabaseFilePath {
			get { 	
				return Path.Combine (Common.DatabaseFileDir(), "Seetings.xml");		
			}
		}

		public static void SetCheck (View v, Doctor doctor) {
			var img = (ImageView)v.FindViewWithTag("imgCheck");
			img.Visibility = ViewStates.Gone; 
//			if(doctor.IsChosen) {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOnBackground);
//			} else {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOffBackground);
//			}
		}

		public static void SetCheck (View v, Hospital hospital) {
			var img = (ImageView) v.FindViewWithTag("imgCheck");
			img.Visibility = ViewStates.Gone; 
//			if(hospital.IsChosen) {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOnBackground);
//			} else {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOffBackground);
//			}
		}	

		public static string DatabaseFileDir(){
			return System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,"MyTempDir");
		}	
	}

	public struct Setts
	{
		public int weekOfStart;
		public string phone;
	}
}

