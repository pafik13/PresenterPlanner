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
				var storeFilename = "SpecialityDB.xml";
				#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
				#else

				#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
				#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, storeFilename);
				#endif		
				return path;		
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

