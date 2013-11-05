using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using PresenterPlanner.Lib.Doctors;

namespace PresenterPlanner.Adapters
{
	public class WorkTimeListAdapter: ArrayAdapter <Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>> 
	{
		Activity context;
		Doctor doctor;
		EventHandler<TimePickerDialog.TimeSetEventArgs> tpCallBack;

		public WorkTimeListAdapter(Activity context, IList<Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>> objects, Doctor doc, EventHandler<TimePickerDialog.TimeSetEventArgs> tpCB)
			: base(context, Android.Resource.Id.Text1, objects)
		{
			this.context = context;
			doctor = doc;
			tpCallBack = tpCB; 
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = context.LayoutInflater.Inflate(Resource.Layout.WorkTimeListItem, null);
			var item = GetItem(position);

			view.FindViewById<TextView> (Resource.Id.txtDayOrOddEven).Text = item.Item1;

			var btnWorkTimeFromValue = view.FindViewById<Button> (Resource.Id.btnWorkTimeFromValue);
			btnWorkTimeFromValue.Text = item.Item2.ToString("t");
			btnWorkTimeFromValue.Click += (object sender, EventArgs e) => {
				doctor.chooseNwtType = item.Item3;
				var timepickDialog = new TimePickerDialog(context, tpCallBack, item.Item2.Hour, item.Item2.Minute, true);
				timepickDialog.SetTitle(item.Item1);
				timepickDialog.Show();
			};

			var btnWorkTimeTillValue = view.FindViewById<Button> (Resource.Id.btnWorkTimeTillValue);
			btnWorkTimeTillValue.Text = item.Item4.ToString("t");
			btnWorkTimeTillValue.Click += (object sender, EventArgs e) => {
				doctor.chooseNwtType = item.Item5;
				var timepickDialog = new TimePickerDialog(context, tpCallBack, item.Item4.Hour, item.Item4.Minute, true);
				timepickDialog.SetTitle(item.Item1);
				timepickDialog.Show();
			};

			return view;
		}
	}
}

