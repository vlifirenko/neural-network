namespace NeuralNetwork
{
	public class Layer
	{
		public List<Neuron> Neurons { get; private set; }
		public int Count => Neurons?.Count ?? 0;

		public Layer(List<Neuron> neurons, ENeuronType type = ENeuronType.Normal)
		{
			Neurons = neurons;
		}

		public List<double> GetSignals()
		{
			var result = new List<double>();

			foreach (var neuron in Neurons)
				result.Add(neuron.Output);

			return result;
		}
	}
}