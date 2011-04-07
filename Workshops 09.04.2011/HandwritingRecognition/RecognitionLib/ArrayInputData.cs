using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace RecognitionLib
{
    public class ArrayInputData
    {
        private Matrix<float> key;

        public Matrix<float> Key
        {
            get { return key; }
            set { key = value; }
        }
        private Matrix<float> value;

        public Matrix<float> Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public ArrayInputData(Matrix<float> key, Matrix<float> value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
