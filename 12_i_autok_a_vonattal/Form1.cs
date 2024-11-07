using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _12_i_autok_a_vonattal
{
    public partial class Form1 : Form
    {
        Timer trainTimer = new Timer();
        Timer carTimer = new Timer();
        Timer trainMover = new Timer();
        List<PictureBox> cars = new List<PictureBox>();
        PictureBox train = new PictureBox();
        PictureBox stopButton = new PictureBox();
        Random r = new Random();
        bool stop = false;
        public Form1()
        {
            InitializeComponent();
            Start();
        }
        void Start()
        {
            StartTimers();
            train.Width = 60;
            train.Height = 200;
            train.BackColor = Color.Brown;
            train.Left = (this.ClientSize.Width - train.Width) / 2;
            train.Top = this.ClientSize.Height;
            this.Controls.Add(train);

            stopButton.Height = 100;
            stopButton.Width = 10;
            stopButton.BackColor = Color.Brown;
            stopButton.Left = (this.ClientSize.Width - train.Width * 2) / 2;
            stopButton.Top = (int)(this.ClientSize.Height - stopButton.Height * 2.5 - 20) / 2;
            this.Controls.Add(stopButton);
        }
        void StartTimers()
        {
            carTimer.Interval = 25;
            randomTrainInterval();
            trainMover.Interval = 10;

            carTimer.Tick += CarMove;
            trainTimer.Tick += TrainComes;
            trainMover.Tick += TrainMove;
            stopButton.Click += CarStop;

            carTimer.Start();
            trainTimer.Start();
        }
        void CarMove(object s, EventArgs e)
        {
            
            if (cars.Count == 0)
                createCar();
            if(!stop || (stop && cars[0].Right > stopButton.Left))
                cars[0].Left += 2;
            if (cars.Last().Left == 0)
                createCar();
            if (cars.Count == 1)
                return;
            for (int i = 1; i < cars.Count; i++)
                if (cars[i - 1].Left - 20 > cars[i].Right &&
                    (!stop || (stop && cars[i].Right > stopButton.Left)))
                    cars[i].Left += 2;
            for (int i = 0; i < cars.Count; i++)
                if (cars[i].Left > this.ClientSize.Width)
                {
                    this.Controls.Remove(cars[i]);
                    cars.Remove(cars[i]);
                }

        }
        void CarStop(object s, EventArgs e)
        {
            stop = !stop;
        }

        void createCar()
        {
            PictureBox oneCar = new PictureBox();
            oneCar.Width = 100;
            oneCar.Height = 50;
            oneCar.BackColor = Color.Blue;
            this.Controls.Add(oneCar);
            oneCar.Top = (this.ClientSize.Height - oneCar.Height) / 2;
            oneCar.Left = -oneCar.Width;
            cars.Add(oneCar);
        }
        void TrainComes(object s, EventArgs e)
        {
            train.Top = this.ClientSize.Height;
            trainMover.Start();
            randomTrainInterval();
        }
        void randomTrainInterval() {
            int interval = r.Next(10000, 30000);
            trainTimer.Interval = interval;
        }
        void TrainMove(object s, EventArgs e)
        {
            train.Top -= 4;
            if (train.Bottom < 0)
                trainMover.Stop();
            foreach (PictureBox car in cars)
                if (car.Bounds.IntersectsWith(train.Bounds))
                {
                    carTimer.Stop();
                    trainMover.Stop();
                    trainTimer.Stop();
                    MessageBox.Show("Meghaltál");
                    Application.Exit();
                }
        }
    }
}
