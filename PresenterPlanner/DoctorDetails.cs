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
using PresenterPlanner.Lib;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Adapters;

namespace PresenterPlanner
{
	[Activity (Label = "Врач", Icon = "@drawable/Icon_data")]			
	public class DoctorDetailsActivity : Activity
	{
		protected Doctor doctor = new Doctor();
		protected EditText SNameTextEdit = null;
		protected EditText FNameTextEdit = null;
		protected EditText TNameTextEdit = null;
		protected EditText TelTextEdit = null;
		protected EditText EmailTextEdit = null;
		protected AutoCompleteTextView SpecTextEdit = null;
		protected AutoCompleteTextView PosTextEdit = null;
		protected Button cancelDeleteButton = null;
		protected Button saveButton = null;
		ProgressDialog progress;
		protected HospitalSpinnerAdapter hospitalList;
		protected IList<Hospital> hospitals;
		protected Spinner spnHospital = null;
		protected CheckBox chIsDays = null;
		protected ListView lstWorkTime = null;
		List<Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>> workTimeItems;
		protected WorkTime_Type chosenWorkTimeType;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here			
			int doctorID = Intent.GetIntExtra("DoctorID", 0);
//			if(doctorID > 0) {
				doctor = DoctorManager.GetDoctor(doctorID);
//			}

			// set our layout to be the home screen
			SetContentView(Resource.Layout.DoctorDetails);

			SNameTextEdit = FindViewById<EditText> (Resource.Id.txtSecondName);
			FNameTextEdit = FindViewById<EditText> (Resource.Id.txtFirstName);
			TNameTextEdit = FindViewById<EditText> (Resource.Id.txtThirdName);
			TelTextEdit   = FindViewById<EditText> (Resource.Id.txtTel);
			EmailTextEdit = FindViewById<EditText> (Resource.Id.txtEmail);
			SpecTextEdit  = FindViewById<AutoCompleteTextView> (Resource.Id.actxtSpeciality);
			ArrayAdapter SpecTextEditAdapter = new ArrayAdapter (this, Android.Resource.Layout.SimpleDropDownItem1Line, DoctorSpecialitys.GetSpecialitys());
			SpecTextEdit.Adapter = SpecTextEditAdapter;

			PosTextEdit   = FindViewById<AutoCompleteTextView> (Resource.Id.txtPosition);
			ArrayAdapter PosTextEditAdapter = new ArrayAdapter (this, Android.Resource.Layout.SimpleDropDownItem1Line,DoctorPositions.GetPositions() );
			PosTextEdit.Adapter = PosTextEditAdapter;

			saveButton = FindViewById<Button> (Resource.Id.btnSave);
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);
			chIsDays = FindViewById<CheckBox> (Resource.Id.chIsDays);
			lstWorkTime = FindViewById<ListView> (Resource.Id.lstWorkTime);


			// set the cancel delete based on whether or not it's an existing task
			if(cancelDeleteButton != null)
			{ cancelDeleteButton.Text = (doctor.ID == 0 ? "Отмена" : "Удалить"); }

			// SecondName
			if(SNameTextEdit != null) { SNameTextEdit.Text = doctor.SecondName; }

			// FirstName
			if(FNameTextEdit != null) { FNameTextEdit.Text = doctor.FirstName; }

			// ThirdName
			if(TNameTextEdit != null) { TNameTextEdit.Text = doctor.ThirdName; }

			// Telephone
			if(TelTextEdit != null) { TelTextEdit.Text = doctor.Tel; }

			// E-mail
			if(EmailTextEdit != null) { EmailTextEdit.Text = doctor.Email; }

			// Speciality
			if(SpecTextEdit != null) { SpecTextEdit.Text = doctor.Speciality; }

