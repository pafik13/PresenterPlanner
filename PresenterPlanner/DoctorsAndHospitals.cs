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

namespace PresenterPlanner
{
	[Activity (Label = "Врачи и больницы", Icon = "@drawable/Icon_data")]			
	public class DoctorsAndHospitals : TabActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.DoctorsAndHospitals);

			CreateTab(typeof(DoctorsList), "doctors", "", Resource.Drawable.ic_tab_doctors);
			CreateTab(typeof(HospitalsList), "hospitals", "", Resource.Drawable.ic_tab_hospitals);

		}

		private void CreateTab(Type activityType, string tag, string label, int drawableId )
		{
			var intent = new Intent(this, activityType);
			intent.AddFlags(ActivityFlags.NewTask);

			var spec = TabHost.NewTabSpec(tag);
			var drawableIcon = Resources.GetDrawable(drawableId);
			spec.SetIndicator(label, drawableIcon);
			spec.SetContent(intent);

			TabHost.AddTab(spec);
		}
	}
}

