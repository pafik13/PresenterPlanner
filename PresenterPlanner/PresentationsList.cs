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
using PresenterPlanner.Presentations;

namespace PresenterPlanner
{
	[Activity (Label = "PresentationsList")]			
	public class PresentationsList : ListActivity
	{
		List <Presentation> preses;
		List<Tuple<string>> items;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			preses = Presentations.Presentations.GetPresentations (); 

			items = new List<Tuple<string>>();
			for (int i=0; i<=preses.Count-1; i++) {
				for (int j=0; j<=preses[i].parts.Count-1; j++) {
					items.Add(new Tuple<string>(preses[i].name+"."+preses[i].parts[j].name));
				}
			}

			this.ListAdapter = new ArrayAdapter<Tuple<string>> (this, Android.Resource.Layout.SimpleListItem1, items);
		}
		protected override void OnListItemClick(Android.Widget.ListView l, Android.Views.View v, int position, long id)
		{
			var t = items[position];
			Android.Widget.Toast.MakeText(this, t.Item1, Android.Widget.ToastLength.Short).Show();
			Console.WriteLine("Clicked on " + t);
			var slides = new Intent ( this, typeof(PresentationView));
			int presentationID = 0;
			int partID = position;
			for (int i=0; (i<=preses.Count-1) && (partID>preses[i].parts.Count-1); i++){
				presentationID = i+1;
				partID = partID - preses [i].parts.Count;
			}
			slides.PutExtra ("presentationID", presentationID);
			slides.PutExtra ("partID", partID);
			StartActivity (slides);
		}
	}
}

