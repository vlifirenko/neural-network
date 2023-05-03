namespace NeuralNetwork
{
	public class Neuron
	{
		public List<double> Weights { get; }
		public List<double> Inputs { get; }
		public ENeuronType NeuronType { get; }
		public double Output { get; private set; }
		public double Delta { get; private set; }

		public Neuron(int inputCount, ENeuronType type = ENeuronType.Normal)
		{
			NeuronType = type;
			Weights = new List<double>();
			Inputs = new List<double>();

			InitWeightsRandomValue(inputCount);
		}

		private void InitWeightsRandomValue(int inputCount)
		{
			var random = new Random();

			for (int i = 0; i < inputCount; i++)
			{
				if (NeuronType == ENeuronType.Input)
					Weights.Add(1);
				else
					Weights.Add(random.NextDouble());
				Inputs.Add(0);
			}
		}

		public double FeedForward(List<double> inputs)
		{
			for (int i = 0; i < inputs.Count; i++)
				Inputs[i] = inputs[i];

			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
				sum += inputs[i] * Weights[i];

			if (NeuronType != ENeuronType.Input)
				Output = Sigmoid(sum);
			else
				Output = sum;

			return Output;
		}

		public void Learn(double error, double learningRate)
		{
			if (NeuronType == ENeuronType.Input)
				return;

			Delta = error * SigmoidDx(Output);

			for (int i = 0; i < Weights.Count; i++)
			{
				var weight = Weights[i];
				var input = Inputs[i];
				var newWeight = weight - input * Delta * learningRate;

				Weights[i] = newWeight;
			}
		}

		private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Pow(Math.E, -x));

		private static double SigmoidDx(double x)
		{
			var sigmoid = Sigmoid(x);
			var result = sigmoid / 1 - sigmoid;

			return result;
		}

		public override string ToString() => Output.ToString();
	}
}