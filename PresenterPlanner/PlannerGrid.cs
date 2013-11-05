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
using PresenterPlanner.PlannerManager;

namespace PresenterPlanner
{
	[Activity (Label = "PlannerGrid")]			
	public class PlannerGrid : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.PlannerGrid);

			var gridview = FindViewById<GridView>(Resource.Id.PlannerGrid);
			gridview.Adapter = new DateAdapter(this);

			gridview.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args) {
				Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
			};

		}

		//baseAdapter
		public class DateAdapter : BaseAdapter {
			Activity context;
			DateTime[] dt;

			public DateAdapter(Activity c)
			{
				context = c;
				dt = PlannerManager.PlannerManager.GetWeeks(3, DateTime.Today);
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
				(view.FindViewById<TextView>(Resource.Id.txtDate)).Text = "Date: " + dt [position].ToString("d") +"\n" + dt [position].DayOfWeek.ToString() + " = " +((int)dt [position].DayOfWeek).ToString();//.ToString (); // ("yy-MM-dd");
//				view.SetPadding (8, 20, 8, 20);
				return view;
			}
		}
	}
}

