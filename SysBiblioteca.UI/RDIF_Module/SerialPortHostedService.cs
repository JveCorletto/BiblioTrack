namespace SysBiblioteca.UI.RDIF_Module
{
    public class SerialPortHostedService : IHostedService, IDisposable
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
            _serialPortListener.StopListening();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _serialPortListener.Dispose();
        }
    }
}