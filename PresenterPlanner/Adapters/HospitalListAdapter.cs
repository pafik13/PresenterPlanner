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
	public class HospitalListAdapter: BaseAdapter<Hospital>
	{
		protected Activity context = null;
		protected IList<Hospital> hospitals = new List<Hospital>();

		public HospitalListAdapter (Activity context, IList<Hospital> hospitals) : base ()
		{
			this.context = context;
			this.hospitals = hospitals;
		}

		public override Hospital this[int position]
		{
			get { return hospitals[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return hospitals.Count; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = hospitals[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
			            context.LayoutInflater.Inflate(
				Resource.Layout.HospitalsListItem, 
				parent, 
				false)) as LinearLayout;
			// Find references to each subview in the list item's view
			var txtHospitalName = view.FindViewById<TextView> (Resource.Id.txtHospitalName);
			var txtHospitalAdress = view.FindViewById<TextView> (Resource.Id.txtHospitalAdress);

			Common.SetCheck (view, item);

			txtHospitalName.SetText (item.Name, CheckBox.BufferType.Normal);
			txtHospitalAdress.SetText (item.Adress, CheckBox.BufferType.Normal);

			//Finally return the view
			return view;
		}

	}
}

