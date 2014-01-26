using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;
using PresenterPlanner.Lib.Report;

namespace PresenterPlanner
{
	[Activity (Label = "Отчет о посещениях")]			
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

			var doctors = DoctorManager.GetDoctors ();

			tlHeader = FindViewById <TableLayout> (Resource.Id.tlHeader);
			MakeTableHeader ();

			tlContent = FindViewById <TableLayout> (Resource.Id.tlContent);
			//var dummy = new TableRow (this);

			int maxFIOLength = 0;
			int docID = 0;

			var hospitals = HospitalManager.GetHospitals ().OrderBy(h=>h.Name).ToList();


			foreach (Doctor doc in doctors) {
				TableRow view = (TableRow)inflater.Inflate(Resource.Layout.TableRow, null);

				view.FindViewById <TextView> (Resource.Id.txtFIO).Text =  doc.FIO ();

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
						viewVisitCount.FindViewById <TextView> (Resource.Id.txtVisitCount).Text = visitCount.ToString();
						if (visitCount > 1) {
							viewVisitCount.SetBackgroundResource(Resource.Drawable.border_red);
							viewVisitCount.SetTextColor (Android.Graphics.Color.Black);
						}
					}
					view.AddView (viewVisitCount);
				}
				tlContent.AddView(view);
//				if (report == null) {
//					view.FindViewById <TextView> (Resource.Id.w1).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w2).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w3).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w4).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w5).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w6).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w7).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w8).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w9).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w10).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w11).Text = "0";
//					view.FindViewById <TextView> (Resource.Id.w12).Text = "0";
//				} else {
//					view.FindViewById <TextView> (Resource.Id.w1).Text = report.FindVisitCountValue (listOfWeekNum[0]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w2).Text = report.FindVisitCountValue (listOfWeekNum[1]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w3).Text = report.FindVisitCountValue (listOfWeekNum[2]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w4).Text = report.FindVisitCountValue (listOfWeekNum[3]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w5).Text = report.FindVisitCountValue (listOfWeekNum[4]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w6).Text = report.FindVisitCountValue (listOfWeekNum[5]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w7).Text = report.FindVisitCountValue (listOfWeekNum[6]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w8).Text = report.FindVisitCountValue (listOfWeekNum[7]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w9).Text = report.FindVisitCountValue (listOfWeekNum[8]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w10).Text = report.FindVisitCountValue (listOfWeekNum[9]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w11).Text = report.FindVisitCountValue (listOfWeekNum[10]).ToString();
//					view.FindViewById <TextView> (Resource.Id.w12).Text = report.FindVisitCountValue (listOfWeekNum[11]).ToString();
//				}

			}

			//var dummyHeader = inflater.Inflate (Resource.Layout.TableRowHeader, null);
//			dummyHeader.FindViewById <TextView> (Resource.Id.txtFIO).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h1).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h2).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h3).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h4).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h5).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h6).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h7).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h8).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h9).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h10).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h11).LayoutParameters.Height = 0;
//			dummyHeader.FindViewById <TextView> (Resource.Id.h12).LayoutParameters.Height = 0;
			//tlContent.AddView (dummyHeader);

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
//				if (report == null) {
//					dummyView.FindViewById <TextView> (Resource.Id.w1).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w2).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w3).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w4).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w5).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w6).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w7).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w8).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w9).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w10).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w11).Text = "5";
//					dummyView.FindViewById <TextView> (Resource.Id.w12).Text = "5";
//				} else {
//					dummyView.FindViewById <TextView> (Resource.Id.w1).Text = report.FindVisitCountValue (listOfWeekNum[0]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w2).Text = report.FindVisitCountValue (listOfWeekNum[1]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w3).Text = report.FindVisitCountValue (listOfWeekNum[2]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w4).Text = report.FindVisitCountValue (listOfWeekNum[3]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w5).Text = report.FindVisitCountValue (listOfWeekNum[4]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w6).Text = report.FindVisitCountValue (listOfWeekNum[5]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w7).Text = report.FindVisitCountValue (listOfWeekNum[6]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w8).Text = report.FindVisitCountValue (listOfWeekNum[7]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w9).Text = report.FindVisitCountValue (listOfWeekNum[8]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w10).Text = report.FindVisitCountValue (listOfWeekNum[9]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w11).Text = report.FindVisitCountValue (listOfWeekNum[10]).ToString();
//					dummyView.FindViewById <TextView> (Resource.Id.w12).Text = report.FindVisitCountValue (listOfWeekNum[11]).ToString();
//				}
				dummyView.FindViewById <TextView> (Resource.Id.txtFIO).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w1).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w2).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w3).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w4).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w5).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w6).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w7).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w8).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w9).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w10).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w11).LayoutParameters.Height = 0;
//				dummyView.FindViewById <TextView> (Resource.Id.w12).LayoutParameters.Height = 0;
//				dummyView.FindViewById<TableRow> (Resource.Id.trContent).LayoutParameters.Height = 0;
				tlHeader.AddView (dummyView);
			}
		}

		void MakeTableHeader () {
			var view = (TableRow)inflater.Inflate (Resource.Layout.TableRowHeader, null);
			view.FindViewById <TextView> (Resource.Id.txtFIO).Text =  "ФИО";

			listOfWeekNum = new List<int> ();
			var min = ReportManager.GetMinWeekNum ();
			view.FindViewById <TextView> (Resource.Id.h1).Text =  min.ToString();
			view.FindViewById <TextView> (Resource.Id.h2).Text =  (min + 1).ToString();
			view.FindViewById <TextView> (Resource.Id.h3).Text =  (min + 2).ToString();
			view.FindViewById <TextView> (Resource.Id.h4).Text =  (min + 3).ToString();
			view.FindViewById <TextView> (Resource.Id.h5).Text =  (min + 4).ToString();
			view.FindViewById <TextView> (Resource.Id.h6).Text =  (min + 5).ToString();
			view.FindViewById <TextView> (Resource.Id.h7).Text =  (min + 6).ToString();
			view.FindViewById <TextView> (Resource.Id.h8).Text =  (min + 7).ToString();
			view.FindViewById <TextView> (Resource.Id.h9).Text =  (min + 8).ToString();
			view.FindViewById <TextView> (Resource.Id.h10).Text =  (min + 9).ToString();
			view.FindViewById <TextView> (Resource.Id.h11).Text =  (min + 10).ToString();
			view.FindViewById <TextView> (Resource.Id.h12).Text =  (min + 11).ToString();
			for (int i = 0; i < 12; i++) {
				listOfWeekNum.Add (min);
				min = min + 1;
			}
			tlHeader.AddView (view);
		}


	}
}