			// Position
			if(PosTextEdit != null) { PosTextEdit.Text = doctor.Position; }

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };

			workTimeItems = new List<Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>>();

			if (doctorID == 0) {
				chIsDays.Checked = true;
				doctor.wtKind = WorkTime_Kind.Days;
			} else {
				switch (doctor.wtKind) {
				case WorkTime_Kind.OddEven: { chIsDays.Checked = false; break; }
				case WorkTime_Kind.Days: { chIsDays.Checked = true; break; }
				}
			}

			UpdateWorkTimeList();

			chIsDays.CheckedChange += (sender, e) => { 
				if (chIsDays.Checked) { doctor.wtKind = WorkTime_Kind.Days; }
				else { doctor.wtKind = WorkTime_Kind.OddEven; }
				UpdateWorkTimeList();
			};

			/////////////////new code///////////////
			spnHospital = FindViewById<Spinner> (Resource.Id.spnHospital);
			hospitals = HospitalManager.GetHospitals();

			hospitalList = new Adapters.HospitalSpinnerAdapter(this, hospitals);
			spnHospital.Adapter = hospitalList;

			for (int i = 0; i < hospitals.Count; i++) {
				if (hospitals [i].ID == doctor.HospitalID) {
					spnHospital.SetSelection (i + 1);
				}
			}

			spnHospital.ItemSelected += (sender, e) => {
				var s = sender as Spinner;
				if (e.Position == 0) {
					doctor.HospitalID = -1;
				}
				else {
					doctor.HospitalID = hospitals[e.Position - 1].ID;
				}
			};
			/////////////////new code///////////////
		}

		protected void Save()
		{
			progress = ProgressDialog.Show(this, "Обработка...", "Пожалуйста, подождите.", true);
			doctor.SecondName = SNameTextEdit.Text;
			doctor.FirstName = FNameTextEdit.Text;
			doctor.ThirdName = TNameTextEdit.Text;
			doctor.IsChosen = false;
			doctor.Tel = TelTextEdit.Text;
			doctor.Email = EmailTextEdit.Text;
			doctor.Speciality = SpecTextEdit.Text;
			doctor.Position = PosTextEdit.Text;
			DoctorManager.SaveDoctor(doctor);
			DoctorPositions.SavePosition (doctor.Speciality);
			DoctorPositions.SavePosition (doctor.Position);
			progress.Dismiss();
			Finish();
		}

		protected void CancelDelete()
		{
			progress = ProgressDialog.Show(this, "Обработка...", "Пожалуйста, подождите.", true);
			if (doctor.ID != 0) {
				DoctorManager.DeleteDoctor(doctor.ID);
			}
			progress.Dismiss();
			Finish();
		}

		private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			var wtD = new WorkTime_Days();
			wtD = doctor.wtDays;
			var wtOE = new WorkTime_OddEven();
			wtOE = doctor.wtOddEven;
			switch(doctor.chooseNwtType)
			{
			case WorkTime_Type.Mon_From: { wtD.Mon_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Mon_Till: { wtD.Mon_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Tue_From: { wtD.Tue_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Tue_Till: { wtD.Tue_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Wed_From: { wtD.Wed_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Wed_Till: { wtD.Wed_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Fri_From: { wtD.Fri_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Fri_Till: { wtD.Fri_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Sut_From: { wtD.Sut_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Sut_Till: { wtD.Sut_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Sun_From: { wtD.Sun_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Sun_Till: { wtD.Sun_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Odd_From: { wtOE.Odd_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Odd_Till: { wtOE.Odd_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }

			case WorkTime_Type.Even_From: { wtOE.Even_From = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			case WorkTime_Type.Even_Till: { wtOE.Even_Till = new DateTime(1,1,1, e.HourOfDay, e.Minute, 0); break; }
			}
			doctor.wtOddEven = wtOE;
			doctor.wtDays = wtD; 
			UpdateWorkTimeList();
		}
		
		private void UpdateWorkTimeList()
		{
			LinearLayout mainll = FindViewById<LinearLayout> (Resource.Id.lstDD);
			mainll.RemoveAllViewsInLayout ();
			//lstWorkTime.RemoveAllViewsInLayout();

			workTimeItems.Clear();
			if (doctor.wtKind == WorkTime_Kind.Days) 
			{ 	
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Понедельник", doctor.wtDays.Mon_From, WorkTime_Type.Mon_From, doctor.wtDays.Mon_Till, WorkTime_Type.Mon_Till));
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Вторник    ", doctor.wtDays.Tue_From, WorkTime_Type.Tue_From, doctor.wtDays.Tue_Till, WorkTime_Type.Tue_Till));
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Среда      ", doctor.wtDays.Wed_From, WorkTime_Type.Wed_From, doctor.wtDays.Wed_Till, WorkTime_Type.Wed_Till));
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Четверг    ", doctor.wtDays.Thu_From, WorkTime_Type.Thu_From, doctor.wtDays.Thu_Till, WorkTime_Type.Thu_Till));
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Пятница    ", doctor.wtDays.Fri_From, WorkTime_Type.Fri_From, doctor.wtDays.Fri_Till, WorkTime_Type.Fri_Till));
//				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Суббота    ", doctor.wtDays.Sut_From, WorkTime_Type.Sut_From, doctor.wtDays.Sut_Till, WorkTime_Type.Sut_Till));
//				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Воскресенье", doctor.wtDays.Sun_From, WorkTime_Type.Sun_From, doctor.wtDays.Sun_Till, WorkTime_Type.Sun_Till));
			}
			else
			{
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Четная дата  ", doctor.wtOddEven.Even_From, WorkTime_Type.Even_From, doctor.wtOddEven.Even_Till, WorkTime_Type.Even_Till));
				workTimeItems.Add(new Tuple<String, DateTime, WorkTime_Type, DateTime, WorkTime_Type>("Нечетная дата", doctor.wtOddEven.Odd_From, WorkTime_Type.Odd_From, doctor.wtOddEven.Odd_Till, WorkTime_Type.Odd_Till));
			}

			//lstWorkTime.Adapter = new WorkTimeListAdapter(this, workTimeItems, doctor, TimePickerCallback);

			for (int i=0; i<=workTimeItems.Count-1; i++) {
				var view = this.LayoutInflater.Inflate(Resource.Layout.WorkTimeListItem, null);
				var item = workTimeItems[i];

				view.FindViewById<TextView> (Resource.Id.txtDayOrOddEven).Text = item.Item1;

				var btnWorkTimeFromValue = view.FindViewById<Button> (Resource.Id.btnWorkTimeFromValue);
				btnWorkTimeFromValue.Text = item.Item2.ToString("t");
				btnWorkTimeFromValue.Click += (object sender, EventArgs e) => {
					doctor.chooseNwtType = item.Item3;
					var timepickDialog = new TimePickerDialog(this, TimePickerCallback, item.Item2.Hour, item.Item2.Minute, true);
					timepickDialog.SetTitle(item.Item1);
					timepickDialog.Show();
				};

				var btnWorkTimeTillValue = view.FindViewById<Button> (Resource.Id.btnWorkTimeTillValue);
				btnWorkTimeTillValue.Text = item.Item4.ToString("t");
				btnWorkTimeTillValue.Click += (object sender, EventArgs e) => {
					doctor.chooseNwtType = item.Item5;
					var timepickDialog = new TimePickerDialog (this, TimePickerCallback, item.Item4.Hour, item.Item4.Minute, true);
					timepickDialog.SetTitle (item.Item1);
					timepickDialog.Show ();
				};

				mainll.AddView (view);
			}
		}

	}
}

