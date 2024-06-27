using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using MatBaoInvoice.Event;
using MatBaoInvoice.Global;
using SAPbobsCOM;
using SAPbouiCOM;

namespace MatBaoInvoice
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Globals.SetApplication())
            {
                if (Globals.SapApplication != null)
                    Globals.SapApplication.StatusBar.SetText("Connection Error", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error);
                Environment.Exit(0);
                return;
            }

            B1Events b1s = new B1Events(Globals.SapApplication, Globals.SapCompany);
            Globals.SapApplication.StatusBar.SetText("Connected", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
            System.Windows.Forms.Application.Run();
        }
    }
}
