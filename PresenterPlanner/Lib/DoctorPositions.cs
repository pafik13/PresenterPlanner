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
	public class DoctorPositions
	{
		static List<string> positions;
		static string storeLocation = "" ;	

		static DoctorPositions ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			positions = new List<string> ();
			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<string>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					positions = (List<string>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<string>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, positions);
			}
		}

		public static string DatabaseFilePath {
			get { 
				return Path.Combine (Common.DatabaseFileDir(), "PositionDB.xml");;		
			}
		}
		

		public static List<string> GetPositions ()
		{
			return positions;
		}

		public static void SavePosition (String item)
		{ 
			if ((positions.Find (x => x == item) == null))
			{
				positions.Add(item);
			}
			WriteXml ();
		}
	}
}

