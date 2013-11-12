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
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Lib;

namespace PresenterPlanner
{
	[Activity (Label = "Список больниц и отделений")]		
	public class HospitalsList : Activity
	{
		protected HospitalListAdapter hospitalList;
		protected IList<Hospital> hospitals;
		protected ListView lstHospitals = null;
		protected Button btnAddHospital = null;
		protected Button btnChoice = null;

		// Menu item ids
		public const int MENU_ITEM_EDIT = Menu.First;
		public const int MENU_ITEM_DELETE = Menu.First + 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.HospitalsList);

			btnAddHospital = FindViewById<Button> (Resource.Id.btnAddHospital);
			
			// wire up add task button handler
			if(btnAddHospital != null) {
				btnAddHospital.Click += (sender, e) => {
					StartActivity(typeof(HospitalDetails));
				};
			}

			lstHospitals = FindViewById<ListView> (Resource.Id.lstHospitals);

			RegisterForContextMenu (lstHospitals);

			// wire up task click handler
			if(lstHospitals != null) {
				lstHospitals.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					//hospitals[e.Position].IsChosen = !hospitals[e.Position].IsChosen;
					//Common.SetCheck(e.View, hospitals[e.Position]);
					//HospitalManager.SaveHospital (hospitals[e.Position]);
					var hospitalDetails = new Intent (this, typeof (HospitalDetails));
					hospitalDetails.PutExtra ("HospitalID", hospitals[e.Position].ID);
					StartActivity (hospitalDetails);
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
					var hospitalDetails = new Intent (this, typeof (HospitalDetails));
					hospitalDetails.PutExtra ("HospitalID", hospitals[info.Position].ID);
					StartActivity (hospitalDetails);
					ContextItemClicked(item.TitleFormatted.ToString()); break;
				case MENU_ITEM_DELETE:
					HospitalManager.DeleteHospital (hospitals [info.Position].ID);
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

			hospitals = HospitalManager.GetHospitals();

			// create our adapter
			hospitalList = new HospitalListAdapter(this, hospitals);

			//Hook up our adapter to our ListView
			lstHospitals.Adapter = hospitalList;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			RefreshList ();
		}
	}
}

