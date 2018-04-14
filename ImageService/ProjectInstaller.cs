using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace ImageService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
           // string parameter = "MySource1\" \"MyLogFile1";
            string parameter = ConfigurationManager.AppSettings.Get("SourceName");
            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\"";
            base.OnBeforeInstall(savedState);
       }
        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
