namespace Case_00038620_tls._1._2
{
    using System.Net;
    using NServiceBus;
    using NServiceBus.Logging;

    public class TLSWorkaround : INeedInitialization
    {
        private static ILog logger = LogManager.GetLogger<TLSWorkaround>();

        public void Customize(BusConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            logger.Warn("TLS 1.2 workaround applied.");
        }
    }
}
