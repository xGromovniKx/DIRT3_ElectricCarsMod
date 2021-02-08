using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO;
using System.Diagnostics;
using DIRT3_ElectricCarsMod.Properties;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;


namespace DIRT3_ElectricCarsMod
{
   
    public partial class Form1 : Form
    {
        public class CarsKeyValuePair
        {

            public object Key;
            public string Value;

            public CarsKeyValuePair(object NewCarValue, string NewCarDescription)
            {
                Key = NewCarValue;
                Value = NewCarDescription;
            }

            public override string ToString()
            {
                return Value;
            }
        }  
        string strPath = string.Empty;
        string strCarSetupPath = @"audio\carSetup.xml";
        Dictionary<string, string> listOfCars = new Dictionary<string, string>();
        
        
        public Form1()
        {
            this.Icon = Resources.dirt3;
            InitializeComponent();
            strPath = ReadRegistryInstallPath();
            listOfCarsAddAll();
            ReadAllCars();
            comboBoxCars.Sorted = true;
            comboBoxCars.DisplayMember = "Value";
            comboBoxCars.SelectedValue = "Key";
            
        }

        private void listOfCarsAddAll()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            Stream streamCarList = asm.GetManifestResourceStream("DIRT3_ElectricCarsMod.Resources.CarListKeyValue.txt");
            string car; 
            StreamReader file = null;
                try
                {
                    file = new StreamReader(streamCarList);
                    while ((car = file.ReadLine()) != null)
                    {
                        // add to dictionary here 
                        listOfCars.Add(car.Substring(0, 3), car.Substring(3, car.Length - 3));
                    }
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
        }

