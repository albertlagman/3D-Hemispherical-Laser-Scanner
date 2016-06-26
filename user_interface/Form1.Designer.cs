namespace user_interface
{
    partial class Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            this.connectButton = new System.Windows.Forms.Button();
            this.comboSerialPort = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.zAxisTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.x90degRadioButton = new System.Windows.Forms.RadioButton();
            this.x45degRadioButton = new System.Windows.Forms.RadioButton();
            this.x0degRadioButton = new System.Windows.Forms.RadioButton();
            this.loadButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.scan360degButton = new System.Windows.Forms.Button();
            this.scanHemiButton = new System.Windows.Forms.Button();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.ibOriginal = new Emgu.CV.UI.ImageBox();
            this.ibFiltered = new Emgu.CV.UI.ImageBox();
            this.btnFrameAdvance = new System.Windows.Forms.Button();
            this.receivedXtextBox = new System.Windows.Forms.TextBox();
            this.receivedZtextBox = new System.Windows.Forms.TextBox();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ibOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibFiltered)).BeginInit();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.Name = "connectButton";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // comboSerialPort
            // 
            this.comboSerialPort.FormattingEnabled = true;
            resources.ApplyResources(this.comboSerialPort, "comboSerialPort");
            this.comboSerialPort.Name = "comboSerialPort";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // zAxisTextBox
            // 
            resources.ApplyResources(this.zAxisTextBox, "zAxisTextBox");
            this.zAxisTextBox.Name = "zAxisTextBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.x90degRadioButton);
            this.panel1.Controls.Add(this.x45degRadioButton);
            this.panel1.Controls.Add(this.x0degRadioButton);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // x90degRadioButton
            // 
            resources.ApplyResources(this.x90degRadioButton, "x90degRadioButton");
            this.x90degRadioButton.Name = "x90degRadioButton";
            this.x90degRadioButton.TabStop = true;
            this.x90degRadioButton.UseVisualStyleBackColor = true;
            // 
            // x45degRadioButton
            // 
            resources.ApplyResources(this.x45degRadioButton, "x45degRadioButton");
            this.x45degRadioButton.Name = "x45degRadioButton";
            this.x45degRadioButton.TabStop = true;
            this.x45degRadioButton.UseVisualStyleBackColor = true;
            // 
            // x0degRadioButton
            // 
            resources.ApplyResources(this.x0degRadioButton, "x0degRadioButton");
            this.x0degRadioButton.Name = "x0degRadioButton";
            this.x0degRadioButton.TabStop = true;
            this.x0degRadioButton.UseVisualStyleBackColor = true;
            // 
            // loadButton
            // 
            resources.ApplyResources(this.loadButton, "loadButton");
            this.loadButton.Name = "loadButton";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // scan360degButton
            // 
            resources.ApplyResources(this.scan360degButton, "scan360degButton");
            this.scan360degButton.Name = "scan360degButton";
            this.scan360degButton.UseVisualStyleBackColor = true;
            this.scan360degButton.Click += new System.EventHandler(this.scan360degButton_Click);
            // 
            // scanHemiButton
            // 
            resources.ApplyResources(this.scanHemiButton, "scanHemiButton");
            this.scanHemiButton.Name = "scanHemiButton";
            this.scanHemiButton.UseVisualStyleBackColor = true;
            this.scanHemiButton.Click += new System.EventHandler(this.scanHemiButton_Click);
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.statusBox, "statusBox");
            this.statusBox.Name = "statusBox";
            this.statusBox.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // stopButton
            // 
            resources.ApplyResources(this.stopButton, "stopButton");
            this.stopButton.Name = "stopButton";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // ibOriginal
            // 
            resources.ApplyResources(this.ibOriginal, "ibOriginal");
            this.ibOriginal.Name = "ibOriginal";
            this.ibOriginal.TabStop = false;
            // 
            // ibFiltered
            // 
            resources.ApplyResources(this.ibFiltered, "ibFiltered");
            this.ibFiltered.Name = "ibFiltered";
            this.ibFiltered.TabStop = false;
            // 
            // btnFrameAdvance
            // 
            resources.ApplyResources(this.btnFrameAdvance, "btnFrameAdvance");
            this.btnFrameAdvance.Name = "btnFrameAdvance";
            this.btnFrameAdvance.UseVisualStyleBackColor = true;
            this.btnFrameAdvance.Click += new System.EventHandler(this.btnFrameAdvance_Click);
            // 
            // receivedXtextBox
            // 
            resources.ApplyResources(this.receivedXtextBox, "receivedXtextBox");
            this.receivedXtextBox.Name = "receivedXtextBox";
            this.receivedXtextBox.ReadOnly = true;
            // 
            // receivedZtextBox
            // 
            resources.ApplyResources(this.receivedZtextBox, "receivedZtextBox");
            this.receivedZtextBox.Name = "receivedZtextBox";
            this.receivedZtextBox.ReadOnly = true;
            // 
            // debugTextBox
            // 
            resources.ApplyResources(this.debugTextBox, "debugTextBox");
            this.debugTextBox.Name = "debugTextBox";
            // 
            // Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.receivedZtextBox);
            this.Controls.Add(this.receivedXtextBox);
            this.Controls.Add(this.btnFrameAdvance);
            this.Controls.Add(this.ibFiltered);
            this.Controls.Add(this.ibOriginal);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.scanHemiButton);
            this.Controls.Add(this.scan360degButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zAxisTextBox);
            this.Controls.Add(this.comboSerialPort);
            this.Controls.Add(this.connectButton);
            this.Name = "Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ibOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ibFiltered)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ComboBox comboSerialPort;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox zAxisTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton x90degRadioButton;
        private System.Windows.Forms.RadioButton x45degRadioButton;
        private System.Windows.Forms.RadioButton x0degRadioButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button scan360degButton;
        private System.Windows.Forms.Button scanHemiButton;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button stopButton;
        private Emgu.CV.UI.ImageBox ibOriginal;
        private Emgu.CV.UI.ImageBox ibFiltered;
        private System.Windows.Forms.Button btnFrameAdvance;
        private System.Windows.Forms.TextBox receivedXtextBox;
        private System.Windows.Forms.TextBox receivedZtextBox;
        private System.Windows.Forms.TextBox debugTextBox;
    }
}

