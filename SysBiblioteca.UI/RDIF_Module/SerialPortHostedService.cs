namespace SysBiblioteca.UI.RDIF_Module
{
    public class SerialPortHostedService : IHostedService
    {
        private readonly SerialPortListener _serialPortListener;

        public SerialPortHostedService(SerialPortListener serialPortListener)
        {
            _serialPortListener = serialPortListener;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serialPortListener.StartListening();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _serialPortListener.Dispose();
            return Task.CompletedTask;
        }
    }
}