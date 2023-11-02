namespace SysBiblioteca.API.Management
{
    public class crypto
    {
        public static string Encrypt(string _cadenaAencriptar)
        {
            try
            {
                string result = string.Empty;
                byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
                result = Convert.ToBase64String(encryted);
                return result;
            }
            catch (Exception ex)
            {
                return _cadenaAencriptar;
            }
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string Decrypt(string _cadenaAdesencriptar)
        {
            try
            {
                string result = string.Empty;
                byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
                result = System.Text.Encoding.Unicode.GetString(decryted);
                return result;
            }
            catch (Exception ex)
            {
                return _cadenaAdesencriptar;
            }
        }
    }
}
