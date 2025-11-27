using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using LemonUI;
using LemonUI.Menus;

namespace PilotV //Your Mod Name Here
{
    public class Main : Script
    {
        ObjectPool pool = new ObjectPool();
        NativeMenu mainMenu = new NativeMenu("Pilot Mod", "Main Menu");
        public Main()
        {
            pool.Add(mainMenu);

            GTA.UI.Notification.Show("Pilot mod by Fady"); //Notification on mod load
            Tick += onTick;
            KeyDown += onKeyDown;
        }
        private void onTick(object sender, EventArgs e)
        {


            pool.Process();
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}