        private void ReadAllCars()
        {
            //int i = 1;
            comboBoxCars.Items.Clear();
            try
            {
                XDocument xmlFile = XDocument.Load(strPath + strCarSetupPath);
                var query = from c in xmlFile.Elements("CarSetup").Elements("Cars").Elements("Car")
                            select c;
                foreach (XElement Car in query)
                {
                    
                    if(listOfCars.ContainsKey(Car.Attribute("name").Value.ToString()))
                    {
                    string carDescription = string.Empty;
                    var carDesc = from cd in listOfCars where cd.Key == Car.Attribute("name").Value select cd;
                    comboBoxCars.Items.Add(new CarsKeyValuePair(Car.Attribute("name").Value, carDesc.ElementAt(0).Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string ReadRegistryInstallPath()
        {
            String s = string.Empty;
            RegistryKey rk;
            rk = Registry.LocalMachine;
            try
            {
                RegistryKey rkDirt3 = rk.OpenSubKey(@"SOFTWARE\Codemasters\DiRT3");
                s = rkDirt3.GetValue("PATH_APPLICATION").ToString();
            }
            catch
            {
                MessageBox.Show("Are you sure you have DiRT3 installed !!!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return s;
        }

        private void btnOriginal_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(strPath + @"audio\carSetup.xml.original", strPath + @"audio\carSetup.xml", true);
                File.Delete(strPath + @"audio\carSetup.xml.original");
                MessageBox.Show("Success. Original sound of cars restored !", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong ! \nMaybe you already have original sound of cars in place !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnElectricCarMod_Click(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(strPath + @"audio\carSetup.xml.backup"))
                {
                    File.Copy(strPath + @"audio\carSetup.xml", strPath + @"audio\carSetup.xml.backup", false);
                }
                File.Move(strPath + @"audio\carSetup.xml", strPath + @"audio\carSetup.xml.original");
                ChangeValuesToElectricCars();
                MessageBox.Show("Success. Electric cars are ready to go !", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Something went wrong !\nMaybe you already have electric cars in place !", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeValuesToElectricCars()
        {
            try
            {
                XDocument xmlFile = XDocument.Load(strPath + strCarSetupPath + @".original");
                var query = from VT in xmlFile.Elements("CarSetup").Elements("Cars").Elements("Car").Elements("VolumeTrims")
                            select VT;
                foreach (XElement VolumeTrims in query)
                {
                    string voltr = VolumeTrims.ToString();
                    VolumeTrims.Attribute("gear").Value = "-48";
                    VolumeTrims.Attribute("engine").Value = "-48";
                    VolumeTrims.Attribute("exhaust").Value = "-48";
                    // resolves error with CTI car, Codemasters error !?!?!
                    if (!voltr.Contains("WWhineSpeedTrim"))
                        VolumeTrims.Attribute("WhineSpeedTrim").Value = "0.0";
                    else
                        VolumeTrims.Attribute("WWhineSpeedTrim").Value = "0.0";
                    VolumeTrims.Attribute("WhineRPMTrim").Value = "0.0";
                }
                var query2 = from SM in xmlFile.Elements("CarSetup").Elements("Cars").Elements("Car").Elements("SplitMech")
                             select SM;
                foreach (XElement SplitMech in query2)
                {
                    SplitMech.Attribute("DetOnUpShift").Value = "false";
                    SplitMech.Attribute("DetOnLimiter").Value = "false";
                    SplitMech.Attribute("DetVol").Value = "-48";
                }
                xmlFile.Save(strPath + strCarSetupPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void electricCarModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form ecmForm = new ElectricCarModForm();
            ecmForm.ShowInTaskbar = false;
            ecmForm.StartPosition = FormStartPosition.CenterScreen;
            ecmForm.Show();
        }

        private void comboBoxCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            // fill out values for specific cars
            
            //int i = comboBoxCars.SelectedIndex;
            //object s = comboBoxCars.SelectedItem;
            //var ss = comboBoxCars.SelectedValue;
            //var sss = comboBoxCars.SelectedText;
            //var ssss = comboBoxCars.SelectedItem;

        }

        private void comboBoxCars_SelectedValueChanged(object sender, EventArgs e)
        {
           
            try
            {
                CarsKeyValuePair kvs = (CarsKeyValuePair)comboBoxCars.SelectedItem;
                string s = kvs.Key.ToString();
                lblCARS.Text = s;

                XDocument xmlFile = XDocument.Load(strPath + strCarSetupPath);
                var query = from VT in xmlFile.Elements("CarSetup").Elements("Cars").Elements("Car")
                            select VT;
                foreach (XElement Cars in query)
                {
                    if (Cars.Attribute("name").Value.ToString() == s)
                    {
                        txbProba.Text = Cars.ToString();
                        var SplitMech = Cars.Element("SplitMech");
                        var VolumeTrims = Cars.Element("VolumeTrims");
                        var Dump = Cars.Element("Turbo").Element("Dump");
                        var EngineBay = Cars.Element("Offsets").Element("EngineBay");
                        var Exhaust = Cars.Element("Offsets").Element("Exhaust");
                        var GearBox = Cars.Element("Offsets").Element("GearBox");
                        var Cockpit = Cars.Element("Offsets").Element("Cockpit");

                        //txbProba.Text = SplitMech.ToString();
                        //txbProba.Text += VolumeTrims.ToString();

                        nudDetVol.Value = Convert.ToDecimal(SplitMech.Attribute("DetVol").Value);
                        nudGear.Value = Convert.ToDecimal(VolumeTrims.Attribute("gear").Value);
                        nudEngine.Value = Convert.ToDecimal(VolumeTrims.Attribute("engine").Value);
                        int i =  (int)Convert.ToDecimal(VolumeTrims.Attribute("engine").Value);
                        tbEngineVolume.Value = i;
                        nudExhaust.Value = Convert.ToDecimal(VolumeTrims.Attribute("exhaust").Value);
                        nudWhineSpeedTrim.Value = Convert.ToDecimal(VolumeTrims.Attribute("WhineSpeedTrim").Value);
                        nudWhineRPMTrim.Value = Convert.ToDecimal(VolumeTrims.Attribute("WhineRPMTrim").Value);
                        nudDump.Value = Convert.ToDecimal(Dump.Attribute("volume").Value);
                        nudEngineBay.Value = Convert.ToDecimal(EngineBay.Attribute("z").Value);


                        // <SplitMech Whine="mec_gearwhn10" RPM="mec_gearwhn10" Clunk="mec_gearchang09" Exhaust="mec_exhdeton07" DetIndex="7" DetPitch="0.9" DetOnUpShift="false" DetOnLimiter="false" DetVol="-48" StartStall="mec_strtstp_mer" />
                        //  <VolumeTrims gear="-48" engine="-48" exhaust="-48" WhineSpeedTrim="0.0" WhineRPMTrim="0.0" />
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void WriteNewSoundValues()
        {
            try
            {
                XDocument xmlFile = XDocument.Load(strPath + strCarSetupPath);
                var query = from VT in xmlFile.Elements("CarSetup").Elements("Cars").Elements("Car")
                            select VT;
                foreach (XElement Cars in query)
                {
                    
                    if (Cars.Attribute("name").Value.ToString() == lblCARS.Text)
                    {
                        //Cars.ReplaceWith(txbProba.Text);
                        //txbProba.Text = Cars.ToString();
                        var SplitMech = Cars.Element("SplitMech");
                        var VolumeTrims = Cars.Element("VolumeTrims");
                        var Dump = Cars.Element("Turbo").Element("Dump");
                        var EngineBay = Cars.Element("Offsets").Element("EngineBay");
                        var Exhaust = Cars.Element("Offsets").Element("Exhaust");
                        var GearBox = Cars.Element("Offsets").Element("GearBox");
                        var Cockpit = Cars.Element("Offsets").Element("Cockpit");
                        //txbProba.Text = SplitMech.ToString();
                        //txbProba.Text += VolumeTrims.ToString();

                        // change to new values
                        SplitMech.Attribute("DetVol").Value = nudDetVol.Value.ToString();
                        VolumeTrims.Attribute("gear").Value = nudGear.Value.ToString();
                        VolumeTrims.Attribute("engine").Value = nudEngine.Value.ToString();
                        VolumeTrims.Attribute("exhaust").Value = nudExhaust.Value.ToString();
                        // resolves error with CTI car, Codemasters error !?!?!
                        if (VolumeTrims.ToString().Contains("WWhineSpeedTrim"))
                        { VolumeTrims.Attribute("WWhineSpeedTrim").Value = nudWhineSpeedTrim.Value.ToString(); }
                        else
                        {VolumeTrims.Attribute("WhineSpeedTrim").Value = nudWhineSpeedTrim.Value.ToString();}

                        VolumeTrims.Attribute("WhineRPMTrim").Value = nudWhineRPMTrim.Value.ToString();
                        Dump.Attribute("volume").Value = nudDump.Value.ToString();
                        EngineBay.Attribute("z").Value = nudEngineBay.Value.ToString();


                        // <SplitMech Whine="mec_gearwhn10" RPM="mec_gearwhn10" Clunk="mec_gearchang09" Exhaust="mec_exhdeton07" DetIndex="7" DetPitch="0.9" DetOnUpShift="false" DetOnLimiter="false" DetVol="-48" StartStall="mec_strtstp_mer" />
                        //  <VolumeTrims gear="-48" engine="-48" exhaust="-48" WhineSpeedTrim="0.0" WhineRPMTrim="0.0" />
                    }
                }
                xmlFile.Save(strPath + strCarSetupPath);
            }
             catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                WriteNewSoundValues();
                MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           
        

    }
}
