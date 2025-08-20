using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    public static class MqttHelper
    {
        private static readonly string _brokerIp = "localhost";
        private static readonly int _brokerPort = 1883;
        private static readonly bool _cleanSession = true;

        private static IMqttClient _mqttClient;
        private static IMqttClientOptions _options;

        static MqttHelper()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(_brokerIp, _brokerPort)
                .WithCleanSession(_cleanSession)
                .Build();
        }

        public static async Task<bool> PublishAsync(string topic, string payloadJson)
        {
            try
            {
                if (!_mqttClient.IsConnected)
                {
                    var result = await _mqttClient.ConnectAsync(_options);
                    if (result.ResultCode.ToString() != "Success")
                        return false;
                }

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(payloadJson))
                    .WithExactlyOnceQoS()
                    .WithRetainFlag(false)
                    .Build();

                await _mqttClient.PublishAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            var ip = host.AddressList.FirstOrDefault(a =>
                a.AddressFamily == AddressFamily.InterNetwork &&
                !IPAddress.IsLoopback(a));

            return ip?.ToString() ?? "127.0.0.1"; // fallback
        }
    }

}
