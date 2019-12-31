namespace GPTS
{
    partial class frmTestComponet
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraScheduler.TimeRuler timeRuler1 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeRuler timeRuler2 = new DevExpress.XtraScheduler.TimeRuler();
            this.schedulerControl1 = new DevExpress.XtraScheduler.SchedulerControl();
            this.schedulerStorage1 = new DevExpress.XtraScheduler.SchedulerStorage(this.components);
            this.dateNavigator1 = new DevExpress.XtraScheduler.DateNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1)).BeginInit();
            this.SuspendLayout();
            // 
            // schedulerControl1
            // 
            this.schedulerControl1.Appearance.AlternateHeaderCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Appearance.AlternateHeaderCaption.Options.UseFont = true;
            this.schedulerControl1.Appearance.AlternateHeaderCaptionLine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Appearance.AlternateHeaderCaptionLine.ForeColor = System.Drawing.Color.Red;
            this.schedulerControl1.Appearance.AlternateHeaderCaptionLine.Options.UseFont = true;
            this.schedulerControl1.Appearance.AlternateHeaderCaptionLine.Options.UseForeColor = true;
            this.schedulerControl1.Appearance.HeaderCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Appearance.HeaderCaption.Options.UseFont = true;
            this.schedulerControl1.Appearance.HeaderCaptionLine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.schedulerControl1.Appearance.HeaderCaptionLine.Options.UseFont = true;
            this.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedulerControl1.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Date;
            this.schedulerControl1.Location = new System.Drawing.Point(0, 0);
            this.schedulerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.schedulerControl1.Name = "schedulerControl1";
            this.schedulerControl1.OptionsCustomization.AllowInplaceEditor = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.Size = new System.Drawing.Size(784, 699);
            this.schedulerControl1.Start = new System.DateTime(2016, 11, 11, 0, 0, 0, 0);
            this.schedulerControl1.Storage = this.schedulerStorage1;
            this.schedulerControl1.TabIndex = 6;
            this.schedulerControl1.Text = "schedulerControl1";
            this.schedulerControl1.Views.DayView.NavigationButtonAppointmentSearchInterval = System.TimeSpan.Parse("08:00:00");
            timeRuler1.TimeZone.DaylightBias = System.TimeSpan.Parse("-01:00:00");
            timeRuler1.TimeZone.DaylightZoneName = "Greenwich Yaz Saati";
            timeRuler1.TimeZone.DisplayName = "(UTC+00:00) Monrovya, Reykjavik";
            timeRuler1.TimeZone.StandardZoneName = "Greenwich Standart Saati";
            timeRuler1.UseClientTimeZone = false;
            this.schedulerControl1.Views.DayView.TimeRulers.Add(timeRuler1);
            this.schedulerControl1.Views.DayView.VisibleTime.End = System.TimeSpan.Parse("21:00:00");
            this.schedulerControl1.Views.DayView.VisibleTime.Start = System.TimeSpan.Parse("08:00:00");
            this.schedulerControl1.Views.DayView.WorkTime.End = System.TimeSpan.Parse("20:00:00");
            this.schedulerControl1.Views.DayView.WorkTime.Start = System.TimeSpan.Parse("08:00:00");
            timeRuler2.TimeZone.Id = DevExpress.XtraScheduler.TimeZoneId.Balkan;
            timeRuler2.UseClientTimeZone = false;
            this.schedulerControl1.Views.WorkWeekView.TimeRulers.Add(timeRuler2);
            this.schedulerControl1.MoreButtonClicked += new DevExpress.XtraScheduler.MoreButtonClickedEventHandler(this.schedulerControl1_MoreButtonClicked);
            // 
            // schedulerStorage1
            // 
            this.schedulerStorage1.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel("Boş", "&Boş"));
            this.schedulerStorage1.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(System.Drawing.Color.LightYellow, "Bekliyor...", "&Bekliyor..."));
            this.schedulerStorage1.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(System.Drawing.Color.LightGreen, "Geldi", "&Geldi"));
            this.schedulerStorage1.Appointments.Labels.Add(new DevExpress.XtraScheduler.AppointmentLabel(System.Drawing.Color.Red, "Gelmedi", "&Gelmedi"));
            this.schedulerStorage1.AppointmentChanging += new DevExpress.XtraScheduler.PersistentObjectCancelEventHandler(this.schedulerStorage1_AppointmentChanging);
            // 
            // dateNavigator1
            // 
            this.dateNavigator1.AppearanceCalendar.Font = new System.Drawing.Font("Tahoma", 9F);
            this.dateNavigator1.AppearanceCalendar.Options.UseFont = true;
            this.dateNavigator1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dateNavigator1.HotDate = null;
            this.dateNavigator1.Location = new System.Drawing.Point(784, 0);
            this.dateNavigator1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateNavigator1.Name = "dateNavigator1";
            this.dateNavigator1.SchedulerControl = this.schedulerControl1;
            this.dateNavigator1.ShowTodayButton = false;
            this.dateNavigator1.Size = new System.Drawing.Size(213, 699);
            this.dateNavigator1.TabIndex = 25;
            // 
            // frmTestComponet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 699);
            this.Controls.Add(this.schedulerControl1);
            this.Controls.Add(this.dateNavigator1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmTestComponet";
            this.Text = "frmTestComponet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmYukleniyor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateNavigator1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraScheduler.SchedulerControl schedulerControl1;
        private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage1;
        private DevExpress.XtraScheduler.DateNavigator dateNavigator1;
    }
}