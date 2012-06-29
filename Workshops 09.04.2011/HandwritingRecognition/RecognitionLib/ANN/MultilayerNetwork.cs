using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Single;
using MathNet.Numerics.LinearAlgebra.Generic;
using RecognitionLib;

namespace RecognitionLib.ANN
{
    /// <summary>
    /// An artificial neural network class, representing a multilayered feedforward net with one hidden layer.
    /// The number of hidden nodes, the pattern and target size can be specified.
    /// Training is done with common backprop.
    /// </summary>
    class MultilayerNetwork
    {
        /// <summary>
        /// Number of hidden nodes in the hidden layer.
        /// </summary>
        public int HiddenNodes
        {
            get { return weights1.RowCount; }
        }
        /// <summary>
        /// Data size of one pattern.
        /// </summary>
        public int PatternSize
        {
            get { return weights1.ColumnCount - 1; }
        }
        /// <summary>
        /// Size of one target.
        /// </summary>
        public int TargetSize
        {
            get { return weights2.RowCount; }
        }

        /// <summary>
        /// Weights between input and hidden layer.
        /// </summary>
        private Matrix<float> weights1;
        /// <summary>
        /// Weights between hidden and output layer.
        /// </summary>
        private Matrix<float> weights2;
        /// <summary>
        /// Output of the network after feedforward() has been called.
        /// </summary>
        private Matrix<float> net_out;
        /// <summary>
        /// Output of the hidden layer, used for backprop.
        /// </summary>
        private Matrix<float> net_hout;
        /// <summary>
        /// Delta of the output, used for backprop.
        /// </summary>
        private Matrix<float> net_deltao;
        /// <summary>
        /// Delta of the hidden layer, used for backprop.
        /// </summary>
        private Matrix<float> net_deltah;

        /// <summary>
        /// A feedforward multilayer network with one hidden layer.
        /// </summary>
        /// <param name="hiddenNodes">number of hidden nodes.</param>
        /// <param name="patternsize">data size of one pattern.</param>
        /// <param name="targetsize">data size of one target.</param>
        public MultilayerNetwork(int hiddenNodes, int patternsize, int targetsize)
        {
            this.weights1 = new DenseMatrix(hiddenNodes, patternsize + 1);
            this.weights2 = new DenseMatrix(targetsize, hiddenNodes + 1);

            randomize();
        }

        /// <summary>
        /// Trains the network using common backprop.
        /// </summary>
        /// <param name="traindata">all training data used in this training session. Matrix must have the size (PatternSize X PatternCount)</param>
        /// <param name="epochs">Number of epochs to train the network.</param>
        internal void backpropTraining(ArrayInputData traindata, int epochs)
        {
            // training pattern init
            Matrix<float> pat = new DenseMatrix(traindata.Key.RowCount + 1, traindata.Key.ColumnCount, 1.0f);
            pat.SetSubMatrix(0, traindata.Key.RowCount, 0, traindata.Key.ColumnCount, traindata.Key);

            // target values init
            Matrix<float> targets = traindata.Value;

            // dw, dv
            Matrix<float> dw = new DenseMatrix(HiddenNodes, PatternSize + 1);
            Matrix<float> dv = new DenseMatrix(TargetSize, HiddenNodes + 1);

            // training
            for (int i = 0; i < epochs; i++)
            {
                // backprop
                forwardPass(pat);
                backwardPass(pat, targets);
                weightUpdate(pat, dw, dv);
            }
        }

