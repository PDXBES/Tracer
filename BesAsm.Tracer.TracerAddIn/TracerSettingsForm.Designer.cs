namespace BesAsm.Tracer.TracerAddIn
{
  partial class TracerSettingsForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOk = new System.Windows.Forms.Button();
      this.btnRemoveStop = new System.Windows.Forms.Button();
      this.btnRemoveStart = new System.Windows.Forms.Button();
      this.lstStopPipes = new System.Windows.Forms.ListBox();
      this.lstStartPipes = new System.Windows.Forms.ListBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cmbDownstreamNodeField = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cmbUpstreamNodeField = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.cmbFeatureLayer = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // checkBox1
      // 
      this.checkBox1.Checked = true;
      this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBox1.Location = new System.Drawing.Point(14, 270);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(104, 24);
      this.checkBox1.TabIndex = 31;
      this.checkBox1.Text = "Zoom to Trace?";
      // 
      // label5
      // 
      this.label5.Location = new System.Drawing.Point(150, 134);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(100, 16);
      this.label5.TabIndex = 30;
      this.label5.Text = "Stop Pipes";
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(14, 134);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(100, 16);
      this.label4.TabIndex = 29;
      this.label4.Text = "Start Pipes";
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(198, 270);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnOk
      // 
      this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOk.Location = new System.Drawing.Point(118, 270);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new System.Drawing.Size(72, 23);
      this.btnOk.TabIndex = 27;
      this.btnOk.Text = "OK";
      this.btnOk.Click += new System.EventHandler(this.btOk_Click);
      // 
      // btnRemoveStop
      // 
      this.btnRemoveStop.Location = new System.Drawing.Point(182, 238);
      this.btnRemoveStop.Name = "btnRemoveStop";
      this.btnRemoveStop.Size = new System.Drawing.Size(56, 23);
      this.btnRemoveStop.TabIndex = 26;
      this.btnRemoveStop.Text = "Remove";
      this.btnRemoveStop.Click += new System.EventHandler(this.btnRemoveStop_Click);
      // 
      // btnRemoveStart
      // 
      this.btnRemoveStart.Location = new System.Drawing.Point(46, 238);
      this.btnRemoveStart.Name = "btnRemoveStart";
      this.btnRemoveStart.Size = new System.Drawing.Size(56, 23);
      this.btnRemoveStart.TabIndex = 25;
      this.btnRemoveStart.Text = "Remove";
      this.btnRemoveStart.Click += new System.EventHandler(this.btnRemoveStart_Click);
      // 
      // lstStopPipes
      // 
      this.lstStopPipes.Location = new System.Drawing.Point(150, 150);
      this.lstStopPipes.Name = "lstStopPipes";
      this.lstStopPipes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstStopPipes.Size = new System.Drawing.Size(128, 82);
      this.lstStopPipes.TabIndex = 24;
      // 
      // lstStartPipes
      // 
      this.lstStartPipes.Location = new System.Drawing.Point(14, 150);
      this.lstStartPipes.Name = "lstStartPipes";
      this.lstStartPipes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstStartPipes.Size = new System.Drawing.Size(128, 82);
      this.lstStartPipes.TabIndex = 23;
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(14, 86);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(208, 16);
      this.label3.TabIndex = 22;
      this.label3.Text = "Select the Downstream Node Field:";
      // 
      // cmbDownstreamNodeField
      // 
      this.cmbDownstreamNodeField.Location = new System.Drawing.Point(14, 102);
      this.cmbDownstreamNodeField.Name = "cmbDownstreamNodeField";
      this.cmbDownstreamNodeField.Size = new System.Drawing.Size(264, 21);
      this.cmbDownstreamNodeField.Sorted = true;
      this.cmbDownstreamNodeField.TabIndex = 21;
      this.cmbDownstreamNodeField.SelectedIndexChanged += new System.EventHandler(this.cmbDownstreamNodeField_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(14, 46);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(216, 16);
      this.label2.TabIndex = 20;
      this.label2.Text = "Select the Upstream Node Field:";
      // 
      // cmbUpstreamNodeField
      // 
      this.cmbUpstreamNodeField.Location = new System.Drawing.Point(14, 62);
      this.cmbUpstreamNodeField.Name = "cmbUpstreamNodeField";
      this.cmbUpstreamNodeField.Size = new System.Drawing.Size(264, 21);
      this.cmbUpstreamNodeField.Sorted = true;
      this.cmbUpstreamNodeField.TabIndex = 19;
      this.cmbUpstreamNodeField.SelectedIndexChanged += new System.EventHandler(this.cmbUpstreamNodeField_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(14, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(168, 16);
      this.label1.TabIndex = 18;
      this.label1.Text = "Select a Polyline Feature Class:";
      // 
      // cmbFeatureLayer
      // 
      this.cmbFeatureLayer.Location = new System.Drawing.Point(14, 22);
      this.cmbFeatureLayer.Name = "cmbFeatureLayer";
      this.cmbFeatureLayer.Size = new System.Drawing.Size(264, 21);
      this.cmbFeatureLayer.Sorted = true;
      this.cmbFeatureLayer.TabIndex = 17;
      this.cmbFeatureLayer.SelectedIndexChanged += new System.EventHandler(this.cmbFeatureLayer_SelectedIndexChanged);
      // 
      // TracerSettingsForm
      // 
      this.AcceptButton = this.btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(292, 318);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOk);
      this.Controls.Add(this.btnRemoveStop);
      this.Controls.Add(this.btnRemoveStart);
      this.Controls.Add(this.lstStopPipes);
      this.Controls.Add(this.lstStartPipes);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.cmbDownstreamNodeField);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cmbUpstreamNodeField);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cmbFeatureLayer);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TracerSettingsForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Tracer Settings";
      this.TopMost = true;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TracerSettingsForm_FormClosing);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnRemoveStop;
    private System.Windows.Forms.Button btnRemoveStart;
    private System.Windows.Forms.ListBox lstStopPipes;
    private System.Windows.Forms.ListBox lstStartPipes;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox cmbDownstreamNodeField;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cmbUpstreamNodeField;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cmbFeatureLayer;
  }
}