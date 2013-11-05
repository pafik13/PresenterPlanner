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
using PresenterPlanner.Adapters;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib;

namespace PresenterPlanner
{
	[Activity (Label = "Список врачей")]		
	public class DoctorsList : Activity
	{
		protected Adapters.DoctorListAdapter doctorList;
		protected IList<Doctor> doctors;
		protected ListView lstDoctors = null;
		protected Button btnAdd = null;
		protected Button btnChoice = null;

		// Menu item ids
		public const int MENU_ITEM_EDIT = Menu.First;
		public const int MENU_ITEM_DELETE = Menu.First + 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.DoctorsList);

			btnAdd = FindViewById<Button> (Resource.Id.btnAdd);

			// wire up add task button handler
			if(btnAdd != null) {
				btnAdd.Click += (sender, e) => {
					StartActivity(typeof(DoctorDetailsActivity));
				};
			}

			
			btnChoice = FindViewById<Button> (Resource.Id.btnChoice);

			if (btnChoice != null) {
				btnChoice.Click += (sender, e) => {	
					Finish();
				};
			}

			btnChoice.Visibility = ViewStates.Gone;

			lstDoctors = FindViewById<ListView> (Resource.Id.lstDoctors);

			RegisterForContextMenu (lstDoctors);

			// wire up task click handler
			if(lstDoctors != null) {
				lstDoctors.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					doctors[e.Position].IsChosen = !doctors[e.Position].IsChosen;
					Common.SetCheck(e.View, doctors[e.Position]);
					DoctorManager.SaveDoctor (doctors[e.Position]);
				};
			}
		}

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo info)
		{
			base.OnCreateOptionsMenu (menu);
			menu.SetHeaderTitle("Выберите действие");
			menu.Add(0, MENU_ITEM_EDIT, 0, "Редактировать");
			menu.Add(0, MENU_ITEM_DELETE, 0, "Удалить");
		}

		public override bool OnContextItemSelected(IMenuItem item)
		{
			var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;

			switch (item.ItemId) { 
				case MENU_ITEM_EDIT:
					var doctorDetails = new Intent (this, typeof (DoctorDetailsActivity));
					doctorDetails.PutExtra ("DoctorID", doctors[info.Position].ID);
					StartActivity (doctorDetails);
					ContextItemClicked(item.TitleFormatted.ToString()); break;
			case MENU_ITEM_DELETE:
				DoctorManager.DeleteDoctor (doctors [info.Position].ID);
				ContextItemClicked (item.TitleFormatted.ToString ());
				RefreshList ();
				break;
			}
			return base.OnOptionsItemSelected(item);
		}

		void ContextItemClicked(string item)
		{
			Console.WriteLine(item + " option menuitem clicked");
			var t = Toast.MakeText(this, "Options Menu '"+item+"' clicked", ToastLength.Short);
			t.Show();
		}


		public void RefreshList()
		{

			doctors = DoctorManager.GetDoctors();

			// create our adapter
			doctorList = new DoctorListAdapter(this, doctors);

			//Hook up our adapter to our ListView
			lstDoctors.Adapter = doctorList;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			RefreshList ();
		}
	}
}

