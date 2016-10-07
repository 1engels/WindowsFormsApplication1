using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private const string userRoot = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Power Monitoring Expert\\8.1";
        private const string userRoot2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Schneider Electric\\Common";
        private const string keyname = userRoot;
        private const string keyname1 = userRoot + "\\" + "Databases";
        private const string keyname2 = userRoot2 + "\\" + "License";

        public Form1()
        {
            InitializeComponent();
            string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "CannotRead");
            
        }

        private void buttonChan(object sender, EventArgs e)
        {
            if(closeprocess())
            {
                if (!read())
                    writeToLog("[Error] No se pudo leer algun registro");
                
                //write();
            }
            else
            {
                writeToLog("[Error]  Cierre las aplicaciones PME.");
            }
            
        }

        public void writeToLog(string s)
        {
            textBox1.Text += DateTime.Now.ToString(new CultureInfo("es-ES")) + " " + s + "\r\n";
        }

        public Boolean closeprocess()
        {    
            foreach (Process proceso in Process.GetProcesses())
            {
                if(proceso.ProcessName=="vista" || proceso.ProcessName=="desginer" || proceso.ProcessName=="repgen" || proceso.ProcessName=="reportgen" || proceso.ProcessName=="ManagementConsole")
                {
                    return false;
                }
            }
            return true;
        }

        public bool read()
        {
            bool success = false;
            try
            {
                string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "No se puede leer");
                writeToLog("[Info] NetmanMachine: " + NetmanMachine);

                string PrimaryMachine = (string)Registry.GetValue(keyname, "PrimaryMachine", "No se puede leer");
                writeToLog("[Info] PrimaryMachine: " + PrimaryMachine);

                string Root2 = (string)Registry.GetValue(keyname, "Root2", "No se puede leer");
                writeToLog("[Info] Root2: " + Root2);

                string IONServer = (string)Registry.GetValue(keyname1, "IONServer", "No se puede leer");
                writeToLog("[Info] IONServer: " + IONServer);

                string NOMServer = (string)Registry.GetValue(keyname1, "NOMServer", "No se puede leer");
                writeToLog("[Info] NOMServer: " + NOMServer);

                string SYSLOGServer = (string)Registry.GetValue(keyname1, "SYSLOGServer", "No se puede leer");
                writeToLog("[Info] SYSLOGServer: " + SYSLOGServer);

                string LicenseServers = (string)Registry.GetValue(keyname2, "LicenseServers", "No se puede leer");
                writeToLog("[Info] LicenseServers: " + LicenseServers);

                success = true;
    
            }
            catch(Exception ex)
            {
                writeToLog("[Exception] " + ex.ToString());
            }
            if (success)
                return true;
            else
                return false;

        }

        private void write()
        {
  
            string serv = textBoxServ1.Text;

            string NetmanMachine = (string)Registry.GetValue(keyname, "NetmanMachine", "CannotRead");
            if(NetmanMachine!="CannotRead")
            {
                Registry.SetValue(keyname, "NetmanMachine", serv, RegistryValueKind.String);

                Registry.SetValue(keyname, "PrimaryMachine", serv, RegistryValueKind.String);

                Registry.SetValue(keyname, "Root2", "\\\\" + serv + "\\ION-Ent\\config", RegistryValueKind.String);

                //DATABASES

                Registry.SetValue(keyname1, "IONServer", serv + "\\ION", RegistryValueKind.String);

                Registry.SetValue(keyname1, "NOMServer", serv + "\\ION", RegistryValueKind.String);

                Registry.SetValue(keyname1, "SYSLOGServer", serv + "\\ION", RegistryValueKind.String);

                //LICENSE

                Registry.SetValue(keyname2, "LicenseServers", "27000@" + serv, RegistryValueKind.String);
            }
            else
            {
                writeToLog("[Error] No se pudo encontrar el nombre del servidor actual");
            }
            

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollBars = ScrollBars.Both;
        }
            
    }
}