        /// <summary>
        /// Forward pass, used to push input through the net to generate output.
        /// </summary>
        /// <param name="pat">input patterns.</param>
        private void forwardPass(Matrix<float> pat)
        {
            /*
                % forward pass, MATLAB code
                hin = w * pat; % 1xhidden
                hout = [2 ./ (1+exp(-hin)) - 1 ; ones(1,n)];
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

        /// <summary>
        /// Backward pass, used for backprop.
        /// Propagates the input from the forward pass back into the network.
        /// </summary>
        /// <param name="pat">input pattern</param>
        /// <param name="targets">targets for the input</param>
        private void backwardPass(Matrix<float> pat, Matrix<float> targets)
        {
            /*
                % backward pass, MATLAB code
                delta_o = (out - targets) .* ((1 + out) .* (1 - out)) * 0.5;
                delta_h = (v' * delta_o) .* ((1 + hout) .* (1 - hout)) * 0.5;
                delta_h = delta_h(1:hidden, :);
             */
            Matrix<float> r1= (net_out.Add(new DenseMatrix(net_out.RowCount, net_out.ColumnCount, 1.0f))).PointwiseMultiply(new DenseMatrix(net_out.RowCount, net_out.ColumnCount, 1.0f).Subtract(net_out));
            net_deltao = (net_out - targets).PointwiseMultiply(r1).Multiply(0.5f);
            Matrix<float> r2 = (net_hout.Add(new DenseMatrix(net_hout.RowCount, net_hout.ColumnCount, 1.0f))).PointwiseMultiply(new DenseMatrix(net_hout.RowCount, net_hout.ColumnCount, 1.0f).Subtract(net_hout));
            net_deltah = (weights2.Transpose().Multiply(net_deltao)).PointwiseMultiply(r2).Multiply(0.5f);

            net_deltah = net_deltah.SubMatrix(0, HiddenNodes, 0, pat.ColumnCount);
        }

        /// <summary>
        /// After the forward and backward pass, the weights must be updated. This is done in this function.
        /// </summary>
        /// <param name="pat">input pattern</param>
        /// <param name="dw">delta w, from last updates. Used to update weights1 after recalculation.</param>
        /// <param name="dv">delta v, from last updates. Used to update weights2 after recalculation.</param>
        private void weightUpdate(Matrix<float> pat, Matrix<float> dw, Matrix<float> dv)
        {
            /*
                % weight update, MATLAB code
                dw = (dw .* alpha) - (delta_h * pat') .* (1-alpha);
                dv = (dv .* alpha) - (delta_o * hout') .* (1-alpha);
                w = w + dw .* eta .* (1 + rand(1,1)/1000)';
                v = v + dv .* eta .* (1 + rand(1,1)/1000)';
                             
                error(epoch) = sum(sum(abs(sign(out) - targets)./2));
             */
            float alpha = 0.9f;
            float eta = 0.1f;

            dw = (dw.Multiply(alpha)).Subtract((net_deltah.Multiply(pat.Transpose())).Multiply(1 - alpha));
            dv = (dv.Multiply(alpha)).Subtract((net_deltao.Multiply(net_hout.Transpose())).Multiply(1 - alpha));

            weights1 += dw.Multiply(eta).Multiply(1 + DataManipulation.rand(1, 1)[0, 0] / 1000f);
            weights2 += dv.Multiply(eta).Multiply(1 + DataManipulation.rand(1, 1)[0, 0] / 1000f);
        }

        /// <summary>
        /// Randomizes the weights to reinitialize the network.
        /// </summary>
        internal void randomize()
        {
            this.weights1 = DataManipulation.rand(HiddenNodes, PatternSize + 1).Multiply(0.001f);
            this.weights2 = DataManipulation.rand(TargetSize, HiddenNodes + 1).Multiply(0.001f);
        }

        /// <summary>
        /// Uses the network with one input pattern to genereate output.
        /// </summary>
        /// <param name="pat">input pattern. Size (PatternSize X 1)</param>
        /// <returns>the output matrix of the network</returns>
        internal Matrix<float> use(ArrayInputData pat)
        {
            // convert
            pat.Key = pat.Key.InsertRow(pat.Key.RowCount, new DenseVector(1, 1.0f));

            // pass
            forwardPass(pat.Key);

            // return result
            return net_out;
        }
    }
}
