using Confluent.Kafka;

namespace KafkaMessageBus
{
    public class KafkaConsumerOptions<Tk, Tv> : ConsumerConfig
    {
        public string Topic { get; set; }
        public KafkaConsumerOptions()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
            EnableAutoOffsetStore = false;
        }
    }
}