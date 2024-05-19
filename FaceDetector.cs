using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using OpenCvSharp;
using OpenCvSharp.Extensions;



namespace Im_Not_Playing
{
    internal class FaceDetector
    {
        CascadeClassifier faceCascade;

        public FaceDetector()
        {
            faceCascade = new CascadeClassifier("haarcascade_frontalface_alt2.xml");
        }

        public int CatchCnt { get; private set; }

        public void DetectAndDraw(Mat image)
        {
            var faces = faceCascade.DetectMultiScale(image, 1.1, 4, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));
            //var faces = faceCascade.DetectMultiScale(image, 1.1, 4, HaarDetectionType.ScaleImage, new OpenCvSharp.Size(30, 30));

            CatchCnt = faces.Length; 

            foreach (var rect in faces)
            {
                Cv2.Rectangle(image, rect, Scalar.Red);
            }
        }




    }
}
