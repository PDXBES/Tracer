using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BesAsm.Tracer.TracerAddIn
{
	/// <summary>
	/// Displays a progressbar during tracing to show progress as features caught
  /// by the native Arc tracer are collected into a GraphEdge collection
	/// </summary>
	public class FloatingProgressBar : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressBar1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FloatingProgressBar(int max)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.progressBar1.Maximum = max;
		}

		public void Increment()
		{
			this.progressBar1.PerformStep();			
		}

		

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressBar1.Location = new System.Drawing.Point(0, 0);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(292, 29);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 0;
			// 
			// FloatingProgressBar
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 29);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "FloatingProgressBar";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Creating Network...";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
