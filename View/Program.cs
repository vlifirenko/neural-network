using NeuralNetwork.View;

namespace NeuralNetwork
{
    internal static class Program
    {
        public static SystemController Controller { get; private set; }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            Controller = new SystemController();
        }
    }
}