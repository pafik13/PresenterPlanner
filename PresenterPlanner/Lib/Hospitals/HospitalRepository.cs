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
				return Path.Combine (Common.DatabaseFileDir(), "HospitalDB.xml");		
			}
		}

		public static Hospital GetHospital (int id)
		{
			for (var t = 0; t < hospitals.Count; t++) {
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

		public static IEnumerable<Hospital> GetAvailableHospitals (int weekNum, DayOfWeek dayOfWeek)
		{
			List<Hospital> availableHospitals = new List<Hospital> ();
			for (int h = 0; h < hospitals.Count; h++) {
				int countOfWeek = 0;
				int countDayOfWeek = 0;
				for (int p = 0; p < hospitals[h].planners.Count; p++) {
					if (hospitals[h].planners[p].weekNum == weekNum) {
						countOfWeek++;
						if (hospitals [h].planners [p].dayOfWeek == dayOfWeek) {
							countDayOfWeek++;
						}
					}

				}
				if ((countDayOfWeek < 1) && (countOfWeek < 2)) {
					availableHospitals.Add (hospitals [h]);
				} 
			};
			return availableHospitals;
		}

		public static IEnumerable<Hospital> GetChoosenHospitals (int weekNum, DayOfWeek dayOfWeek)
		{
			List<Hospital> choosenHospitals = new List<Hospital> ();
			for (int h = 0; h < hospitals.Count; h++) {
				for (int p = 0; p < hospitals[h].planners.Count; p++) {
					if (hospitals [h].planners [p].weekNum   == weekNum &&
					    hospitals [h].planners [p].dayOfWeek == dayOfWeek) {
						choosenHospitals.Add (hospitals [h]);
					}
				}
			};
			return choosenHospitals;
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