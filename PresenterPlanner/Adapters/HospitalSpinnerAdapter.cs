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
using PresenterPlanner.Lib;

namespace PresenterPlanner.Adapters
{
	public class HospitalSpinnerAdapter: BaseAdapter<Hospital>
	{
		protected Activity context = null;
		protected IList<Hospital> hospitals = new List<Hospital>();

		public HospitalSpinnerAdapter (Activity context, IList<Hospital> hospitals) : base ()
		{
			this.context = context;
			this.hospitals = hospitals;
		}

		public override Hospital this[int position]
		{
			get { return hospitals[position - 1]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return hospitals.Count + 1; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			
			var view = context.LayoutInflater.Inflate(Resource.Layout.HospitalsSpinnerItem, parent,	false);

			var txtSprItemName = view.FindViewById<TextView> (Resource.Id.txtSpnrItemHName);
			var txtSpnrItemHAdress = view.FindViewById<TextView> (Resource.Id.txtSpnrItemHAdress);

			if (position == 0) {
				txtSprItemName.SetText ("<None>", CheckBox.BufferType.Normal);
				txtSpnrItemHAdress.SetText ("<None>", CheckBox.BufferType.Normal);
			} else {
				var item = hospitals[position - 1];
				txtSprItemName.SetText (item.Name, CheckBox.BufferType.Normal);
				txtSpnrItemHAdress.SetText (item.Adress, CheckBox.BufferType.Normal);
			}

			//Finally return the view
			return view;
		}

	}
}

