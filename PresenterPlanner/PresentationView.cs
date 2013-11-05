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
using PresenterPlanner.Lib;
using Android.Locations;
using System.Threading;

namespace PresenterPlanner
{
	[Activity (Label = "PresentationView")]			
	public class PresentationView : Activity, GestureDetector.IOnGestureListener, ILocationListener
	{
		GestureDetector gestureDetector;
		List <Presentation> preses;
		int selectedPresent = 0;
		int selectedPart = 0;
		int selectedSlide = 0;
		LinearLayout.LayoutParams lParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.FillParent,LinearLayout.LayoutParams.FillParent,1);
		ImageView ivSlide;

		LocationManager _locMgr;
		bool IsEnd = false;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.PresentationView);
			gestureDetector = new GestureDetector (this);
			_locMgr = GetSystemService (Context.LocationService) as LocationManager;

			preses = Presentations.GetPresentations ();
			ivSlide = FindViewById<ImageView> (Resource.Id.ivSlide);

			selectedPresent = Intent.GetIntExtra ("presentationID", 0);

			CreateTopLayout ();
			RefreshParts ();

			OnButtonTopClick (preses [selectedPresent].btn, null);

			OnButtonBottomClick(preses[selectedPresent].parts [Intent.GetIntExtra ("partID", 0)].btn, null);
		}

		//Top
		void CreateTopLayout(){
			LinearLayout llTop = FindViewById<LinearLayout> (Resource.Id.llTop);
			for (int i=0; i<=preses.Count-1; i++) {
				Button btnTop = new Button (this);
				btnTop.LayoutParameters = lParams;
				btnTop.Text = preses [i].name;
				btnTop.Tag = i;
				btnTop.Click += OnButtonTopClick;
				preses[i].btn = btnTop;
				llTop.AddView (btnTop);
			}
		}

		//Bottom
		void RefreshParts (){
			LinearLayout llBottom = FindViewById<LinearLayout> (Resource.Id.llBottom);
			llBottom.RemoveAllViews ();
			for (int i=0; i<=preses[selectedPresent].parts.Count-1; i++) {
				Button btnBottom = new Button (this);
				btnBottom.LayoutParameters = lParams;
				btnBottom.Text = preses[selectedPresent].parts[i].name;
				btnBottom.Tag = i;
				btnBottom.Click += OnButtonBottomClick;
				preses [selectedPresent].parts [i].btn = btnBottom;
				llBottom.AddView (btnBottom);
			}
		}

		void OnButtonTopClick (object sender, EventArgs e) {
			ivSlide.Visibility = ViewStates.Visible;
			preses [selectedPresent].btn.Enabled = true;
			selectedPresent = (int)(sender as Button).Tag; 
			(sender as Button).Enabled = false;
			RefreshParts ();
			selectedPart = 0;
			OnButtonBottomClick(preses [selectedPresent].parts [selectedPart].btn, null);
		}

		void OnButtonBottomClick (object sender, EventArgs e) {
			ivSlide.Visibility = ViewStates.Visible;
			preses [selectedPresent].parts [selectedPart].btn.Enabled = true;
			selectedPart = (int)(sender as Button).Tag; 
			(sender as Button).Enabled = false;
			selectedSlide = 0;
			ivSlide.SetImageBitmap (preses [selectedPresent].parts [selectedPart].slides [selectedSlide].Image);
		}

		void NextSlide() {
			if (preses [selectedPresent].parts [selectedPart].slides.Count - 1 < selectedSlide + 1) {
				if (preses [selectedPresent].parts.Count - 1 < selectedPart + 1) {
					if (preses.Count - 1 < selectedPresent + 1) {
						Android.Widget.Toast.MakeText (this, "КОНЕЦ!", Android.Widget.ToastLength.Short).Show ();
						selectedSlide = preses [selectedPresent].parts [selectedPart].slides.Count;
						ShowConent();
					} else {
						OnButtonTopClick(preses [selectedPresent + 1].btn, null);
					}
				} else {
					OnButtonBottomClick(preses [selectedPresent].parts [selectedPart + 1].btn, null);
				}
			} else {
				selectedSlide++;
				ivSlide.SetImageBitmap (preses [selectedPresent].parts [selectedPart].slides [selectedSlide].Image); 
			}
		}

		void PrevSlide(){
			ivSlide.Visibility = ViewStates.Visible;
			IsEnd = false;

			if (selectedSlide - 1 < 0) {
				if (selectedPart - 1 < 0) {
					if (selectedPresent - 1 < 0) {
						Android.Widget.Toast.MakeText (this, "НАЧАЛО!", Android.Widget.ToastLength.Short).Show ();
					} else {
						OnButtonTopClick(preses [selectedPresent - 1].btn, null);
						OnButtonBottomClick(preses [selectedPresent].parts [preses [selectedPresent].parts.Count - 1].btn, null);
						selectedSlide = preses [selectedPresent].parts [selectedPart].slides.Count - 1;
						ivSlide.SetImageBitmap (preses [selectedPresent].parts [selectedPart].slides [selectedSlide].Image);
					}
				} else {
					OnButtonBottomClick(preses [selectedPresent].parts [selectedPart - 1].btn, null);
					selectedSlide = preses [selectedPresent].parts [selectedPart].slides.Count - 1;
					ivSlide.SetImageBitmap (preses [selectedPresent].parts [selectedPart].slides [selectedSlide].Image);
				}
			} else {
				selectedSlide--;
				ivSlide.SetImageBitmap (preses [selectedPresent].parts [selectedPart].slides [selectedSlide].Image); 
			}
		}

		void ShowConent(){
			ivSlide.Visibility = ViewStates.Invisible;
			IsEnd = true;
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			gestureDetector.OnTouchEvent (e);
			return base.OnTouchEvent (e);
		}

		public bool OnFling( MotionEvent e1, MotionEvent e2, float velocityX, float velocityY){
			TextView txtView = FindViewById <TextView> (Resource.Id.txtView);
			txtView.Text = string.Format ("Fling velocity: {0} x {1}", velocityX, velocityY);
			if (velocityX > 1500) {
				PrevSlide ();
			}

			if (velocityX < -1500) {
				NextSlide ();
			}
			return true;
		}

		public bool OnDown(MotionEvent e){
			return false;
		}

		public void OnLongPress(MotionEvent e){
		}

		public bool OnScroll(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY){
			return false;
		}

		public void OnShowPress(MotionEvent e){
		}

		public bool OnSingleTapUp(MotionEvent e){
			return false;
		}

				protected override void OnResume ()
				{
					base.OnResume ();
		
					//-----------------------------------------------------------------------//
					var locationCriteria = new Criteria ();
		
					locationCriteria.Accuracy = Accuracy.NoRequirement;
					locationCriteria.PowerRequirement = Power.NoRequirement;
		
					string locationProvider = _locMgr.GetBestProvider (locationCriteria, true);
		
					_locMgr.RequestLocationUpdates (locationProvider, 2000, 1, this);
				}
		
				protected override void OnPause ()
				{
					base.OnPause ();
		
					_locMgr.RemoveUpdates (this);
				}
		
				#region ILocationListener implementation
				public void OnLocationChanged (Location location)
				{
					if (IsEnd) {
						(FindViewById<TextView>(Resource.Id.txtView)).Append(String.Format ("\r\n\r\nLatitude = {0}, Longitude = {1}", location.Latitude, location.Longitude));
					}
					
		
					// demo geocoder
//					Geocoder geocdr = new Geocoder(this);
//
//					System.Threading.Tasks.Task<IList<Address>> getAddressTask = geocdr.GetFromLocationAsync(location.Latitude, location.Longitude, 5);
//					adrs = "Trying to reverse geo-code the latitude/longitude...";
//					
//					IList<Address> addresses = await getAddressTask;
//
//					if (addresses.Any())
//					{
//						Address addr = addresses.First();
//						adrs = addr.ToString();
//					}
//					else
//					{
//						Toast.MakeText(this, "Could not reverse geo-code the location", ToastLength.Short).Show();
//					}
				}
		
				public void OnProviderDisabled (string provider)
				{
		
				}
		
				public void OnProviderEnabled (string provider)
				{
		
				}
		
				public void OnStatusChanged (string provider, Availability status, Bundle extras)
				{
		
				}
				#endregion
	}
}

