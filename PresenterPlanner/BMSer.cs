using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using Android.Graphics;
using System;

namespace BitMapSerializer
{
    public class BMSer
    {
        public string Name;

        [XmlIgnoreAttribute()]
		public Bitmap MyImage;

        // Serializes the 'Picture' Bitmap to XML.
        [XmlElementAttribute("Picture")]
        public byte[] PictureByteArray
        {
            get
            {
                TypeConverter BitmapConverter = TypeDescriptor.GetConverter(MyImage.GetType());
                return (byte[])BitmapConverter.ConvertTo(MyImage, typeof(byte[]));
            }

            set
            {
				MyImage = BitmapFactory.DecodeStream(new MemoryStream(value)) ;
            }
        }

		public static string GetDirectory ()
		{
			
			return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
		}
 
    }
}
