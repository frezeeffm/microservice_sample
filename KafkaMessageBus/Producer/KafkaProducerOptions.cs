using Confluent.Kafka;

namespace KafkaMessageBus
{
    public class KafkaProducerOptions<Tk, Tv> : ProducerConfig
    {
        public string Topic { get; set; }
    }
}