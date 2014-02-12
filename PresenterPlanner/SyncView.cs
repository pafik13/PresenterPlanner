using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using PresenterPlanner.Lib;
using PresenterPlanner.Lib.Report;

namespace PresenterPlanner
{
	[Activity (Label = "Обновления и синхронизация")]			
	public class SyncView : Activity
	{
		protected Setts settings;
		protected string filePath = "";
		protected string type = "application/vnd.android.package-archive"; 
		protected Button btnUpdateApp;
		protected TextView txtVersion;
		protected ProgressBar pB;

		protected String versionOfNew = "";
		protected String versionCurrent = "";
		protected String downLoadPath = "";
		protected bool isDownLoading = false;

		WebClient client = null;

		public Button btnUploadFiles;
		public TextView txtProgress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SyncView);

			settings = Common.GetSettings ();
			filePath = Path.Combine(Common.DatabaseFileDir(), settings.packageName);

			versionCurrent = PackageManager.GetPackageInfo (PackageName, PackageInfoFlags.Activities).VersionName;

			client = new WebClient();

			client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => {
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(e.Result);
				versionOfNew = doc.GetElementById ("version").InnerText;
				if ((versionOfNew != "") && (versionOfNew != versionCurrent)) {
					RunOnUiThread(() => CheckLoadPackage());
				}
			};

			client.DownloadDataCompleted += (object sender, DownloadDataCompletedEventArgs e) => {
				File.WriteAllBytes(filePath, e.Result);
				RunOnUiThread(() => Toast.MakeText(this, "ALL", ToastLength.Short).Show());
				RunOnUiThread(() => pB.Visibility = ViewStates.Gone);
				RunOnUiThread(() => CheckLoadPackage ());
			};

