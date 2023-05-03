namespace NeuralNetwork
{
	public class Layer
	{
		public List<Neuron> Neurons { get; private set; }
		public int NeuronCount => Neurons?.Count ?? 0;
		public ENeuronType Type;

		public Layer(List<Neuron> neurons, ENeuronType type = ENeuronType.Normal)
		{
			Neurons = neurons;
			Type = type;
		}

		public List<double> GetSignals()
		{
			var result = new List<double>();

			foreach (var neuron in Neurons)
				result.Add(neuron.Output);

			return result;
		}

		public override string ToString() => Type.ToString();
	}
}