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

		static DemonstrationRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			demonstrations = new List<Demonstration> ();

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

