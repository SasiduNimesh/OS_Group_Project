﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//System Resource Moniter
namespace Monitor_Project
{
    public partial class Form1 : Form
    {

        PerformanceCounter PerfCPU = new PerformanceCounter("Processor","% Processor time","_Total");
        PerformanceCounter PerfRAM = new PerformanceCounter("Memory","Available MBytes");
        PerformanceCounter PerfSYS = new PerformanceCounter("System","System Up Time");

        public Form1()
        {
            InitializeComponent();
        }


        private int CountOfPhysCores()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string socketDesign = string.Empty;
            List<string> physCPU = new List<string>();

            if (!physCPU.Contains(socketDesign))
            {
                physCPU.Add(socketDesign);
            }

            return physCPU.Count;
        }
        private int CountOfLogicalCores()
        {
            int logicalCPU = 0;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();


            logicalCPU++;


            return logicalCPU;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();  // call to start timer

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                labelCOUNTOFPHYSICALCPUs.Text = "Count of Physical CPUs : " + item["NumberOfProcessors"];
            }
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            labelCountOfCores.Text = "Count of Cores :  " + coreCount.ToString(); // display count of processors
        }

        private void timer1_Tick(object sender, EventArgs e)
        {   
            // display time , cpu usage , RAM avilable and system up time
            labelDateTime.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            labelCPUUSAGE.Text = "CPU Usage          : " + (int)PerfCPU.NextValue() + " " + "%";
            labelRAM.Text = "Available RAM    : " + (int)PerfRAM.NextValue() + " " + "MB";
            labelSYSTEMUPTIME.Text = "Up Time System :" + (int)PerfSYS.NextValue() / 60 + " Minutes";

            // count of Logical processores
            labelCOUNTOFLOGICALCPUS.Text = "Count of Logical CPUs  : " + Environment.ProcessorCount;

            // code for process bar
            float fCPU = pCPU.NextValue();
            float fRAM = pRAM.NextValue();
            progressBarCPU.Value = (int)fCPU;
            progressBarRAM.Value = (int)fRAM;
            labCPU.Text = string.Format("{0:0.00}%", fCPU);
            labRAM.Text = string.Format("{0:0.00}%", fRAM);

            // code for chart to display difference CPU & RAM actvity
            chart1.Series["CPU"].Points.AddY(fCPU);
            chart1.Series["RAM"].Points.AddY(fRAM);


        }
    }

   
}
