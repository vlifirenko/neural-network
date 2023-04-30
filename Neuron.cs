namespace NeuralNetwork
{
	public class Neuron
	{
		public List<double> Weights { get; }
		public ENeuronType NeuronType { get; }
		public double Output { get; private set; }

		public Neuron(int inputCount, ENeuronType type = ENeuronType.Normal)
		{
			NeuronType = type;
			Weights = new List<double>();

			for (int i = 0; i < inputCount; i++)
				Weights.Add(1);
		}

		public double FeedForward(List<double> inputs)
		{
			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
				sum += inputs[i] * Weights[i];

			Output = Sigmoid(sum);
			return Output;
		}

		public void SetWeights(params double[] weights)
		{
			for (int i = 0; i < weights.Length; i++)
				Weights[i] = weights[i];
		}

		private static double Sigmoid(double x) => 1.0 / (1.0 + Math.Pow(Math.E, -x));

		public override string ToString() => Output.ToString();
	}
}