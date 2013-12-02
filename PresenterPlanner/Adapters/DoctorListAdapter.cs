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
using PresenterPlanner.Lib;

namespace PresenterPlanner.Adapters
{
	public class DoctorListAdapter : BaseAdapter<Doctor> {
		protected Activity context = null;
		protected IList<Doctor> doctors = new List<Doctor>();

		public DoctorListAdapter (Activity context, IList<Doctor> doctors) : base ()
		{
			this.context = context;
			this.doctors = doctors;
		}

		public override Doctor this[int position]
		{
			get { return doctors[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return doctors.Count; }
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = doctors[position];
			var hosp = HospitalManager.GetHospital (item.HospitalID);			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
			            context.LayoutInflater.Inflate(
				Resource.Layout.DoctorsListItem, 
				parent, 
				false)) as LinearLayout;
			// Find references to each subview in the list item's view
			var txtDoctorFullName = view.FindViewById<TextView> (Resource.Id.txtDoctorFullName);
			var txtDocSpeciality = view.FindViewById<TextView> (Resource.Id.txtDocSpeciality);
			var txtDocHospital = view.FindViewById<TextView> (Resource.Id.txtDocHospital);

			Common.SetCheck (view, item);

			txtDoctorFullName.Text = item.SecondName + ' ' + item.FirstName + ' ' + item.ThirdName;
			txtDocSpeciality.Text = item.Speciality;
			txtDocHospital.Text = hosp.Name;
			//Finally return the view
			return view;
		}

	}
}

