using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;
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

		protected Dictionary <string, DLFile> files = null;

		protected String vrsnAppNew = "";
		protected String vrsnAppCurr = "";
		protected String vrsnPrsCurr = "";
		protected String downLoadPath = "";
		protected bool isDownLoading = false;

		WebClient client = null;

		public Button btnUploadFiles;
		public Button btnDownLoadPresents;
		public TextView txtProgress;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SyncView);

			Window.AddFlags (WindowManagerFlags.KeepScreenOn);

			settings = Common.GetSettings ();

			files = new Dictionary <string, DLFile>();

			files.Add("App", new DLFile () { path = Path.Combine(Common.DatabaseFileDir(), settings.packageName), isNew = false });

			files.Add("Prs", new DLFile () { path = Path.Combine(Common.DatabaseFileDir(), "presents.xml.gz"), isNew = false });
//			filePath = Path.Combine(Common.DatabaseFileDir(), settings.packageName);

			vrsnAppCurr = PackageManager.GetPackageInfo (PackageName, PackageInfoFlags.Activities).VersionName;
//			vrsnPrsCurr = settings.vrsnOfPresents;

			client = new WebClient();

			client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) => {
				if (File.Exists(files["Prs"].path)) {
					string fileText = File.ReadAllText (files["Prs"].path);
					vrsnPrsCurr = UploadFiles.GetMd5Hash (fileText);
				} else {
					vrsnPrsCurr = "";
				};
				try {
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(e.Result);
					files["App"].vrsnNew = doc.GetElementById ("versionApp").InnerText;
					files["Prs"].vrsnNew = doc.GetElementById ("versionPrs").InnerText;
					files["App"].isNew = !bool.Equals(vrsnAppCurr, files["App"].vrsnNew);
					files["Prs"].isNew = !bool.Equals(vrsnPrsCurr, files["Prs"].vrsnNew);

					if ((files["App"].isNew) || (files["Prs"].isNew)) {
						RunOnUiThread(() => CheckLoadPackage());
					}
				} catch (Exception exc) {
					RunOnUiThread(() => Toast.MakeText (this, exc.Message, ToastLength.Long).Show ());
				}
			};

			client.DownloadDataCompleted += (object sender, DownloadDataCompletedEventArgs e) => {
				if (File.Exists (filePath)) {
					File.Delete (filePath);
				};
				try {
					File.WriteAllBytes(filePath, e.Result);
					RunOnUiThread(() => txtProgress.Text = "Загрузка окончена");
					RunOnUiThread(() => Toast.MakeText(this, "ALL", ToastLength.Short).Show());
					RunOnUiThread(() => pB.Visibility = ViewStates.Gone);
					RunOnUiThread(() => CheckLoadPackage ());
				} catch(Exception exc) {
					RunOnUiThread(() => Toast.MakeText (this, exc.Message, ToastLength.Long).Show ());
				}
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

			btnDownLoadPresents = FindViewById <Button> (Resource.Id.btnDownLoadPresents);
			btnDownLoadPresents.Visibility = ViewStates.Gone;
			btnDownLoadPresents.Click += (object sender, EventArgs e) => {
				pB.Visibility = ViewStates.Visible;
				if (DownLoad.HasConnection (this) && !client.IsBusy) {
					filePath = files ["Prs"].path;
					client.DownloadDataAsync (new Uri (settings.dlSite + "presents.xml.gz"));
					files ["Prs"].isNew = false;
					btnDownLoadPresents.Visibility = ViewStates.Gone;
				} else {
					btnDownLoadPresents.Text = "Подождите окончания другой загрузки...";
				}
			};


			txtProgress = FindViewById <TextView> (Resource.Id.txtProgress);
			txtProgress.Text = "Идет сверка информации с сервером...";
			btnUploadFiles = FindViewById <Button> (Resource.Id.btnUploadFiles);
			btnUploadFiles.Visibility = ViewStates.Gone;

			btnUploadFiles.Click += (object sender, EventArgs e) => {
				var t = new Thread (() => {
					string loadResult = "";
					RunOnUiThread(() => pB.Visibility = ViewStates.Visible);
					RunOnUiThread(() => pB.Activated = true);
					RunOnUiThread(() => pB.Max = 5);
					RunOnUiThread(() => pB.Progress = 0);
					RunOnUiThread(() => txtProgress.Text = "Идет синхронизация с сервером...");
					loadResult = UploadFiles.LoadFile("Settings");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 1);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					loadResult = UploadFiles.LoadFile("DoctorDB");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 2);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					//RunOnUiThread(() => txtProgress.Text = "load HospitalDB");
					loadResult = UploadFiles.LoadFile("HospitalDB");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 3);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					//RunOnUiThread(() => txtProgress.Text = "load Demonstration");
					loadResult = UploadFiles.LoadFile("Demonstration");
					Console.WriteLine(loadResult);
					RunOnUiThread(() => pB.Progress = 4);
					//RunOnUiThread(() => txtProgress.Text = loadResult);
					Thread.Sleep(1000);
					if (loadResult.Contains("FILE SAVED")) {
						string reportString = UploadFiles.DownLoadReport();

						DemonstrationManager.CurrentDemonstrationToArchive();

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
					RunOnUiThread(() => pB.Progress = 5);
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

//			FileInfo fi = new FileInfo (Path.Combine(Common.DatabaseFileDir(), "presents.xml"));
//			Zip.Compress (fi);
			UploadFiles uf = new UploadFiles (this);
			uf.CheckFiles ();
			base.OnResume ();
		}

		public void CheckAppNewVersion () {
			if (DownLoad.HasConnection(this) && !client.IsBusy) {
				client.DownloadStringAsync (new Uri (Common.VersionFileLink));
			}
		}

		protected void CheckLoadPackage () {
			if (files["App"].isNew) {
				if (File.Exists (files["App"].path)) {
					vrsnAppNew = this.PackageManager.GetPackageArchiveInfo (files["App"].path, PackageInfoFlags.Activities).VersionName;
					float newVrsn = Convert.ToSingle(vrsnAppNew, new CultureInfo("en-US"));
					float curVrsn = Convert.ToSingle(vrsnAppCurr, new CultureInfo("en-US"));
					if (curVrsn >= newVrsn) {
						File.Delete (files["App"].path);
					} else {
						btnUpdateApp.Text = "Установить обновление программы";
						btnUpdateApp.Visibility = ViewStates.Visible;
						btnUpdateApp.Click -= OnClickDownLoadApp;
						btnUpdateApp.Click += OnClickInstallApp;
					}
				} else {
					btnUpdateApp.Text = "Скачать обновление программы";
					btnUpdateApp.Visibility = ViewStates.Visible;
					btnUpdateApp.Click -= OnClickInstallApp;
					btnUpdateApp.Click += OnClickDownLoadApp;
				}
			}
			if (files ["Prs"].isNew) {
				btnDownLoadPresents.Text = "Скачать новую версию презентаций";
				btnDownLoadPresents.Visibility = ViewStates.Visible;
			} else {
				if (File.Exists (files ["Prs"].path)) {
					if (!bool.Equals (vrsnPrsCurr, files ["Prs"].vrsnNew)) {
						try {
							File.Delete (Path.Combine (Common.DatabaseFileDir (), "presents.xml"));
							txtProgress.Text = "Идет разархивирование презентаций.";
							FileInfo fi = new FileInfo(files ["Prs"].path);
							Zip.Decompress (fi);
							settings.vrsnOfPresents = files ["Prs"].vrsnNew;
							Common.SetSettings (settings);
							txtProgress.Text = "Идет чтение и проверка презентаций.";
							DemonstrationManager.RefreshDemonstrations();
							txtProgress.Text = "Презентации загружены!";
						} catch(Exception exc) {
							Toast.MakeText (this, exc.Message, ToastLength.Long).Show ();
						}
					}
				} else {
					txtProgress.Text = "Ошибка. Попробуйте загрузить презентации позже...";
				}
			}
		}

		public void OnClickDownLoadApp (object sender, EventArgs e) {
			pB.Visibility = ViewStates.Visible;
			if (DownLoad.HasConnection (this) && !client.IsBusy) {
				filePath = files ["App"].path;
				client.DownloadDataAsync (new Uri (settings.dlSite + settings.packageName));
			}
		}

		public void OnClickInstallApp (object sender, EventArgs e) {
			Intent updateApp = new Intent(Intent.ActionView);
			updateApp.SetDataAndType(Android.Net.Uri.FromFile(new Java.IO.File(files ["App"].path)), type);
			StartActivity(updateApp);
		}
	}
}

