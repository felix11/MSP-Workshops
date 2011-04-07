using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra.Generic;
using System.Drawing;

namespace RecognitionLib
{
    class DataManipulation
    {
        private static MathNet.Numerics.Random.SystemCryptoRandomNumberGenerator rGen = new MathNet.Numerics.Random.SystemCryptoRandomNumberGenerator();

        /// <summary>
        /// Converts a given bitmap into an input pattern of the network.
        /// </summary>
        /// <param name="inputBmp"></param>
        /// <returns>a Row*Col X 1 matrix containing the bitmap pixel</returns>
        public static Matrix<float> Bmp2Pattern(System.Drawing.Bitmap inputBmp)
        {
            Matrix<float> result = new DenseMatrix(inputBmp.Width * inputBmp.Height, 1);
            for(int row = 0; row < inputBmp.Height; row++)
                for (int col = 0; col < inputBmp.Width; col++)
                {
                    result[(row + 1) * col, 0] = (Convert.ToSingle(inputBmp.GetPixel(row, col).R) + Convert.ToSingle(inputBmp.GetPixel(row, col).G) + Convert.ToSingle(inputBmp.GetPixel(row, col).B)) / (3.0f * 255.0f);
                }

            return result;
        }

        /// <summary>
        /// creates a random matrix with given size.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns>a matrix with uniform random elements from 0 .. 1</returns>
        internal static Matrix<float> rand(int rows, int cols)
        {
            double[] rvals = rGen.NextDouble(rows*cols);
            float[] frvals = new float[rvals.Length];
            for(int i=0; i<rvals.Length; i++)
                frvals[i] = (float)rvals[i];

            return new DenseMatrix(rows, cols, frvals);
        }
    }
}
