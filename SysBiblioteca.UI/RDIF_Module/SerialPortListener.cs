using RJCP.IO.Ports;

namespace SysBiblioteca.UI.RDIF_Module
{
    public class SerialPortListener : IDisposable
    {
        private SerialPortStream _serialPort;

        public event Action<string> OnTagRead;

        public SerialPortListener(string portName, int baudRate)
        {
            _serialPort = new SerialPortStream(portName, baudRate)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                RtsEnable = true,
                DtrEnable = true
            };

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
        }

        public void StartListening()
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.DataReceived += SerialPort_DataReceived;
                    _serialPort.Open();

                    Console.WriteLine("Puerto serial abierto exitosamente.");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine("Error: Acceso no autorizado al puerto serial. Intenta ejecutar como administrador.");
                    Dispose();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error al abrir el puerto serial: {ex.Message}");
                    Dispose();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Error: El puerto ya está abierto o no se puede acceder. Detalles: {ex.Message}");
                    Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Dispose();
                }
            }
        }

        private void SerialPort_DataReceived(object sender, EventArgs e)
        {
            string data = _serialPort.ReadLine();
            Console.WriteLine($"Datos recibidos del puerto serial: {data}");
            if (data.StartsWith("Ejemplar:"))
            {
                string ejemplarCode = data.Replace("Ejemplar:", "").Trim();
                OnTagRead?.Invoke(ejemplarCode);
            }
        }

        public void StopListening()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Close();
                    Dispose();
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cerrar el puerto serial: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.DataReceived -= SerialPort_DataReceived;
                    _serialPort.Close();
                    Thread.Sleep(500); // Espera medio segundo después de cerrar el puerto
                }
                _serialPort.Dispose();
                _serialPort = null; // Asegúrate de establecerlo en null después de desecharlo
            }
        }
    }
}