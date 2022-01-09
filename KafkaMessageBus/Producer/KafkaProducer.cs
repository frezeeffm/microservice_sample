using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace KafkaMessageBus
{
    public class KafkaProducer<Tk, Tv> : IDisposable
    {
        private readonly IProducer<Tk, Tv> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaProducerOptions<Tk, Tv>> options, IProducer<Tk, Tv> producer)
        {
            _topic = options.Value.Topic;
            _producer = producer;
        }

        public async Task ProduceAsync(Tk key, Tv value)
        {
            await _producer.ProduceAsync(_topic, new Message<Tk, Tv> { Key = key, Value = value });
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}