using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra.Generic;
using RecognitionLib;

namespace RecognitionLib.ANN
{
    class MultilayerNetwork
    {
        public int HiddenNodes
        {
            get { return weights1.RowCount; }
        }
        public int PatternSize
        {
            get { return weights1.ColumnCount - 1; }
        }
        public int TargetSize
        {
            get { return weights2.RowCount; }
        }

        private Matrix<float> weights1;
        private Matrix<float> weights2;
        private Matrix<float> net_out;
        private Matrix<float> net_hout;
        private Matrix<float> net_deltao;
        private Matrix<float> net_deltah;

        private MathNet.Numerics.Distributions.IContinuousDistribution dist = new MathNet.Numerics.Distributions.ContinuousUniform();

        public MultilayerNetwork(int hiddenNodes, int patternsize, int targetsize)
        {
            this.weights1 = new DenseMatrix(hiddenNodes, patternsize + 1);
            this.weights2 = new DenseMatrix(targetsize, hiddenNodes + 1);

            randomize();
        }

        internal void backpropTraining(ArrayInputData traindata, int epochs)
        {
            // training pattern
            Matrix<float> pat = new DenseMatrix(traindata.Key.RowCount + 1, traindata.Key.ColumnCount, 1.0f);
            pat.SetSubMatrix(0, traindata.Key.RowCount, 0, traindata.Key.ColumnCount, traindata.Key);

            // target values
            Matrix<float> targets = traindata.Value;

            // dw, dv
            Matrix<float> dw = new DenseMatrix(HiddenNodes, pat.ColumnCount);
            Matrix<float> dv = new DenseMatrix(TargetSize, HiddenNodes + 1);

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                forwardPass(pat);
                backwardPass(pat, targets);
                weightUpdate(pat, dw, dv);
            }
        }

        private void forwardPass(Matrix<float> pat)
        {
            /*
                %forward pass
                hin = w * pat; % 1xhidden
                hout = [2 ./ (1+exp(-hin)) - 1 ; ones(1,n)]; %
                oin = v * hout;
                out = 2./ (1+exp(-oin)) - 1;
             */
            Matrix<float> hin = weights1.Multiply(pat);
            net_hout = new DenseMatrix(HiddenNodes + 1, pat.ColumnCount, 1.0f);
            for (int i = 0; i < HiddenNodes; i++)
                for (int j = 0; j < pat.ColumnCount; j++)
                    net_hout[i, j] = (float)(2.0 / (1.0 + Math.Exp(-hin[i, j])) - 1.0);

            Matrix<float> oin = weights2.Multiply(net_hout);
            net_out = new DenseMatrix(TargetSize, pat.ColumnCount);
            for (int i = 0; i < TargetSize; i++)
                for (int j = 0; j < pat.ColumnCount; j++)
                    net_out[i, j] = (float)(2.0 / (1.0 + Math.Exp(-oin[i, j])) - 1.0);
        }

        private void backwardPass(Matrix<float> pat, Matrix<float> targets)
        {
            /*
                %backward pass
                delta_o = (out - targets) .* ((1 + out) .* (1 - out)) * 0.5;
                delta_h = (v' * delta_o) .* ((1 + hout) .* (1 - hout)) * 0.5;
                delta_h = delta_h(1:hidden, :);
             */
            net_deltao = (net_out - targets) * ((net_out.Add(new DenseMatrix(net_out.RowCount, net_out.ColumnCount, 1.0f))) * (new DenseMatrix(net_out.RowCount, net_out.ColumnCount, 1.0f).Subtract(net_out))) * 0.5f;
            net_deltah = (weights2.Transpose().Multiply(net_deltao)) * ((net_hout.Add(new DenseMatrix(net_hout.RowCount, net_hout.ColumnCount, 1.0f))) * (new DenseMatrix(net_hout.RowCount, net_hout.ColumnCount, 1.0f).Subtract(net_hout))) * 0.5f;
            net_deltah = net_deltah.SubMatrix(0, HiddenNodes, 0, pat.ColumnCount);
        }

        private void weightUpdate(Matrix<float> pat, Matrix<float> dw, Matrix<float> dv)
        {
            /*
                dw = (dw .* alpha) - (delta_h * pat') .* (1-alpha);
                dv = (dv .* alpha) - (delta_o * hout') .* (1-alpha);
                w = w + dw .* eta .* (1 + rand(1,1)/1000)';
                v = v + dv .* eta .* (1 + rand(1,1)/1000)';
                             * 
                error(epoch) = sum(sum(abs(sign(out) - targets)./2));
             */
            float alpha = 0.9f;
            float eta = 0.1f;

            dw = (dw.Multiply(alpha)).Subtract((net_deltah * pat) * (1 - alpha));
            dv = (dv.Multiply(alpha)).Subtract((net_deltao * net_hout) * (1 - alpha));

            weights1 += dw.Multiply(eta).Multiply(1 + DataManipulation.rand(1, 1)[0, 0] / 1000f);
            weights2 += dv.Multiply(eta).Multiply(1 + DataManipulation.rand(1, 1)[0, 0] / 1000f);
        }

        internal void randomize()
        {
            this.weights1 = DataManipulation.rand(HiddenNodes, PatternSize + 1).Multiply(0.001f);
            this.weights2 = DataManipulation.rand(TargetSize, HiddenNodes + 1).Multiply(0.001f);
        }

        internal Matrix<float> use(ArrayInputData aid)
        {
            // convert
            aid.Key = aid.Key.InsertRow(aid.Key.RowCount, new DenseVector(1, 1.0f));

            // pass
            forwardPass(aid.Key);

            // return result
            return net_out;
        }
    }
}
