using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Setting
{
     public class EnviromentSetting
    {
        private static EnviromentSetting _instance;
        private static object _lockObject = new object();

        private const string DATABASE_CONNECTION_STRING = "PetMeConnectionString";
        private const string SMTP_HOST = "SmtpServer";
        private const string SMTP_PORT = "SmtpPort";
        private const string SMTP_PASSWORD = "SmtpAppPassword";
        private const string SMTP_FROM_EMAIL = "SmtpEmail";

        private static string _databaseConnectionString = string.Empty;
        private static string _smtp_host = string.Empty;
        private static string _smtp_port = string.Empty;
        private static string _smtp_password = string.Empty;
        private static string _smtp_from_email = string.Empty;

        private EnviromentSetting() { }

        public static EnviromentSetting GetInstance() {
            if (_instance == null) {
                lock (_lockObject) {
                    if (_instance == null) {
                        _instance = new EnviromentSetting();

                        _databaseConnectionString = Environment.GetEnvironmentVariable(DATABASE_CONNECTION_STRING);
                        _smtp_host = Environment.GetEnvironmentVariable(SMTP_HOST);
                        _smtp_port =  Environment.GetEnvironmentVariable(SMTP_PORT);
                        _smtp_password = Environment.GetEnvironmentVariable(SMTP_PASSWORD);
                        _smtp_from_email = Environment.GetEnvironmentVariable(SMTP_FROM_EMAIL);
                    }
                }
            }
            return _instance;
        }

        public string GetConnectionString() => _databaseConnectionString;
        public string GetSMTPHost() => _smtp_host;
        public string GetSMTPPort() => _smtp_port;
        public string GetSMTPPassword() => _smtp_password;
        public string GetSMTPFFromEmail() => _smtp_from_email;
    }
}
