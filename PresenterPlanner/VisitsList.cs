using System;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
//using Android.Util;

using PresenterPlanner.Lib;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

using PresenterPlanner.Adapters;

namespace PresenterPlanner
{
	[Activity (Label = "План на сегодня ", Icon = "@drawable/Icon_presents_72")]
	public class VisitsList : Activity
	{
		protected VisitListAdpter adapter = null;
		protected List <Presentation> presents;
		protected Doctor doctor = null;

		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.VisitsList);

			Title = Title + "(" + DateTime.Today.ToString ("D") + ")";
			Setts sett = Common.GetSettings ();

			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			int week = (cal.GetWeekOfYear (DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - sett.weekOfStart) % 3;

			var choosenHospitals = (List<Hospital>)HospitalManager.GetChoosenHospitals (week, DateTime.Today.DayOfWeek);

			FindViewById <TextView> (Resource.Id.edtAnalyze).SetSingleLine (false);
			FindViewById <TextView> (Resource.Id.edtCommentForPharmacy).SetSingleLine (false);
			FindViewById <TextView> (Resource.Id.edtPOSmaterials).SetSingleLine (false);

			FindViewById <Button> (Resource.Id.btnShow).Clickable = false;
			adapter = new VisitListAdpter (this, choosenHospitals);
			var lvList = FindViewById <ListView> (Resource.Id.lvList);
			lvList.Adapter = adapter; //ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItemChecked, docs);
			lvList.ChoiceMode = ChoiceMode.Single;
			lvList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				doctor = adapter [e.Position];

				//presentations
				if (presents == null) {
					var lblPresents = FindViewById<TextView> (Resource.Id.PresentLabel);
					lblPresents.Visibility = ViewStates.Visible;
					var btnShow = FindViewById <Button> (Resource.Id.btnShow); 
					btnShow.Visibility = ViewStates.Visible;
					var spn = FindViewById <Spinner> (Resource.Id.spnPresents);
					spn.Visibility = ViewStates.Visible;

					string[] load = {"Загружается список презентаций..."};
					spn.Adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, load);
					new Thread(new ThreadStart(delegate {
							//LOAD METHOD TO GET ACCOUNT INFO
							RunOnUiThread(() => {

								presents = Presentations.GetPresentations ();
								List<String> presentsTitle = new List<String> ();
								for (int i = 0; i < presents.Count; i++) {
									for (int j = 0; j < presents [i].parts.Count; j++) {
										presentsTitle.Add (presents [i].name + "." + presents [i].parts [j].name);
									}
								}

								spn.Adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, presentsTitle.ToArray ());
						});
					})).Start ();
				}

				FindViewById <TextView> (Resource.Id.SecondNameLabel).Text = "Фамилия: " + doctor.SecondName;
				FindViewById <TextView> (Resource.Id.FirstNameLabel).Text = "Имя: " + doctor.FirstName;
				FindViewById <TextView> (Resource.Id.ThirdNameLabel).Text = "Отчество: " + doctor.ThirdName;
				FindViewById <TextView> (Resource.Id.TelLabel).Text = "Телефон: " + doctor.Tel;
				FindViewById <TextView> (Resource.Id.EmailLabel).Text = "E-mail: " + doctor.Email;
				FindViewById <TextView> (Resource.Id.SpecialityLabel).Text = "Специальность: " + doctor.Speciality;
				FindViewById <TextView> (Resource.Id.PositionLabel).Text = "Должность: " + doctor.Position;
				FindViewById <TextView> (Resource.Id.HospitalLabel).Text = "Поликлиника: " + HospitalManager.GetHospital (doctor.HospitalID).Name;
				FindViewById <TextView> (Resource.Id.CabinetLabel).Text = "№ кабинета: " + doctor.Cabinet;
				FindViewById <TextView> (Resource.Id.edtAnalyze).Text = doctor.LastVisitAnalyze;
				FindViewById <TextView> (Resource.Id.edtCommentForPharmacy).Text = doctor.LastCommForPharm;
				FindViewById <TextView> (Resource.Id.edtPOSmaterials).Text = doctor.LastPOSmaterials;
			};
				
			FindViewById <Button> (Resource.Id.btnShow).Click += (object sender, EventArgs e) => {

				doctor.LastVisitAnalyze = FindViewById <TextView> (Resource.Id.edtAnalyze).Text;
				doctor.LastCommForPharm = FindViewById <TextView> (Resource.Id.edtCommentForPharmacy).Text;
				doctor.LastPOSmaterials = FindViewById <TextView> (Resource.Id.edtPOSmaterials).Text;
				DoctorManager.SaveDoctor(doctor);

				var slides = new Intent (this, typeof(VisitPresentationView));
				int presentationID = 0;
				int partID = FindViewById <Spinner> (Resource.Id.spnPresents).SelectedItemPosition;
				for (int i = 0; (i <= presents.Count - 1) && (partID > presents [i].parts.Count - 1); i++) {
					presentationID = i + 1;
					partID = partID - presents [i].parts.Count;
				}
				slides.PutExtra ("presentationID", presentationID);
				slides.PutExtra ("partID", partID);
				slides.PutExtra ("doctorID", adapter [lvList.CheckedItemPosition].ID);
				StartActivity (slides);
			};
		}

		protected override void OnPause()
		{
			base.OnPause(); // Always call the superclass first
		}

		protected override void OnResume ()
		{
			base.OnResume ();
		}
	}
}


