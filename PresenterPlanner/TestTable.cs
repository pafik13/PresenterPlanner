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

namespace PresenterPlanner
{
	[Activity (Label = "TestTable")]			
	public class TestTable : Activity
	{
		protected TableLayout mainTable = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here

			SetContentView(Resource.Layout.TestTable);

			mainTable = FindViewById <TableLayout> (Resource.Id.maintable);

			for (int i=0; i<10 ; i++){

				// Create a TableRow and give it an ID
				TableRow tr = new TableRow(this);
				tr.Id = 100+i;
				tr.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent);

				// Create a TextView for column 1
				TextView col1 = new TextView(this);
				col1.Id = 200+i;
				col1.Text = ("col1");
				col1.SetPadding(0,0,2,0);          
				col1.SetTextColor(Android.Graphics.Color.Black);
				col1.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent);
				tr.AddView(col1);

				// Create a TextView for column 2
				TextView col2 = new TextView(this);
				col2.Id = 300 + i;
				col2.Text = "col2";
				col2.SetPadding(0,0,2,0);          
				col2.SetTextColor(Android.Graphics.Color.Black);
				col2.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent);
				tr.AddView(col2);

				// Create a TextView for column 3
				TextView col3 = new TextView(this);
				col3.Id = 500+i;
				col3.Text = DateTime.Now.ToString("dd.MM");          
				col3.SetTextColor(Android.Graphics.Color.Black);
				if (i%2 == 0)
				{
					col1.SetBackgroundColor(Android.Graphics.Color.White);
					col2.SetBackgroundColor(Android.Graphics.Color.White);
					col3.SetBackgroundColor(Android.Graphics.Color.White);
					tr.SetBackgroundColor(Android.Graphics.Color.White);
				}
				else
				{
					tr.SetBackgroundColor(Android.Graphics.Color.LightGray);
					col1.SetBackgroundColor(Android.Graphics.Color.LightGray);
					col2.SetBackgroundColor(Android.Graphics.Color.LightGray);
					col3.SetBackgroundColor(Android.Graphics.Color.LightGray);
				}
				col3.SetHorizontallyScrolling(false);
				col3.SetMaxLines(100);
				col3.LayoutParameters = new TableRow.LayoutParams(TableRow.LayoutParams.MatchParent, TableRow.LayoutParams.WrapContent, 1f);
				tr.AddView(col3);

				// Add the TableRow to the TableLayout
				mainTable.AddView(tr, new TableLayout.LayoutParams(TableLayout.LayoutParams.MatchParent, TableLayout.LayoutParams.WrapContent));
				//i++;
			}
		}
	}
}

