using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.DXperience.Demos;
using DevExpress.XtraScheduler;

namespace GPTS
{
    public class TutorialControl : TutorialControlBase, IDXMenuManager {
		private LookAndFeelMenu menu = null;
        IDXMenuManager fMenuManager;

		public TutorialControl() {}
		
		public virtual SchedulerControl PrintingSchedulerControl { get { return null; } }

		public LookAndFeelMenu DemoMainMenu {
			get { return menu; }
			set {
				if(menu == value) return;
				this.menu = value;
			}
		}

        void IDXMenuManager.ShowPopupMenu(DXPopupMenu menu, Control control, Point pos) {
            MenuManagerHelper.ShowMenu(menu, LookAndFeel, fMenuManager, control, pos);
        }
		IDXMenuManager IDXMenuManager.Clone(Form newForm) { return this; }
		void IDXMenuManager.DisposeManager() { }

        public IDXMenuManager MenuManager {
            get { return fMenuManager; }
            set { fMenuManager = value; }
        }

		public virtual bool ShowOptions { get { return false; }}

		void OnSwitchStyle(object sender, EventArgs e) {
			OnSwitchStyle();
			//MessageBox.Show("Style Changed: " + AppearanceMenu.NeedPaintAppearance.ToString());
		}
    }
}