			client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) => {
				if (pB.Visibility == ViewStates.Visible) {
					RunOnUiThread(() => pB.Activated = true);
					RunOnUiThread(() => pB.Max = (int)e.TotalBytesToReceive);
					RunOnUiThread(() => pB.Progress = (int)e.BytesReceived);
				}
			};

			pB = FindViewById <ProgressBar> (Resource.Id.StatusProgress);
			pB.Visibility = ViewStates.Gone;

			txtVersion = FindViewById <TextView> (Resource.Id.txtVersion);
			txtVersion.Text = "Текущая версия программы: " + PackageManager.GetPackageInfo (PackageName, PackageInfoFlags.Activities).VersionName; 

			btnUpdateApp = FindViewById <Button> (Resource.Id.btnUpdateApp);
			btnUpdateApp.Visibility = ViewStates.Gone;

			txtProgress = FindViewById <TextView> (Resource.Id.txtProgress);
			txtProgress.Text = "Идет сверка информации с сервером...";
			btnUploadFiles = FindViewById <Button> (Resource.Id.btnUploadFiles);
			btnUploadFiles.Visibility = ViewStates.Gone;

			btnUploadFiles.Click += (object sender, EventArgs e) => {
				var t = new Thread (() => {
					string loadResult = "";
					RunOnUiThread(() => pB.Visibility = ViewStates.Visible);
					RunOnUiThread(() => pB.Activated = true);
					RunOnUiThread(() => pB.Max = 4);
					RunOnUiThread(() => pB.Progress = 0);
					RunOnUiThread(() => txtProgress.Text = "Идет синхронизация с сервером...");
					loadResult = UploadFiles.LoadFile("DoctorDB");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 1);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					//RunOnUiThread(() => txtProgress.Text = "load HospitalDB");
					loadResult = UploadFiles.LoadFile("HospitalDB");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 2);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					//RunOnUiThread(() => txtProgress.Text = "load Demonstration");
					loadResult = UploadFiles.LoadFile("Demonstration");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 3);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					if (loadResult.Contains("PROCESS GOOD")) {
						DemonstrationManager.CurrentDemonstrationToArchive();

						string reportString = UploadFiles.DownLoadReport();

						var serializer = new XmlSerializer (typeof(List<Report>));
						using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(reportString))) {
							List<Report> dlReport  = (List<Report>)serializer.Deserialize (stream);
							List<Report> curReport = (List<Report>)ReportManager.GetReports();
							if (curReport.Count == 0 ) {
								curReport = dlReport;
							} else {
								for (int dl=0; dl<dlReport.Count; dl++) {
									var report = curReport.Find(r => r.doctorID == dlReport[dl].doctorID);
									if (report == null) {
										curReport.Add(dlReport[dl]);
									} else {
										for (int i=0; i<dlReport[dl].rItems.Count; i++) {
											var rItem = report.rItems.Find(ri => ri.weekNum == dlReport[dl].rItems[i].weekNum);
											if (rItem == null) {
												report.rItems.Add(dlReport[dl].rItems[i]);
											} else {
												rItem.visitCount = rItem.visitCount + dlReport[dl].rItems[i].visitCount;
											}
										}
									}
								}
							}
							if (ReportManager.SaveReports(curReport)) {
								Console.WriteLine ("Report saved...");					
								//RunOnUiThread(() => txtProgress.Text = "Report saved...");
							} else {
								Console.WriteLine ("Report not saved...");
								//RunOnUiThread(() => txtProgress.Text = "Report not saved...");
							}
						}
					}
					RunOnUiThread(() => pB.Progress = 4);
					Thread.Sleep(2500);
					Console.WriteLine ("Uploading finished...");
					RunOnUiThread(() => pB.Visibility = ViewStates.Gone);
					RunOnUiThread(() => txtProgress.Visibility = ViewStates.Visible);
					RunOnUiThread(() => txtProgress.Text = "Синхронизация успешно завершена.");
					RunOnUiThread(() => btnUploadFiles.Visibility = ViewStates.Invisible);
					}
				);
				t.Start ();
				//RunOnUiThread(() => pB.Activated = true);
				//RunOnUiThread(() => pB.Max = (int)e.TotalBytesToReceive);
				//RunOnUiThread(() => pB.Progress = (int)e.BytesReceived);
				//txtProgress.Text = "load DoctorDB";
				//txtProgress.Text = UploadFiles.LoadFile("DoctorDB");//Toast.MakeText(this, "Toast within progress dialog.", ToastLength.Long).Show());
				//txtProgress.Text = "load HospitalDB";
				//txtProgress.Text = UploadFiles.LoadFile("HospitalDB");
				//Console.WriteLine ("Uploading finished...");
			};
		}

		protected override void OnResume ()
		{
			//CheckNeWVersion ();
			UploadFiles uf = new UploadFiles (this);
			uf.CheckFiles ();
			base.OnResume ();
		}

		public void CheckNeWVersion () {
			if (DownLoad.HasConnection(this) && !client.IsBusy) {
				client.DownloadStringAsync (new Uri (Common.VersionFileLink));
			}
		}

		protected void CheckLoadPackage () {
			if (File.Exists (filePath)) {
				versionOfNew = this.PackageManager.GetPackageArchiveInfo (filePath, PackageInfoFlags.Activities).VersionName;
				if (versionCurrent == versionOfNew) {
					File.Delete (filePath);
				} else {
					btnUpdateApp.Text = "Установить обновление программы";
					btnUpdateApp.Visibility = ViewStates.Visible;
					btnUpdateApp.Click -= OnClickDownLoad;
					btnUpdateApp.Click += OnClickInstall;
				}
			} else {
				btnUpdateApp.Text = "Скачать обновление программы";
				btnUpdateApp.Visibility = ViewStates.Visible;
				btnUpdateApp.Click -= OnClickInstall;
				btnUpdateApp.Click += OnClickDownLoad;
			}
		}

		public void OnClickDownLoad(object sender, EventArgs e) {
			pB.Visibility = ViewStates.Visible;
			client.DownloadDataAsync (new Uri (settings.dlSite + settings.packageName));
		}

		public void OnClickInstall (object sender, EventArgs e) {
			Intent updateApp = new Intent(Intent.ActionView);
			updateApp.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(filePath)), type);
			StartActivity(updateApp);
		}
	}
}

