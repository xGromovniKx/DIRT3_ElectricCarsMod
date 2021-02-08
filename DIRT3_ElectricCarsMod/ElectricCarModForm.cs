using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using DIRT3_ElectricCarsMod.Properties;

namespace DIRT3_ElectricCarsMod
{
    public partial class ElectricCarModForm : Form
    {
        public ElectricCarModForm()
        {
            this.Icon = Resources.dirt3;
            InitializeComponent();
            // checking for resource names
            //Assembly myAssembly = Assembly.GetExecutingAssembly();
            //string[] names = myAssembly.GetManifestResourceNames();
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("DIRT3_ElectricCarsMod.Resources.DiRT3ElectricCarMod.rtf");
            if (stream != null)
            {
                StreamReader sr = new StreamReader(stream);
                rtbECM.LoadFile(stream, RichTextBoxStreamType.RichText);
                sr.Close();
            }
        }

        
               
    }
}
