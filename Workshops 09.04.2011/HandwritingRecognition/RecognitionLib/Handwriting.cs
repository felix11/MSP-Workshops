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
        private List<StringInputData> trainImages = new List<StringInputData>();
        private ArrayInputData trainData = null;
        private MultilayerNetwork mln;
        private int patternSize = 10000;

        /// <summary>
        /// A handwriting recognition object.
        /// </summary>
        /// <param name="patternSize">The size of one input pattern.</param>
        public Handwriting(int patternSize)
        {
            this.patternSize = patternSize;
            mln = new MultilayerNetwork(10, patternSize, 26);
        }

        /// <summary>
        /// Loads images into an input pattern structure.
        /// </summary>
        /// <param name="p">list of StringInputData, containing the image path as Key and the represented character as Value.</param>
        public void loadTrainImages(List<StringInputData> p)
        {
            trainImages = p;
            trainData = new ArrayInputData(new DenseMatrix(patternSize, p.Count), new DenseMatrix(26, p.Count, 0));
            Matrix<float> inputPattern;
            for (int img = 0; img < p.Count; img++)
            {
                inputPattern = DataManipulation.Bmp2Pattern(p[img].Key);
                trainData.Key.SetColumn(img, inputPattern.Column(0));
                // Careful: only upper letters are covered so far!!
                trainData.Value.At(p[img].Value.ToUpper()[0] - 'A', img, 1.0f);
            }
        }

        /// <summary>
        /// Clears the training data.
        /// </summary>
        public void clearTrainImages()
        {
            trainImages = new List<StringInputData>();
            trainData = null;
        }

        /// <summary>
        /// Trains the network using backprop.
        /// </summary>
        public void train()
        {
            if (trainData == null)
                throw new ArgumentException("No training data available!");

            // use backprop
            
        }

        /// <summary>
        /// Resets the network to random values.
        /// </summary>
        public void reset()
        {
            mln.randomize();
        }

        /// <summary>
        /// Recognizes the character in the file of the given path.
        /// </summary>
        /// <param name="p">Path to a file with a character image.</param>
        /// <returns>The character in this image.</returns>
        public string recognise(string p)
        {
            return "";
        }

        /// <summary>
        /// Converts a target value matrix into a string.
        /// </summary>
        /// <param name="mat">The target value matrix of a network.</param>
        /// <returns>the associated character.</returns>
        private string Mat2String(Matrix<float> mat)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mat.RowCount; i++)
                if (mat[i, 0] > 0.6)
                    sb.Append((char)(i + 'A'));
            return sb.ToString();
        }
    }
}
