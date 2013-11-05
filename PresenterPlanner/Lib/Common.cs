using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

namespace PresenterPlanner.Lib
{
	public class Common
	{
		public static void SetCheck (View v, Doctor doctor) {
			var img = (ImageView)v.FindViewWithTag("imgCheck");
			if(doctor.IsChosen) {
				img.SetImageResource(Android.Resource.Drawable.CheckboxOnBackground);
			} else {
				img.SetImageResource(Android.Resource.Drawable.CheckboxOffBackground);
			}
		}

		public static void SetCheck (View v, Hospital hospital) {
			var img = (ImageView) v.FindViewWithTag("imgCheck");
			img.Visibility = ViewStates.Gone; 
//			if(hospital.IsChosen) {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOnBackground);
//			} else {
//				img.SetImageResource(Android.Resource.Drawable.CheckboxOffBackground);
//			}
		}	

		public static string DatabaseFileDir(){
			return System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,"MyTempDir");
		}	
	}
}

