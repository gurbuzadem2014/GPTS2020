using System;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using DevExpress.DXperience.Demos;

namespace GPTS
{
    public class DemosInfo : ModulesInfo
    {
        public static void ShowModule(string name, DevExpress.XtraEditors.GroupControl groupControl,  IDXMenuManager menuManager, DevExpress.Utils.Frames.ApplicationCaption caption)
        {
            ModuleInfo item = DemosInfo.GetItem(name);
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Control oldTutorial = null;
                if (Instance.CurrentModuleBase != null)
                {
                    if (Instance.CurrentModuleBase.Name == name) return;
                    oldTutorial = Instance.CurrentModuleBase.TModule;
                }

                TutorialControl tutorial = item.TModule as TutorialControl;
                tutorial.Bounds = groupControl.DisplayRectangle;
                Instance.CurrentModuleBase = item;
                tutorial.Visible = false;
                groupControl.Controls.Add(tutorial);
                tutorial.Dock = DockStyle.Fill;

                //-----Set----
             //   tutorial.DemoMainMenu = menu;
                tutorial.TutorialName = name;
                tutorial.Caption = caption;
                tutorial.MenuManager = menuManager;
                //------------

                tutorial.Visible = true;
                item.WasShown = true;
            //    menu.SchedulerControl = tutorial.PrintingSchedulerControl;
              //  menu.SchedulerReport = tutorial as IDemoSchedulerReport;

                if (oldTutorial != null)
                {
                    ((TutorialControl)oldTutorial).DemoMainMenu = null;
                    oldTutorial.Visible = false;
                }
            }
            finally
            {
                Cursor.Current = currentCursor;
            }
            RaiseModuleChanged();
        }
    }
}
