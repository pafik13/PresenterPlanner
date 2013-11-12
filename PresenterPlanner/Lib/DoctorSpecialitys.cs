using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using System.IO;
using System.Xml.Serialization;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PresenterPlanner.Lib
{
	public class DoctorSpecialitys
	{
		static List<string> specialitys;
		static string storeLocation = "" ;	

		static DoctorSpecialitys ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			specialitys = new List<string> ();
			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<string>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					specialitys = (List<string>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<string>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, specialitys);
			}
		}

		public static string DatabaseFilePath {
			get { 
				return Path.Combine (Common.DatabaseFileDir(), "SpecialityDB.xml");;		
			}
		}


		public static List<string> GetSpecialitys ()
		{
			return specialitys;
		}

		public static void SaveSpeciality (String item)
		{ 
			if ((specialitys.Find (x => x == item) == null))
			{
				specialitys.Add(item);
			}
			WriteXml ();
		}
	}
}

