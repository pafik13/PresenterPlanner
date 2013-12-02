using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PresenterPlanner.PlannerManager;
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Lib;
using Android.Telephony;

namespace PresenterPlanner
{

	[Activity (Label = "Планировщик показов", Icon = "@drawable/Icon_planner")]			
	public class PlannerGrid : Activity
	{
		protected GridView plannerGrid;
		protected Button btnPrevWeek;
		protected Button btnNextWeek;
		protected DateTime dayInWeek;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			Setts sett = Common.GetSettings ();

			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			int weekDiv = sett.weekOfStart - cal.GetWeekOfYear(DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

			dayInWeek = DateTime.Today.AddDays (weekDiv%3 * 7);

			SetContentView(Resource.Layout.PlannerGrid);

			plannerGrid = FindViewById<GridView>(Resource.Id.PlannerGrid);

			plannerGrid.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				//Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
				var plannerHospList = new Intent (this, typeof (PlannerHospitalsLists));
//				int weekNum = e.Position/7;
				plannerHospList.PutExtra ("WeekNum", e.Position/5);
				if ((e.Position + 1)%5 == 0) {
					plannerHospList.PutExtra ("DayOfWeek", 5);
				} else {
					plannerHospList.PutExtra ("DayOfWeek", (e.Position + 1)%5);
				}

				StartActivity (plannerHospList);
			};

			btnPrevWeek = FindViewById<Button>(Resource.Id.btnPrevWeek);
			btnPrevWeek.Click += (sender, e) => {
				dayInWeek = dayInWeek.AddDays(-7);
				RefreshGrid ();
				//Setts sett = Common.GetSettings ();
				if (!(sett.phone == "")) {
					SmsManager.Default.SendTextMessage (sett.phone, null, "Изменен календарь: Пердыдущая неделя.", null, null);
				}
			};

			btnNextWeek = FindViewById<Button>(Resource.Id.btnNextWeek);
			btnNextWeek.Click += (sender, e) => {
				dayInWeek = dayInWeek.AddDays(7);
				RefreshGrid ();
				//Setts sett = Common.GetSettings ();
				if (!(sett.phone == "")) {
					SmsManager.Default.SendTextMessage (sett.phone, null, "Изменен календарь: Слелующая неделя. ", null, null);
				}
			};
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			RefreshGrid ();
		}

		protected void RefreshGrid () {
			plannerGrid.Adapter = new DateAdapter(this, dayInWeek);
		}

		//baseAdapter
		public class DateAdapter : BaseAdapter {
			Activity context;
			DateTime[] dt;
			Setts setts;

			public DateAdapter(Activity c, DateTime dayInWeek)
			{
				context = c;
				dt = PlannerManager.PlannerManager.GetWeeks(3, dayInWeek);
				setts = Common.GetSettings ();
			}

			public override int Count
			{
				get { return dt.Length; }
			}

			public override Java.Lang.Object GetItem(int position)
			{
				return null;
			}

			public override long GetItemId(int position)
			{
				return 0;
			}

			// create a new ImageView for each item referenced by the Adapter  
			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				var view = (convertView ?? 
				            context.LayoutInflater.Inflate(
					Resource.Layout.PlannerGridItem, 
					parent, 
					false)) as LinearLayout;
				DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
				Calendar cal = dfi.Calendar;
				int week = cal.GetWeekOfYear(dt [position], dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

				if (position == 0) {
					setts.weekOfStart = week;
					Common.SetSettings (setts);
				}

				(view.FindViewById<TextView> (Resource.Id.txtDate)).Text = dt [position].ToString ("dd MMMMM") + "\n" + 
				                                                           DateTimeFormatInfo.CurrentInfo.DayNames [(int)dt [position].DayOfWeek];
//					"Date: " + dt [position].ToString("d") +"\n" + 
//					dt [position].DayOfWeek.ToString() + " = " +((int)dt [position].DayOfWeek).ToString() + "\n" +
//						                                                   "Week = " + week.ToString();//.ToString (); // ("yy-MM-dd");
//				view.SetPadding (8, 20, 8, 20);
				var txtHosps = view.FindViewById<TextView> (Resource.Id.txtHosps);
				txtHosps.Text = "";
				var chHosps = HospitalManager.GetChoosenHospitals(position/5, dt [position].DayOfWeek);
				for (int h = 0; h < chHosps.Count; h++) {
					txtHosps.Text = txtHosps.Text + chHosps [h].Name + "\n";
				}
				return view;
			}
		}
	}
}

