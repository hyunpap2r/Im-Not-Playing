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
            timer1.Enabled= true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Video 연결
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

            // Db 연결
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
            
            //Mat -> Bitmap 형변환
            Bitmap mImageBitmap = BitmapConverter.ToBitmap(mImage);
            pictureBox1.Image = mImageBitmap;


            if (faceDetector.CatchCnt == 0 && !stopwatch.IsRunning)
            {
                stopwatch.Start();
                CatchCnt.BackColor = Color.Red;

                UploadCnt++;
                string filename = "NewFace" + UploadCnt.ToString() + ".png";
                string folderPath = @"C:\Users\mw281\OneDrive\바탕 화면\IdentifyFace\steven";
                
                Task.Run(() =>
                {
                    // 비동기 과정에서 원본파일이 아닌 Clone파일을 사용하기 위해 복제
                    Bitmap mImageBitmapCopy = (Bitmap)mImageBitmap.Clone();

                    // 파일 저장
                    mImageBitmapCopy.Save(Path.Combine(folderPath, filename), ImageFormat.Png);
                }).ContinueWith((prevTask) =>
                {

                    var imageBytes = File.ReadAllBytes(Path.Combine(folderPath, filename));
                    
                    // ML모델 사용
                    Identify_Face.ModelInput sampleData = new Identify_Face.ModelInput()
                    {
                        ImageSource = imageBytes,
                    };

                   var sortedScoresWithLabel = Identify_Face.PredictAllLabels(sampleData);
                   
                    // 비교결과중 예상치가 더 높은 결과를 가져오도록 함.
                   var highestScoreKey = sortedScoresWithLabel.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

                    // UI 업데이트는 UI 스레드에서 실행
                    UpdateResultBox(highestScoreKey.ToString());

                });

            }
            else if (faceDetector.CatchCnt == 0 && stopwatch.IsRunning)
            {
                // 얼굴이 감지되지 않는 동안의 시간을 계속 측정
                CatchCnt.BackColor = Color.Red;
            }
            else if (faceDetector.CatchCnt > 0 && stopwatch.IsRunning)
            {
                stopwatch.Stop();

                // 빨간 사각형이 표시되는 동안의 시간만 DB에 삽입
                InsertTimeIntoDatabase(stopwatch.Elapsed, cumulativeTime);

                cumulativeTime += stopwatch.Elapsed;
                stopwatch.Reset();
                CatchCnt.BackColor = Color.Wheat;
            }

            CatchCnt.Text = "No Face Detected Time: " + cumulativeTime.ToString(@"hh\:mm\:ss");

        }


        private void InsertTimeIntoDatabase(TimeSpan onceTime, TimeSpan totalTime)
        {
            // 한 번 동안의 시간
            string onceTimeString = onceTime.ToString(@"hh\:mm\:ss");
            // 총 누적시작
            string totalTimeString = totalTime.ToString(@"hh\:mm\:ss");

            if (onceTimeString == "00:00:00")
            {
                return;
            }

            // Db연결
            ConnectionDb connectionDb = new ConnectionDb();
            string connString = connectionDb.GetConnectionString();


            // Table에 데이터 Insert
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

        // UI 업데이트는 UI 스레드에서 실행
        private void UpdateResultBox(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateResultBox), new object[] { text });
                return;
            }
            ResultBox.Text = text;
        }



    }
}


