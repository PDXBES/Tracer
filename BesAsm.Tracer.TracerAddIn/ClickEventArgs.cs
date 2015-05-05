using System;

namespace BesAsm.Tracer.TracerAddIn
{
	/// <summary>
	/// Summary description for ClickEventArgs.
	/// </summary>
	public class ClickEventArgs : EventArgs
	{
		private int button;
		private int shift;		
		private int x;
		private int y;
		
		public ClickEventArgs(int button, int shift, int x, int y)
		{
			this.button = button;
			this.shift = shift;
			this.x = x;
			this.y = y;
		}

		public int Button
		{
			get { return this.button; }
		}
		public int Shift
		{
			get { return this.shift; }
		}
		public int X
		{
			get { return this.x; }
		}
		public int Y
		{
			get { return this.y; }
		}


	}
}
