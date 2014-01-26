using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Runtime;

using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Lib.Report;

namespace PresenterPlanner
{
	[Activity (Label = "Отчет о визитах")]			
	public class ReportList : Activity
	{
		protected TableLayout tlHeader = null;
		protected TableLayout tlContent = null;
		protected List<int> listOfWeekNum = null;

		protected LayoutInflater inflater = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.ReportList);

			inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);

			tlHeader = FindViewById <TableLayout> (Resource.Id.tlHeader);
			MakeTableHeader ();

			tlContent = FindViewById <TableLayout> (Resource.Id.tlContent);
			//var dummy = new TableRow (this);

			int maxFIOLength = 0;
			int docID = 0;

			var hospitals = HospitalManager.GetHospitals ().OrderBy(h=>h.Name).ToList();

			foreach (Hospital hosp in hospitals) {
				var doctors = DoctorManager.GetDoctors (hosp.ID).OrderBy(d=>d.SecondName).ToList();

				if (doctors.Count > 0) {
					TableRow hview = (TableRow)inflater.Inflate (Resource.Layout.TableRow, null);
					hview.FindViewById <TextView> (Resource.Id.txtFIO).Text = hosp.Name;
					hview.FindViewById <TextView> (Resource.Id.txtFIO).SetBackgroundResource (Resource.Drawable.border_blue);
					hview.FindViewById <TextView> (Resource.Id.txtFIO).SetTextColor (Android.Graphics.Color.Black);
					hview.FindViewById <TextView> (Resource.Id.txtFIO).SetShadowLayer (0, 0, 0, Android.Graphics.Color.Black);

					foreach (int weekNum in listOfWeekNum) {
						TextView hviewVisitCount = (TextView)inflater.Inflate (Resource.Layout.TableVisitCount, null);
						hviewVisitCount.Text = "";
						hviewVisitCount.SetBackgroundResource (Resource.Drawable.border_blue);
						hviewVisitCount.SetTextColor (Android.Graphics.Color.Black);
						hview.AddView (hviewVisitCount);
					}
					tlContent.AddView (hview);
				}

				foreach (Doctor doc in doctors) {
					TableRow view = (TableRow)inflater.Inflate (Resource.Layout.TableRow, null);

					view.FindViewById <TextView> (Resource.Id.txtFIO).Text = doc.FIO () + "\n" + doc.Speciality;

					if (maxFIOLength < view.FindViewById <TextView> (Resource.Id.txtFIO).Text.Length) {
						maxFIOLength = view.FindViewById <TextView> (Resource.Id.txtFIO).Text.Length;
						docID = doc.ID;
					}

					var report = ReportManager.GetReport (doc.ID);

					foreach (int weekNum in listOfWeekNum) {
						TextView viewVisitCount = (TextView)inflater.Inflate (Resource.Layout.TableVisitCount, null);
						if (report == null) {
							viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = "0";
						} else {
							int visitCount = report.FindVisitCountValue (weekNum);
							viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = visitCount.ToString ();
							switch (visitCount) {
							case 0:
								break;
							case 1: 
								viewVisitCount.SetBackgroundResource (Resource.Drawable.border_green);
								viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
								break;
							default: 
								viewVisitCount.SetBackgroundResource (Resource.Drawable.border_red);
								viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
								break;
							}
						}
						viewVisitCount.LayoutParameters = new TableRow.LayoutParams (TableRow.LayoutParams.WrapContent, TableRow.LayoutParams.MatchParent);
						view.AddView (viewVisitCount);
					}
					tlContent.AddView (view);
				}
			}

			var doctorsWithoutHospitals = DoctorManager.GetDoctors (-1).OrderBy(d=>d.SecondName).ToList();
			foreach (Doctor doc in doctorsWithoutHospitals) {
				TableRow view = (TableRow)inflater.Inflate (Resource.Layout.TableRow, null);

				view.FindViewById <TextView> (Resource.Id.txtFIO).Text = doc.FIO ();

				if (maxFIOLength < view.FindViewById <TextView> (Resource.Id.txtFIO).Text.Length) {
					maxFIOLength = view.FindViewById <TextView> (Resource.Id.txtFIO).Text.Length;
					docID = doc.ID;
				}

				var report = ReportManager.GetReport (doc.ID);

				foreach (int weekNum in listOfWeekNum) {
					TextView viewVisitCount = (TextView)inflater.Inflate (Resource.Layout.TableVisitCount, null);
					if (report == null) {
						viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = "0";
					} else {
						int visitCount = report.FindVisitCountValue (weekNum);
						viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = visitCount.ToString ();
						switch (visitCount) {
						case 0:
							break;
						case 1: 
							viewVisitCount.SetBackgroundResource (Resource.Drawable.border_green);
							viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
							break;
						default: 
							viewVisitCount.SetBackgroundResource (Resource.Drawable.border_red);
							viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
							break;
						}
						viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
					}
					viewVisitCount.LayoutParameters = new TableRow.LayoutParams (TableRow.LayoutParams.WrapContent, TableRow.LayoutParams.MatchParent);
					view.AddView (viewVisitCount);
				}
				tlContent.AddView (view);
			}

			if (docID != 0) {
				TableRow dummyView = (TableRow)inflater.Inflate(Resource.Layout.TableRow, null);
				dummyView.FindViewById <TextView> (Resource.Id.txtFIO).Text = DoctorManager.GetDoctor (docID).FIO ();
				var report = ReportManager.GetReport (docID);
				foreach (int weekNum in listOfWeekNum) {
					TextView viewVisitCount = (TextView)inflater.Inflate (Resource.Layout.TableVisitCount, null);
					//viewVisitCount.LayoutParameters = new TableRow.LayoutParams(
					if (report == null) {
						viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = "0";
					} else {
						viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = weekNum.ToString();
					}
					viewVisitCount.LayoutParameters = new TableRow.LayoutParams (TableRow.LayoutParams.MatchParent, 0);
					dummyView.AddView (viewVisitCount);
				}
				dummyView.FindViewById <TextView> (Resource.Id.txtFIO).LayoutParameters.Height = 0;
				tlHeader.AddView (dummyView);
			}
		}


			/*  MyFuncs */
		void MakeTableHeader () {
			TableRow view = (TableRow)inflater.Inflate (Resource.Layout.TableRowHeader, null);
			view.FindViewById <TextView> (Resource.Id.txtFIO).Text =  "ФИО";

			listOfWeekNum = new List<int> ();
			var min = ReportManager.GetMinWeekNum ();
			for (int i = 0; i < 12; i++) {
				listOfWeekNum.Add ((min + i)%52);
			}

			foreach (int weekNum in listOfWeekNum) {
				TextView txtWeekNum = (TextView)inflater.Inflate (Resource.Layout.TableWeekNum, null);
				txtWeekNum.Text = weekNum.ToString ();
				view.AddView (txtWeekNum);
			}
			tlHeader.AddView (view);
		}


	}
}

