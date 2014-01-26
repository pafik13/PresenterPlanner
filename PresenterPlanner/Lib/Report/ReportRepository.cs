using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace PresenterPlanner.Lib.Report
{
	public class ReportRepository
	{
		static string storeLocation;	
		static List<Report> reports;

		static ReportRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			reports = new List<Report> ();

			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Report>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					reports = (List<Report>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<Report>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, reports);
			}
		}

		public static string DatabaseFilePath {
			get {
				return Path.Combine (Common.DatabaseFileDir(), "ReportDB.xml");		
			}
		}

		public static IEnumerable<Report> GetReports ()
		{
			return reports;
		}

		public static Report GetReport (int doctorID)
		{
			for (int i = 0; i < reports.Count; i++) {
				if (reports [i].doctorID == doctorID) {
					return reports [i];
				}
			}
			return null;
		}

		public static bool SaveReports (List<Report> saveReports) {
			try {
				reports = saveReports;
			  	WriteXml ();
				return true;
			}
			catch (Exception e) {
				return false;
			}
		}

		public static int GetMinWeekNum () {
			int minWeekNum = 52; //#hack
			foreach (var report in reports) {
				foreach (var ri in report.rItems) {
					if (ri.weekNum < minWeekNum) { minWeekNum = ri.weekNum; }
				}
			}
			return minWeekNum;
		}

	}
}

