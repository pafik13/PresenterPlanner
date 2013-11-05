using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PresenterPlanner.Lib.Hospitals;

namespace PresenterPlanner
{
	[Activity (Label = "Больница или отделение")]			
	public class HospitalDetails : Activity
	{
		protected Hospital hospital = new Hospital();
		protected EditText hospitalName = null;
		protected EditText hospitalAdress = null;
		protected Button cancelDeleteButton = null;
		protected Button saveButton = null;
		ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here			
			int hospitalID = Intent.GetIntExtra("HospitalID", 0);
			if(hospitalID > 0) {
				hospital = HospitalManager.GetHospital(hospitalID);
			}

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HospitalDetails);
			hospitalName = FindViewById<EditText>(Resource.Id.txtHospitalName);
			hospitalAdress = FindViewById<EditText>(Resource.Id.txtHospitalAdress);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);

			// find all our controls
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);


			// set the cancel delete based on whether or not it's an existing task
			if(cancelDeleteButton != null)
			{ cancelDeleteButton.Text = (hospital.ID == 0 ? "Отмена" : "Удалить"); }

			// name
			if(hospitalName != null) { hospitalName.Text = hospital.Name; }

			// notes
			if(hospitalAdress != null) { hospitalAdress.Text = hospital.Adress; }

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		protected void Save()
		{
			progress = ProgressDialog.Show(this, "Обработка...", "Пожалуйста, подождите.", true);
			hospital.Name = hospitalName.Text;
			hospital.Adress = hospitalAdress.Text;
			hospital.IsChosen = false;
			HospitalManager.SaveHospital(hospital);
			progress.Dismiss();
			Finish();
		}

		protected void CancelDelete()
		{
			progress = ProgressDialog.Show(this, "Обработка...", "Пожалуйста, подождите.", true);
			if (hospital.ID != 0) {
				HospitalManager.DeleteHospital(hospital.ID);
			}
			progress.Dismiss();
			Finish();
		}
	}
}

