using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Linemod;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Npgsql;


namespace Im_Not_Playing
{
    public partial class Form1 : Form
    {
        OpenCvSharp.VideoCapture video;
        Mat mImage = new Mat();

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled= true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // video connection
            try
            {
                video = new VideoCapture(0);
                video.FrameHeight = 640;
                video.FrameWidth = 480;
            }
            catch (Exception)
            {
                timer1.Enabled = false;
            }

            // Db connection
            ConnectionDb connection = new ConnectionDb();
            connection.ConnectionTest();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mImage.Dispose();
        }

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        private TimeSpan cumulativeTime = TimeSpan.Zero;

        private void timer1_Tick(object sender, EventArgs e)
        {
            video.Read(mImage);

            var faceDetector = new FaceDetector();
            faceDetector.DetectAndDraw(mImage);
            pictureBox1.Image = BitmapConverter.ToBitmap(mImage);

            if (faceDetector.CatchCnt == 0 && !stopwatch.IsRunning)
            {
                stopwatch.Start();
                CatchCnt.BackColor = Color.Red;
            }
            else if (faceDetector.CatchCnt == 0 && stopwatch.IsRunning)
            {
                // 얼굴이 감지되지 않는 동안의 시간을 계속 측정합니다.
                CatchCnt.BackColor = Color.Red;
            }
            else if (faceDetector.CatchCnt > 0 && stopwatch.IsRunning)
            {
                stopwatch.Stop();

                // 빨간 사각형이 표시되는 동안의 시간만 DB에 삽입합니다.
                InsertTimeIntoDatabase(stopwatch.Elapsed, cumulativeTime);

                cumulativeTime += stopwatch.Elapsed;
                stopwatch.Reset();
                CatchCnt.BackColor = Color.Wheat;
            }

            CatchCnt.Text = "No Face Detected Time: " + cumulativeTime.ToString(@"hh\:mm\:ss");

        }


        private void InsertTimeIntoDatabase(TimeSpan onceTime, TimeSpan totalTime)
        {
            string onceTimeString = onceTime.ToString(@"hh\:mm\:ss");
            string totalTimeString = totalTime.ToString(@"hh\:mm\:ss");

            if (onceTimeString == "00:00:00")
            {
                return;
            }

            ConnectionDb connectionDb = new ConnectionDb();
            string connString = connectionDb.GetConnectionString();


            using (var connection = new NpgsqlConnection(connString))
            {
                string query = "INSERT INTO Playing_time (Once_Time, Total_Time) VALUES (@OnceTime, @TotalTime);";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OnceTime", onceTimeString);
                    command.Parameters.AddWithValue("@TotalTime", totalTimeString);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}
