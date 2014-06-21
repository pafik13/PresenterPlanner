using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PresenterPlanner.Lib;

namespace PresenterPlanner.Lib
{
	public class DemonstrationRepository
	{
		static string storeLocation;	
		static List<Demonstration> demonstrations;
		static List<string> tempFiles;

		static DemonstrationRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			demonstrations = new List<Demonstration> ();
			tempFiles = new List<string> ();
			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Demonstration>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					demonstrations = (List<Demonstration>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<Demonstration>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, demonstrations);
			}
		}

		public static void ReReadDemonstrations ()
		{
			ReadXml ();
		}

		static List<Demonstration> ReadFromFile (string filePath)
		{
			List <Demonstration> demons = new List<Demonstration> ();
			if (File.Exists(filePath)) {
				var serializer = new XmlSerializer (typeof(List<Demonstration>));
				using (var stream = new FileStream (filePath, FileMode.Open)) {
					demons = (List<Demonstration>)serializer.Deserialize (stream);
				}
			}
			return demons;
		}

		static string WriteToFile (Demonstration[] demons)
		{
			string filePath =  Path.Combine(Common.DatabaseFileDir(), Path.GetRandomFileName() + ".xml");
			var serializer = new XmlSerializer (typeof(Demonstration[]));
			using (var writer = new StreamWriter (filePath)) {
				serializer.Serialize (writer, demons);
			}
			return filePath;
		}

		public static void WriteToArchive ()
		{
			string archiveLocation =  Path.Combine(Common.DatabaseFileDir(), "Archive", Path.GetRandomFileName() + ".xml");
			var serializer = new XmlSerializer (typeof(List<Demonstration>));
			using (var writer = new StreamWriter (archiveLocation)) {
				serializer.Serialize (writer, demonstrations);
			}
			demonstrations.Clear ();
			WriteXml ();
		}

		public static bool SplitFile(string filePath) 
		{
			FileInfo fi = new FileInfo (filePath);
			List <Demonstration> demons = ReadFromFile (filePath);
			if (fi.Length <= 600 * 1024) {
				if (filePath != DatabaseFilePath) { File.Delete (filePath); }
				tempFiles.Add (WriteToFile (demons.ToArray ()));
				return true;
			} else {
				if (filePath != DatabaseFilePath) { File.Delete (filePath); }
				int mediumCnt = demons.Count / 2;

				Demonstration[] d1 = new Demonstration[mediumCnt]; 
				demons.CopyTo (0, d1, 0, mediumCnt);

				Demonstration[] d2 = new Demonstration[demons.Count - mediumCnt]; 
				demons.CopyTo (mediumCnt, d2, 0, demons.Count - mediumCnt );

				return SplitFile(WriteToFile (d1)) && SplitFile(WriteToFile (d2));
			}
		}

		public static List<string> GetSplitFiles() {
			List<string> files = new List<string> ();
			if (SplitFile (DatabaseFilePath)) {
				files = tempFiles.ToList();
				tempFiles.Clear ();
			} else {
				files.Add (DatabaseFilePath);
			}
			return files;
		}

		public static string DatabaseFilePath {
			get { 		
				return Path.Combine (Common.DatabaseFileDir(), "Demonstrations.xml");		
			}
		}

		public static Demonstration GetDemonstration(int docID, DateTime vDate)
		{
			for (int d = 0; d < demonstrations.Count; d++) {
				if ((demonstrations [d].doctorID == docID) && (demonstrations [d].visitDate.Date == vDate.Date)){
					return demonstrations[d];
				}
			}
			return new Demonstration() {doctorID = docID, visitDate = vDate, demos = new List<Demo>()};
		}

		public static Demonstration GetLastDemonstration(int docID)
		{
			if (demonstrations.Count == 0) {
				return null;
			} else {
				List<Demonstration> docDemos = demonstrations.Where (d => d.doctorID == docID).ToList ();
				if (docDemos.Count == 0) {
					return null;
				} else {
					DateTime maxVisitDate = docDemos.Max (d => d.visitDate);
					return GetDemonstration (docID, maxVisitDate);
				} 
			}
		}

		public static int SaveDemonstration (Demonstration item)
		{
			Demonstration demo = null;
			for (int d = 0; d < demonstrations.Count; d++) {
				if ((demonstrations [d].doctorID == item.doctorID) && (demonstrations [d].visitDate == item.visitDate)){
					demo = demonstrations[d];
				}
			}
			if ( demo == null ) {
				demo = GetLastDemonstration (item.doctorID);
				if (demo == null) {
					demonstrations.Add (item);
				} else {
					item.analyze = demo.analyze;
					item.commentForPharmacy = demo.commentForPharmacy;
					item.POSmaterials = demo.POSmaterials;
					demonstrations.Add (item);
				}
			} else {
				demo = item; // replaces item in collection with updated value
			}

			WriteXml ();
			return item.doctorID;
		}
	}
}

