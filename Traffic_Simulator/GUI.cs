﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Traffic_Simulator
{
    public partial class GUI : Form
    {
        public delegate void drawCarHandler(Car c);
        public event drawCarHandler drawCarEvent;

        /// <summary>
        /// Controller element of the application.
        /// </summary>
        private SimulationController _controller = new SimulationController();
        private PictureBox p;

        private void drawCar(Car c)
        {
            Point position = new Point(6 + pictureBox1.Location.X, 6 + pictureBox1.Location.Y);
            p = new PictureBox();
            p.Image = new Bitmap(@"D:\Documents\GitHub\Pro-CSharp\Bitmap1.bmp");
            if(c.Crossing.GetType() == typeof(Crossing_2))
                position = new Point(position.X + 3*66 , position.Y);
            
            switch(c.Street.Position)
                {
                    case Direction.North:
                        position = new Point(position.X + 66 + 22 * c.StreetIndex[0], position.Y + 22 * c.StreetIndex[1]);                         
                            break; 
                    case Direction.West:
                            position = new Point(position.X + 22 * c.StreetIndex[1], position.Y + 66 * 2 - 22 * c.StreetIndex[0]);                         
                            break;   
                    case Direction.South:
                            position = new Point(position.X + 66 * 2 - 22 * c.StreetIndex[0], position.Y + 66 * 3 - 22 * c.StreetIndex[1]);                         
                            break;
                    case Direction.East:
                            position = new Point(position.X + 66 * 3 - 22 * c.StreetIndex[1], position.Y + 66 + 22 * c.StreetIndex[0]);                         
                            break;
                    case Direction.Center:
                            position = new Point(position.X + 66 + 22 * c.StreetIndex[0], position.Y + 66 + 22 * c.StreetIndex[1]);                         
                            break;
            }
           // position.Y -= 44;
            p.Location = position;
                p.Size = new System.Drawing.Size(10, 10);
                            p.Show();
                            this.Controls.Add(p);
                            p.BringToFront();          
        }


        /// <summary>
        /// Starts browse file dialog (save or open)
        /// </summary>
        /// <param name="n"> 0 = "open a file" --- 1 = "save a file"</param>
        /// <returns>Path browsed if user clicked "save"</returns>
        /// <returns>Empty string (null) if user clicked "cancel"</returns>
        public string filePath(int n)
        {

            if (n == 0)//shows open file dialog
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {//if user clicks "ok" in the file dialog
                    return openFileDialog1.FileName;
                }
            }

            if (n == 1)//shows save file dialog
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {//if user clicks "ok" in the file dialog
                    return saveFileDialog1.FileName;
                }
            }

            return "";
        }

        /// <summary>
        /// Method called by controller when user closes program and there are unsaved modifications.
        /// </summary>
        /// <param name="title">Title of the message box</param>
        /// <param name="message">Message shown in the message box</param>
        /// <returns>True if user clics OK; False if user clicks CANCEL</returns>
        public string saveMessage(string title, string message)
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel).ToString();//if users clicks "ok"
           
        }

        public void refreshScreen(Grid copyOfGrid)
        {
            if (copyOfGrid == null)
                return;
            foreach (Car c in copyOfGrid.ListOfCars) //moves every existing car by 1 position
                if (c!=null)
                {
                    //drawCar(c);
                    Invoke(drawCarEvent, new object[]{c});
                }
            Invalidate();
        }



        public GUI() 
        { 
            InitializeComponent();
            drawCarEvent += drawCar;
            _controller.Gui = this;
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            FormClosing += closeToolStripMenuItem_Click; //subscribe close-button method to close "X"

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.saveAs();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = _controller.load();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(ToolStripMenuItem))//if this method was called by the menu bar
            {
                this.Close();
            }
            else 
            { 
                label1.Text = _controller.close();
                if (label1.Text == "") 
                {
                    ((CancelEventArgs)e).Cancel = true;   
                }
            }
        }

             
        private void button1_Click(object sender, EventArgs e)// start/pause button click method
        {
            
            if (_controller.State != State.Running) //if simulation is not running
            {
                label1.Text = _controller.startSimulation();

                if (label1.Text == "")
                {
                    button1.Text = "Pause";
                    button2.Enabled = true;
                }                
                return;     //leave method
            }
            

            if(_controller.State == State.Running) //if simulation is running
            {
                label1.Text = _controller.pauseSimulation();
                if (label1.Text == "")
                {
                    button1.Text = "Start";
                    button2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)// stop button click method
        {
            label1.Text = _controller.stopSimulation();
            if (label1.Text == "") {
                button1.Text = "Start";
                button2.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e) // make change button method
        {
            _controller.setCrossingProperty(null, null); //just a simulation of having changed data
        }

        
        
        
    }
}
