namespace NeuralNetwork
{
	public class NeuralNetwork
	{
		public Topology Topology { get; }
		public List<Layer> Layers { get; }

		public NeuralNetwork(Topology topology)
		{
			Topology = topology;
			Layers = new List<Layer>();

			CreateInputLayer();
			CreateHiddenLayers();
			CreateOutputLayers();
		}

		public Neuron FeedForward(params double[] inputSignals)
		{
			SendSignalsToInputNeurons(inputSignals);
			FeedForwardAllLayersAfterInput();

			if (Topology.OutputCount == 1)
				return Layers.Last().Neurons[0];
			else
				return Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
		}

		public double Learn(double[] expected, double[,] inputs, int epoch)
		{
			var signals = Normalization(inputs);

			var error = 0.0;
			for (int i = 0; i < epoch; i++)
			{
				for (int j = 0; j < expected.Length; j++)
				{
					var output = expected[j];
					var input = GetRow(signals, j);

					error += BackPropagation(output, input);
				}
			}

			var result = error / epoch;
			return result;
		}

		private double[,] Scalling(double[,] inputs)
		{
			var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

			for (int col = 0; col < inputs.GetLength(1); col++)
			{
				var min = inputs[0, col];
				var max = inputs[0, col];

				for (int row = 1; row < inputs.GetLength(0); row++)
				{
					var item = inputs[row, col];

					if (item < min)
						min = item;

					if (item > max)
						max = item;
				}

				var divider = max - min;

				for (int row = 1; row < inputs.GetLength(0); row++)
					result[row, col] = (inputs[row, col] - min) / divider;
			}

			return result;
		}

		private double[,] Normalization(double[,] inputs)
		{
			var result = new double[inputs.GetLength(0), inputs.GetLength(1)];

			for (int col = 0; col < inputs.GetLength(1); col++)
			{
				// average value of signal
				var sum = 0.0;
				for (int row = 0; row < inputs.GetLength(0); row++)
					sum += inputs[row, col];
				var average = sum / inputs.GetLength(0);

				// standard quad offset of neuron
				var error = 0.0;
				for (int row = 0; row < inputs.GetLength(0); row++)
					error += Math.Pow(inputs[row, col] - average, 2);
				var standardError = Math.Sqrt(error / inputs.GetLength(0));

				for (int row = 0; row < inputs.GetLength(0); row++)
					result[row, col] = (inputs[row, col] - average) / standardError;
			}

			return result;
		}

		private double BackPropagation(double expected, params double[] inputs)
		{
			var actual = FeedForward(inputs).Output;
			var difference = actual - expected;

			foreach (var neuron in Layers.Last().Neurons)
				neuron.Learn(difference, Topology.LearningRate);

			for (int i = Layers.Count - 2; i >= 0; i--)
			{
				var layer = Layers[i];
				var previousLayer = Layers[i + 1];

				for (int j = 0; j < layer.NeuronCount; j++)
				{
					var neuron = layer.Neurons[i];

					for (int k = 0; k < previousLayer.NeuronCount; k++)
					{
						var previousNeuron = previousLayer.Neurons[k];
						var error = previousNeuron.Weights[i] * previousNeuron.Delta;

						neuron.Learn(error, Topology.LearningRate);
					}
				}
			}

			return difference * difference;
		}

		private void FeedForwardAllLayersAfterInput()
		{
			for (int i = 1; i < Layers.Count; i++)
			{
				var layer = Layers[i];
				var previousLayerSignals = Layers[i - 1].GetSignals();

				foreach (var neuron in layer.Neurons)
					neuron.FeedForward(previousLayerSignals);
			}
		}

		private void SendSignalsToInputNeurons(params double[] inputSignals)
		{
			for (int i = 0; i < inputSignals.Length; i++)
			{
				var signal = new List<double>() { inputSignals[i] };
				var neuron = Layers[0].Neurons[i];

				neuron.FeedForward(signal);
			}
		}

		private void CreateInputLayer()
		{
			var inputNeurons = new List<Neuron>();
			for (int i = 0; i < Topology.InputCount; i++)
			{
				var neuron = new Neuron(1, ENeuronType.Input);
				inputNeurons.Add(neuron);
			}

			var inputLayer = new Layer(inputNeurons, ENeuronType.Input);
			Layers.Add(inputLayer);
		}

		private void CreateHiddenLayers()
		{
			for (int j = 0; j < Topology.HiddenLayers.Count; j++)
			{
				var hiddenNeurons = new List<Neuron>();
				var lastLayer = Layers.Last();
				for (int i = 0; i < Topology.HiddenLayers[j]; i++)
				{
					var neuron = new Neuron(lastLayer.NeuronCount);
					hiddenNeurons.Add(neuron);
				}

				var hiddenLayer = new Layer(hiddenNeurons);
				Layers.Add(hiddenLayer);
			}
		}

		private void CreateOutputLayers()
		{
			var outputNeurons = new List<Neuron>();
			var lastLayer = Layers.Last();
			for (int i = 0; i < Topology.OutputCount; i++)
			{
				var neuron = new Neuron(lastLayer.NeuronCount, ENeuronType.Output);
				outputNeurons.Add(neuron);
			}

			var outputLayer = new Layer(outputNeurons, ENeuronType.Output);
			Layers.Add(outputLayer);
		}

		public static double[] GetRow(double[,] matrix, int row)
		{
			var columns = matrix.GetLength(1);
			var array = new double[columns];

			for (int i = 0; i < columns; i++)
				array[i] = matrix[row, i];

			return array;
		}
	}
}