using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RecognitionLib.ANN;
using MathNet.Numerics.LinearAlgebra.Single;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace RecognitionLib
{
    public class Handwriting
    {
        private List<StringInputData> trainImages;
        private ArrayInputData trainData;
        private MultilayerNetwork mln;
        private int patternSize = 10000;

        public Handwriting()
        {
            mln = new MultilayerNetwork(10, patternSize, 26);
        }

        public void loadTrainImages(List<StringInputData> p)
        {
            trainImages = p;
            trainData = new ArrayInputData(new DenseMatrix(patternSize, p.Count), new DenseMatrix(26, p.Count));
            Matrix<float> inputPattern;
            Bitmap inputBmp;
            for (int img = 0; img < p.Count; img++)
            {
                inputBmp = new Bitmap(p[img].Key, false);
                inputPattern = DataManipulation.Bmp2Pattern(inputBmp);
            }
        }

        public void clearTrainImages()
        {
            trainImages = new List<StringInputData>();
        }

        public void train()
        {
            mln.backpropTraining(null, 100);
        }

        public void reset()
        {
            mln.randomize();
        }

        public string recognise(string p)
        {
            Bitmap inputBmp = new Bitmap(p, false);
            Matrix<float> inputPattern = DataManipulation.Bmp2Pattern(inputBmp);

            ArrayInputData aid = new ArrayInputData(inputPattern, null);
            Matrix<float> result = mln.use(aid);

            return Mat2String(result);
        }

        private string Mat2String(Matrix<float> result)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.RowCount; i++)
                if (result[i, 0] > 0.1)
                    sb.Append((char)(i + 'A'));
            return sb.ToString();
        }
    }
}
