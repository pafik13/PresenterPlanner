using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using BitMapSerializer;

namespace PresenterPlanner
{
	[Activity (Label = "PresenterPlanner", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

//			Button btn = FindViewById <Button> (Resource.Id.btnData);
//			btn.Text = BMSer.GetDirectory();

			Button btn2 = FindViewById <Button> (Resource.Id.btnSlides);
			btn2.Click += (object sender, EventArgs e) => 
			{
				var presents = new Intent ( this, typeof(PresentationsList));
				StartActivity (presents);
			};

			Button btn3 = FindViewById <Button> (Resource.Id.btnPlanning);
			btn3.Click += (object sender, EventArgs e) => 
			{
				var planning = new Intent ( this, typeof(PlannerGrid));
				StartActivity (planning);
			};

			Button btn4 = FindViewById <Button> (Resource.Id.btnData);
			btn4.Click += (object sender, EventArgs e) => 
			{
				var data = new Intent ( this, typeof(DoctorsAndHospitals));
				StartActivity (data);
			};

		//	string storage = Path.Combine(BMSer.GetDirectory(),"MyTempDir");
		//	File f = new File (storage + "t.xml");
//			var serializer = new XmlSerializer (typeof(BMSer));
//			var filestream = new FileStream (Path.Combine(BMSer.GetDirectory(),"MyTempDir","test.xml"), FileMode.Open); 
//			BMSer bm = (BMSer)serializer.Deserialize (filestream);
//			btn2.Text = bm.Name;

//			btn2.Click += (object sender, EventArgs e) => {
//				ImageView iv = FindViewById <ImageView> (Resource.Id.imageView1);
//				iv.SetImageBitmap(bm.MyImage);
//			};

//			LinearLayout lv = FindViewById <LinearLayout> (Resource.Id.llFirstRow);
//
//			Button btn3 = new Button (this);
//			btn3.Text = "Buttom 3";
//
//				lv.AddView ( btn3);
			// Get our button from the layout resource,
			// and attach an event to it
//			WebClient client = new WebClient();
//			Byte[] pageData = client.DownloadData("http://www.contoso.com");
//			string pageHtml = Encoding.ASCII.GetString(pageData);
//
//			TextView tv_Inet = FindViewById<TextView> (Resource.Id.tv_Inet);
//			tv_Inet.Text = pageHtml;
//			Console.WriteLine(pageHtml);

		}
	}
}


