using RJCP.IO.Ports;

namespace SysBiblioteca.UI.RDIF_Module
{
    public class SerialPortListener : IDisposable
    {
        private readonly SerialPortStream _serialPort;

        public SerialPortListener(string portName, int baudRate)
        {
            _serialPort = new SerialPortStream(portName, baudRate);
            _serialPort.DataReceived += OnDataReceived;
        }

        public void StartListening()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Acceso denegado al puerto {_serialPort.PortName}: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error de E/S al intentar abrir el puerto {_serialPort.PortName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al abrir el puerto {_serialPort.PortName}: {ex.Message}");
            }
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = _serialPort.ReadLine();
                Console.WriteLine($"Data received: {data}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error al leer del puerto {_serialPort.PortName}: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _serialPort.Dispose();
        }
    }
}