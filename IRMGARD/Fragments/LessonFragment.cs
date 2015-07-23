
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IRMGARD
{	
	public abstract class LessonFragment : Fragment
	{
		public delegate void FinishedEventHandler(object sender, EventArgs e);
		public event FinishedEventHandler Finished;

		protected void FireFinished(EventArgs e)
		{
			if (Finished != null)
				Finished (this, e);
		}
	}
}

