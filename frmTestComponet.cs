using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GPTS
{
    public partial class frmTestComponet : Form
    {
        public static Random RandomInstance = new Random();

        private BindingList<CustomResource> CustomResourceCollection = new BindingList<CustomResource>();
        private BindingList<CustomAppointment> CustomEventList = new BindingList<CustomAppointment>();

        public frmTestComponet()
        {
            InitializeComponent();
        }
       
        private void frmYukleniyor_Load(object sender, EventArgs e)
        {
            InitResources();
            InitAppointments();
            schedulerControl1.Start = DateTime.Now;
        }

        private void InitResources()
        {
            ResourceMappingInfo mappings = this.schedulerStorage1.Resources.Mappings;
            mappings.Id = "ResID";
            mappings.Color = "ResColor";
            mappings.Caption = "Name";

            CustomResourceCollection.Add(CreateCustomResource(1, "Max Fowler", Color.PowderBlue));
            CustomResourceCollection.Add(CreateCustomResource(2, "Nancy Drewmore", Color.PaleVioletRed));
            CustomResourceCollection.Add(CreateCustomResource(3, "Pak Jang", Color.PeachPuff));
            CustomResourceCollection.Add(CreateCustomResource(4, "Mike Brown", Color.PeachPuff));
            //CustomResourceCollection.Add(CreateCustomResource(5, "Sara Connor", Color.PeachPuff));
            this.schedulerStorage1.Resources.DataSource = CustomResourceCollection;
        }

        private CustomResource CreateCustomResource(int res_id, string caption, Color ResColor)
        {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            cr.ResColor = ResColor;
            return cr;
        }

        private void InitAppointments()
        {
            AppointmentMappingInfo mappings = this.schedulerStorage1.Appointments.Mappings;
            mappings.Start = "StartTime";
            mappings.End = "EndTime";
            mappings.Subject = "Subject";
            mappings.AllDay = "AllDay";
            mappings.Description = "Description";
            mappings.Label = "Label";
            mappings.Location = "Location";
            mappings.RecurrenceInfo = "RecurrenceInfo";
            mappings.ReminderInfo = "ReminderInfo";
            mappings.ResourceId = "OwnerId";
            mappings.Status = "Status";
            mappings.Type = "EventType";

            GenerateEvents(CustomEventList);
            this.schedulerStorage1.Appointments.DataSource = CustomEventList;
        }

        private void GenerateEvents(BindingList<CustomAppointment> eventList)
        {
            int count = schedulerStorage1.Resources.Count;
            DataTable dt = DB.GetData("select * from Hatirlatma");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Resource resource = schedulerStorage1.Resources[i];


                string odaid = dt.Rows[i]["fkOda"].ToString();
                if (odaid == "") odaid = "1";
                int fkOda = int.Parse(odaid);

                //string subjPrefix = "'s ";
                //resource.Caption + "'s ";

                DateTime ilktar = Convert.ToDateTime(dt.Rows[i]["Tarih"].ToString()); //DateTime.Now.AddHours(-2);
                DateTime sontar = Convert.ToDateTime(dt.Rows[i]["BitisTarihi"].ToString());//DateTime.Now.AddHours(-1);

                string ack = dt.Rows[i]["Aciklama"].ToString();
                int fkDurumu = 1;
                if (dt.Rows[i]["fkDurumu"].ToString() != "")
                    fkDurumu = int.Parse(dt.Rows[i]["fkDurumu"].ToString());

                eventList.Add(CreateEvent(ack, fkOda,0, fkDurumu, 0,ilktar,sontar));
                
                //eventList.Add(CreateEvent(subjPrefix + "travel", resource.Id, 3, 6, 1));
                //eventList.Add(CreateEvent(subjPrefix + "phone call", resource.Id, 0, 10, 2));
                //eventList.Add(CreateEvent(subjPrefix + "meeting", resource.Id, 2, 5, 3));
                //eventList.Add(CreateEvent(subjPrefix + "travel", resource.Id, 3, 6, 4));
                //eventList.Add(CreateEvent(subjPrefix + "phone call", resource.Id, 0, 10, 5));
            }
        }
        private CustomAppointment CreateEvent(string subject, object resourceId, int status, int label, int dayShift,DateTime ilktar,DateTime sontar)
        {
            CustomAppointment apt = new CustomAppointment();
            apt.Subject = subject;
            apt.OwnerId = resourceId;
            Random rnd = RandomInstance;
            apt.StartTime = ilktar;//DateTime.Now.AddHours(-2);//.AddDays(dayShift);
            apt.EndTime = sontar;//DateTime.Now.AddHours(-1);//.AddDays(dayShift);//apt.StartTime.AddDays(1);
            apt.Status = status;
            apt.Label = label;
            apt.Description = subject;
            apt.EventType = 0;//tüm gün mü tüm yıl mı
            apt.Location = "yer";
            //apt.CustomFields["pkHatirlatma"] =
            //hatırlat
            //apt.ReminderInfo = new string("a");
            return apt;
        }

        private void schedulerControl1_MoreButtonClicked(object sender, MoreButtonClickedEventArgs e)
        {
            TimeIntervalCollection newIntervalCollection = new TimeIntervalCollection();
            newIntervalCollection.Add(new TimeInterval(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(7)));
            schedulerControl1.ActiveView.SetVisibleIntervals(newIntervalCollection);
            e.Handled = true;
        }

        private void schedulerStorage1_AppointmentChanging(object sender, PersistentObjectCancelEventArgs e)
        {
            DevExpress.XtraScheduler.Appointment apt = e.Object as DevExpress.XtraScheduler.Appointment;
        }
    }
}
