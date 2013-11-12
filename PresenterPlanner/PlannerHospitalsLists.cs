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
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Adapters;

namespace PresenterPlanner
{
	[Activity (Label = "PlannerHospitalsLists")]			
	public class PlannerHospitalsLists : Activity
	{
		protected ListView lstChoosenHosp = null;
		protected HospitalListAdapter choosenHospList;
		protected IList<Hospital> choosenHospitals;
		
		protected ListView lstAvailableHosp = null;
		protected HospitalListAdapter availableHospList;
		protected IList<Hospital> availableHospitals;

		protected int weekNum;
		protected int dayOfWeek;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView(Resource.Layout.PlannerHospitalsLists);

			weekNum   = Intent.GetIntExtra("WeekNum", 0);
			dayOfWeek = Intent.GetIntExtra("DayOfWeek", 0);

			Toast.MakeText (this, String.Format ("WeekNum = {0}", weekNum), ToastLength.Short).Show ();
			Toast.MakeText (this, String.Format ("DayOfWeek = {0}", dayOfWeek), ToastLength.Short).Show ();

			lstChoosenHosp = FindViewById<ListView> (Resource.Id.lstChoosenHosp);
			if(lstChoosenHosp != null) {
				lstChoosenHosp.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					for (int p = 0; p < choosenHospitals[e.Position].planners.Count; p++) {
						if (choosenHospitals[e.Position].planners [p].weekNum   == weekNum &&
						    choosenHospitals[e.Position].planners [p].dayOfWeek == (DayOfWeek)dayOfWeek) {
							choosenHospitals[e.Position].planners.RemoveAt(p);
							HospitalManager.SaveHospital(choosenHospitals[e.Position]);
						}
					}
					RefreshChoosenHospList ();
					RefreshAvailableHospList ();
				};
			}

			lstAvailableHosp = FindViewById<ListView> (Resource.Id.lstAvailableHosp);
			if(lstAvailableHosp != null) {
				lstAvailableHosp.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
//					int p = availableHospitals[e.Position].planners.Count;
					PlannerItem pItem = new PlannerItem {weekNum = weekNum, dayOfWeek = (DayOfWeek)dayOfWeek};
					availableHospitals[e.Position].planners.Add(pItem);
					HospitalManager.SaveHospital(availableHospitals[e.Position]);
					RefreshChoosenHospList ();
					RefreshAvailableHospList ();
				};
			}
		}

		public void RefreshAvailableHospList()
		{

			availableHospitals = HospitalManager.GetAvailableHospitals(weekNum, (DayOfWeek)dayOfWeek);

			// create our adapter
			availableHospList = new HospitalListAdapter(this, availableHospitals);

			//Hook up our adapter to our ListView
			lstAvailableHosp.Adapter = availableHospList;
		}

		public void RefreshChoosenHospList()
		{

			choosenHospitals = HospitalManager.GetChoosenHospitals(weekNum, (DayOfWeek)dayOfWeek);

			// create our adapter
			choosenHospList = new HospitalListAdapter(this, choosenHospitals);

			//Hook up our adapter to our ListView
			lstChoosenHosp.Adapter = choosenHospList;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			RefreshAvailableHospList ();
			RefreshChoosenHospList ();
		}
	}
}

