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

	[Activity (Label = "Планировщик", MainLauncher = true, Icon = "@drawable/Icon_planner_72")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button btnSlides = FindViewById <Button> (Resource.Id.btnSlides);
			btnSlides.Click += (object sender, EventArgs e) => 
			{
				var presents = new Intent ( this, typeof(PresentationsList));
				StartActivity (presents);
			};

			Button btnPlanning = FindViewById <Button> (Resource.Id.btnPlanning);
			btnPlanning.Click += (object sender, EventArgs e) => 
			{
				var planning = new Intent ( this, typeof(PlannerGrid));
				StartActivity (planning);
			};

			Button btnData = FindViewById <Button> (Resource.Id.btnData);
			btnData.Click += (object sender, EventArgs e) => 
			{
				var data = new Intent ( this, typeof(DoctorsAndHospitals));
				StartActivity (data);
			};

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


