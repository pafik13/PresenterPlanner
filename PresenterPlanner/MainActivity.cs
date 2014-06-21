using System;
using System.Net;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using PresenterPlanner.Lib;

namespace PresenterPlanner
{

	[Activity (Label = "Планировщик", MainLauncher = true, Icon = "@drawable/Icon_planner_72")]
	public class MainActivity : Activity
	{
		protected Button btnPresentations = null;
		protected Button btnPlanner = null;
		protected Button btnData = null;
		protected Button btnVisits = null;
		protected Button btnSync = null;
		protected Button btnReport = null;

		protected const int DLG_WORK_TIME_WARNING = 20001;

		protected override void OnCreate (Bundle bundle)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			btnPresentations = FindViewById <Button> (Resource.Id.btnPresentations);
			btnPresentations.Click += (object sender, EventArgs e) => 
			{
				btnPresentations.Text = "Загрузка презентаций...";
				var presentations = new Intent (this, typeof(PresentationsList));
				StartActivity (presentations);
			};

			btnPlanner = FindViewById <Button> (Resource.Id.btnPlanner);
			btnPlanner.Click += (object sender, EventArgs e) => 
			{
				btnPlanner.Text = "Загрузка планировщика...";
				var planner = new Intent (this, typeof(PlannerGrid));
				StartActivity (planner);
			};

			btnData = FindViewById <Button> (Resource.Id.btnData);
			btnData.Click += (object sender, EventArgs e) => 
			{
				btnData.Text = "Загрузка данных...";
				var data = new Intent (this, typeof(DoctorsAndHospitals));
				StartActivity (data);
			};

			btnVisits = FindViewById <Button> (Resource.Id.btnVisits);
			btnVisits.Click += (object sender, EventArgs e) => 
			{
				btnVisits.Text = "Загрузка визитов...";
				var visits = new Intent (this, typeof(VisitsList));
				StartActivity (visits);
			};

			btnSync = FindViewById <Button> (Resource.Id.btnSync);
			btnSync.Click += (object sender, EventArgs e) => 
			{
				btnSync.Text = "Загрузка информации...";
				var sync = new Intent (this, typeof(SyncView));
				StartActivity (sync);
			};

			btnReport = FindViewById <Button> (Resource.Id.btnReport);
			btnReport.Click += (object sender, EventArgs e) => 
			{
				btnReport.Text = "Загрузка отчета...";
				var report = new Intent (this, typeof(ReportList));
				StartActivity (report);
			};
		}

		protected override Dialog OnCreateDialog (int id)
		{
			//return base.OnCreateDialog (id);
			switch (id) {
			case DLG_WORK_TIME_WARNING:
				Dialog dialog = new Dialog (this);
				dialog.SetTitle ("Предупреждение");
				dialog.SetContentView (Resource.Layout.Dialog);
				dialog.FindViewById <Button> (Resource.Id.btnDlgClose).Click += (object sender, EventArgs e) => {
					this.Finish();
				};

				dialog.SetCancelable (false);
				return dialog;
			}
			return null;
		} 

		protected override void OnResume ()
		{
			btnVisits.Text = "Визиты";
			btnPlanner.Text = "План";
			btnData.Text = "Новые врачи/ЛПУ";
			btnPresentations.Text = "Слайды";
			btnSync.Text = "Синхронизация";
			btnReport.Text = "Отчет";

			base.OnResume ();

//			if ((DateTime.Now < Convert.ToDateTime("09:00:00"))
//				||
//				(DateTime.Now > Convert.ToDateTime("20:00:00")) ) {
//				ShowDialog (DLG_WORK_TIME_WARNING);
//			}
		}
	}
}


