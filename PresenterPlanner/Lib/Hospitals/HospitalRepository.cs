using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace PresenterPlanner.Lib.Hospitals
{
	public class HospitalRepository
	{
		static string storeLocation;	
		static List<Hospital> hospitals;

		static HospitalRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			hospitals = new List<Hospital> ();

			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Hospital>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					hospitals = (List<Hospital>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<Hospital>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, hospitals);
			}
		}

		public static string DatabaseFilePath {
			get { 
				var storeFilename = "HospitalDB.xml";
//				#if SILVERLIGHT
//				// Windows Phone expects a local path, not absolute
//				var path = sqliteFilename;
//				#else
//
//				#if __ANDROID__
//				// Just use whatever directory SpecialFolder.Personal returns
//				string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//				#else
//				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
//				// (they don't want non-user-generated data in Documents)
//				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
//				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
//				#endif
				var path = Path.Combine (Common.DatabaseFileDir(), storeFilename);
//				#endif		
				return path;		
			}
		}

		public static Hospital GetHospital(int id)
		{
			for (var t = 0; t< hospitals.Count; t++) {
				if (hospitals[t].ID == id)
					return hospitals[t];
			}
			return new Hospital() {ID=id};
		}

		public static IEnumerable<Hospital> GetHospitals ()
		{
			return hospitals;
		}

		public static IEnumerable<Hospital> GetSelectedHospitals ()
		{
			List<Hospital> selectedHospitals = new List<Hospital> ();
			for (var t = 0; t< hospitals.Count; t++) {
				if (hospitals [t].IsChosen) {
					selectedHospitals.Add(hospitals [t]);
				};
			};
			return selectedHospitals;
		}

		/// <summary>
		/// Insert or update a task
		/// </summary>
		public static int SaveHospital (Hospital item)
		{ 
			var max = 0;
			if (hospitals.Count > 0) 
				max = hospitals.Max(x => x.ID);

			if (item.ID == 0) {
				item.ID = ++max;
				hospitals.Add (item);
			} else {
				var i = hospitals.Find (x => x.ID == item.ID);
				i = item; // replaces item in collection with updated value
			}

			WriteXml ();
			return max;
		}

		public static int DeleteHospital (int id)
		{
			for (var t = 0; t< hospitals.Count; t++) {
				if (hospitals[t].ID == id){
					hospitals.RemoveAt (t);
					WriteXml ();
					return 1;
				}
			}
			return -1;
		}
	}
}