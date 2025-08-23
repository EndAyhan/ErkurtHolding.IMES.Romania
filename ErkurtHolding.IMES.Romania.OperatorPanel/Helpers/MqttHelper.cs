using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Helpers
{
    /// <summary>
    /// Lightweight MQTT publishing helper built on MQTTnet for .NET Framework 4.8.
    /// </summary>
    public static class MqttHelper
    {
        // Defaults (can be overridden via Configure)
        private static string _brokerIp = "localhost";
        private static int _brokerPort = 1883;
        private static bool _cleanSession = true;

        private static readonly object _sync = new object();
        private static IMqttClient _mqttClient;
        private static IMqttClientOptions _options;
        private static readonly SemaphoreSlim _connectGate = new SemaphoreSlim(1, 1);

        static MqttHelper()
        {
            InitializeClient();
        }

        /// <summary>
        /// Changes the target broker/port and reinitializes the client/options.
        /// Safe to call at startup if you want to override defaults from config.
        /// </summary>
        /// <param name="brokerIp">Broker host/IP (e.g., "127.0.0.1" or "mqtt.myco.local").</param>
        /// <param name="brokerPort">Broker port (default 1883; 8883 for TLS if enabled externally).</param>
        /// <param name="cleanSession">Whether to start with a clean session.</param>
        public static void Configure(string brokerIp, int brokerPort = 1883, bool cleanSession = true)
        {
            if (string.IsNullOrWhiteSpace(brokerIp)) throw new ArgumentNullException(nameof(brokerIp));

            lock (_sync)
            {
                _brokerIp = brokerIp;
                _brokerPort = brokerPort;
                _cleanSession = cleanSession;
                DisposeClient_NoThrow();
                InitializeClient();
            }
        }

        /// <summary>
        /// Publishes a JSON payload to the given topic.
        /// Returns <c>true</c> on success; <c>false</c> otherwise (no throw).
        /// </summary>
        /// <param name="topic">The topic to publish to (non-empty).</param>
        /// <param name="payloadJson">UTF‑8 JSON string payload (non-null).</param>
        /// <param name="ct">Optional cancellation token.</param>
        public static async Task<bool> PublishAsync(string topic, string payloadJson, CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(topic))
                return false;
            if (payloadJson == null)
                return false;

            try
            {
                await EnsureConnectedAsync(ct).ConfigureAwait(false);
                if (_mqttClient == null || !_mqttClient.IsConnected) return false;

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(payloadJson))
                    .WithExactlyOnceQoS()
                    .WithRetainFlag(false)
                    .Build();

                // In MQTTnet v3, PublishAsync returns Task; v4 returns result. We ignore result to stay compatible.
                await _mqttClient.PublishAsync(message, ct).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // -------------------- internals --------------------

        private static void InitializeClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var clientId = BuildClientId();

            _options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(_brokerIp, _brokerPort)
                .WithCleanSession(_cleanSession)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .WithCommunicationTimeout(TimeSpan.FromSeconds(10))
                // .WithCredentials("user","pass") // add here if your broker requires auth
                // .WithTls() // enable if you use TLS (port 8883 typically). Configure certs as needed.
                .Build();

            // (Optional) handle disconnects – here we do nothing special, but you can auto-reconnect if desired.
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
        }

        private static async Task EnsureConnectedAsync(CancellationToken ct)
        {
            if (_mqttClient != null && _mqttClient.IsConnected) return;

            await _connectGate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (_mqttClient == null)
                {
                    InitializeClient();
                }


                if (!_mqttClient.IsConnected)
                {
                    // Try connect once; optionally you could add a small retry here
                    await _mqttClient.ConnectAsync(_options, ct).ConfigureAwait(false);
                }
            }
            finally
            {
                _connectGate.Release();
            }
        }

        private static void OnDisconnected(MqttClientDisconnectedEventArgs args)
        {
            // Hook for logging or delayed reconnect strategy if necessary.
            // Example simple delayed reconnect (fire-and-forget, swallow errors):
            // Task.Run(async () =>
            // {
            //     await Task.Delay(2000);
            //     try { await _mqttClient.ConnectAsync(_options); } catch { }
            // });
        }

        private static void DisposeClient_NoThrow()
        {
            try
            {
                var c = _mqttClient;
                if (c != null)
                {
                    if (c.IsConnected)
                    {
                        try { c.DisconnectAsync().Wait(2000); } catch { /* ignore */ }
                    }
                    c.Dispose();
                }
            }
            catch { /* ignore */ }
            finally
            {
                _mqttClient = null;
            }
        }


        /// <summary>
        /// Returns the first non-loopback IPv4 of this machine, or 127.0.0.1 if none.
        /// </summary>
        private static string GetLocalIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var a in host.AddressList)
                {
                    if (a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a))
                        return a.ToString();
                }
            }
            catch { /* ignore */ }
            return "127.0.0.1";
        }

        private static string BuildClientId()
        {
            // ClientId should be unique per client – include machine and process
            var host = Environment.MachineName ?? "host";
            var proc = Process.GetCurrentProcess();
            return $"imes-{host}-{proc.Id}";
        }
    }
}
