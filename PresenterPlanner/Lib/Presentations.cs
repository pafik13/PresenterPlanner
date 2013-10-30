using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Graphics;
using Android.Widget;

namespace PresenterPlanner.Presentations
{
	public class Presentations
	{
		static string storeLocation;
		static List <Presentation> presents;

		static Presentations(){
			storeLocation = DatabaseFilePath();
			presents = new List<Presentation> ();
			ReadXml ();
		}

		static void ReadXml(){
			if (System.IO.File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Presentation>));
				using (var stream = new System.IO.FileStream (storeLocation, System.IO.FileMode.Open)) {
					presents = (List<Presentation>)serializer.Deserialize (stream);
				}
			}
		}

		static string DatabaseFilePath(){
			return System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,"MyTempDir","presents.xml");
		}

		public static List<Presentation> GetPresentations(){
			return new List<Presentation>((IEnumerable<Presentation>)presents);
		}
	}

	public class Presentation
	{
		[XmlIgnoreAttribute()]
		public Button btn;

		public string name;
		public int imgIdx;
		public List<Part> parts;
	}

	public class Part
	{
		[XmlIgnoreAttribute()]
		public Button btn;

		public string name;
		public int imgIdx;
		public List<MyBitMap> slides = new List<MyBitMap>();
	}

	public class MyBitMap
	{
		[XmlIgnoreAttribute()]
		public Bitmap Image;

		// Serializes the 'Picture' Bitmap to XML.
		[XmlElementAttribute("Picture")]
		public byte[] PictureByteArray
		{
			get
			{
				TypeConverter BitmapConverter = TypeDescriptor.GetConverter(Image.GetType());
				return (byte[])BitmapConverter.ConvertTo(Image, typeof(byte[]));
			}

			set
			{
				Image = BitmapFactory.DecodeStream(new System.IO.MemoryStream(value)) ;
			}
		}
	}
}

