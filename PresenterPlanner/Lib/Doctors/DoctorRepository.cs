using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using PresenterPlanner.Lib;

namespace PresenterPlanner.Lib.Doctors
{
	public class DoctorRepository
	{
		static string storeLocation;	
		static List<Doctor> doctors;

		static DoctorRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			doctors = new List<Doctor> ();

			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Doctor>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					doctors = (List<Doctor>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<Doctor>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, doctors);
			}
		}

		public static string DatabaseFilePath {
			get { 		
				return Path.Combine (Common.DatabaseFileDir(), "DoctorDB.xml");		
			}
		}

		public static Doctor GetDoctor(int id)
		{
			for (var t = 0; t < doctors.Count; t++) {
				if (doctors[t].ID == id)
					return doctors[t];
			}

			var vwtDays = new WorkTime_Days ();
			vwtDays.Mon_From = new DateTime(1,1,1,9,0,0);
			vwtDays.Mon_Till = new DateTime(1,1,1,18,0,0);
			vwtDays.Tue_From = new DateTime(1,1,1,9,0,0);
			vwtDays.Tue_Till = new DateTime(1,1,1,18,0,0);
			vwtDays.Wed_From = new DateTime(1,1,1,9,0,0);
			vwtDays.Wed_Till = new DateTime(1,1,1,18,0,0);
			vwtDays.Thu_From = new DateTime(1,1,1,9,0,0);
			vwtDays.Thu_Till = new DateTime(1,1,1,18,0,0);
			vwtDays.Fri_From = new DateTime(1,1,1,9,0,0);
			vwtDays.Fri_Till = new DateTime(1,1,1,18,0,0);
//			vwtDays.Sut_From = new DateTime(1,1,1,9,0,0);
//			vwtDays.Sut_Till = new DateTime(1,1,1,18,0,0);
//			vwtDays.Sun_From = new DateTime(1,1,1,9,0,0);
//			vwtDays.Sun_Till = new DateTime(1,1,1,18,0,0);

			var vwtOddEven = new WorkTime_OddEven ();
			vwtOddEven.Odd_From = new DateTime(1,1,1,9,0,0);
			vwtOddEven.Odd_Till = new DateTime(1,1,1,18,0,0);
			vwtOddEven.Even_From = new DateTime(1,1,1,9,0,0);
			vwtOddEven.Even_Till = new DateTime(1,1,1,18,0,0);

			return new Doctor() {ID=id, wtDays = vwtDays, wtOddEven = vwtOddEven};
		}

		public static IEnumerable<Doctor> GetDoctors ()
		{
			return doctors;
		}

		public static IEnumerable<Doctor> GetDoctors (int hospitalID)
		{
			List<Doctor> newDoctorList = new List<Doctor> ();
			for (var t = 0; t< doctors.Count; t++) {
				if (doctors [t].HospitalID == hospitalID) {
					newDoctorList.Add(doctors [t]);
				};
			};
			return newDoctorList;
		}

		public static IEnumerable<Doctor> GetSelectedDoctors ()
		{
			List<Doctor> selectedDoctors = new List<Doctor> ();
			for (var t = 0; t< doctors.Count; t++) {
				if (doctors [t].IsChosen) {
					selectedDoctors.Add(doctors [t]);
				};
			};
			return selectedDoctors;
		}

		/// <summary>
		/// Insert or update a task
		/// </summary>
		public static int SaveDoctor (Doctor item)
		{ 
			var max = 0;
			if (doctors.Count > 0) 
				max = doctors.Max(x => x.ID);

			if (item.ID == 0) {
				item.ID = ++max;
				doctors.Add (item);
			} else {
				var i = doctors.Find (x => x.ID == item.ID);
				i = item; // replaces item in collection with updated value
			}

			WriteXml ();
			return max;
		}

		public static int DeleteDoctor (int id)
		{
			for (var t = 0; t< doctors.Count; t++) {
				if (doctors[t].ID == id){
					doctors.RemoveAt (t);
					WriteXml ();
					return 1;
				}
			}
			return -1;
		}
	}
}