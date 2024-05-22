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
using System.Drawing.Imaging;


namespace Im_Not_Playing
{
    public partial class Form1 : Form
    {
        OpenCvSharp.VideoCapture video;
        Mat mImage = new Mat();

        int UploadCnt = 0;

        public Form1()
        {
            InitializeComponent();
            timer1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Video ����
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

            // Db ����
            ConnectionDb connection = new ConnectionDb();
            connection.ConnectionTest();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            mImage.Dispose();
        }

        private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        private TimeSpan cumulativeTime = TimeSpan.Zero;

        // ���� CPU Core ����, ML, I/O �� Clone���� ���� core ��� / 2�� ����
        private SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);

        private bool DetectorFlag = true;

        private void timer1_Tick(object sender, EventArgs e)
        {


            video.Read(mImage);

            var faceDetector = new FaceDetector();
            faceDetector.DetectAndDraw(mImage);

            //Mat -> Bitmap ����ȯ
            Bitmap mImageBitmap = BitmapConverter.ToBitmap(mImage);
            pictureBox1.Image = mImageBitmap;

            if(DetectorFlag)
            {
                if (faceDetector.CatchCnt != 0)
                    {
                    // �񵿱� �۾� ���� ����
                    if (semaphore.CurrentCount > 2)
                    {
                        Task.Run(async () =>
                        {
                            await semaphore.WaitAsync();

                            try
                            {
                                stopwatch.Start();
                                CatchTime.BackColor = Color.Blue;

                                UploadCnt++;
                                string filename = "NewFace" + UploadCnt.ToString() + ".png";
                                string folderPath = @"C:\Users\mw281\OneDrive\���� ȭ��\IdentifyFace\steven";

                                Bitmap mImageBitmapCopy = (Bitmap)mImageBitmap.Clone();

                                // ���� ����
                                mImageBitmapCopy.Save(Path.Combine(folderPath, filename), ImageFormat.Png);

                                var imageBytes = File.ReadAllBytes(Path.Combine(folderPath, filename));

                                // ML�� ���
                                Identify_Face.ModelInput sampleData = new Identify_Face.ModelInput()
                                {
                                    ImageSource = imageBytes,
                                };

                                var sortedScoresWithLabel = Identify_Face.PredictAllLabels(sampleData);

                                var highestScoreKey = sortedScoresWithLabel.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                                // UI ������Ʈ�� UI �����忡�� ����
                                UpdateResultBox(highestScoreKey.ToString());

                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });
                    }
                }

                else if (faceDetector.CatchCnt == 0 && stopwatch.IsRunning)
                {
                    CatchTime.BackColor = Color.Red;
                    stopwatch.Stop();

                    cumulativeTime += stopwatch.Elapsed;
                    CatchTime.Text = "Working Time: " + cumulativeTime.ToString(@"hh\:mm\:ss");

                    stopwatch.Reset();
                    CatchTime.BackColor = Color.Wheat;

                }


            }

        }



        private void InsertTimeIntoDatabase(TimeSpan onceTime, TimeSpan totalTime)
        {
            // �� �� ������ �ð�
            string onceTimeString = onceTime.ToString(@"hh\:mm\:ss");
            // �� ��������
            string totalTimeString = totalTime.ToString(@"hh\:mm\:ss");

            if (onceTimeString == "00:00:00")
            {
                return;
            }

            // Db����
            ConnectionDb connectionDb = new ConnectionDb();
            string connString = connectionDb.GetConnectionString();


            // Table�� ������ Insert
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

        // UI ������Ʈ�� UI �����忡�� ����
        private void UpdateResultBox(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateResultBox), new object[] { text });
                return;
            }
            ResultBox.Text = text;
        }

        //�Ͻ����� �� ����
        private void BTN_P_S_Click(object sender, EventArgs e)
        {
            if(DetectorFlag)
            {
                DetectorFlag = false;
            }
            else
            {
                DetectorFlag = true;

            }
        }

        //���� �� DB ����
        private void button2_Click(object sender, EventArgs e)
        {
            InsertTimeIntoDatabase(stopwatch.Elapsed, cumulativeTime);
            DetectorFlag = false;

        }
    }
}


