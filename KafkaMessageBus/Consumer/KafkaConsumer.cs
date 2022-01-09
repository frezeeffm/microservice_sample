using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KafkaMessageBus
{
    public class KafkaConsumer<Tk, Tv> : BackgroundService
    {
        private readonly KafkaConsumerOptions<Tk, Tv> _options;
        private IKafkaHandler<Tk, Tv> _handler;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumer(IOptions<KafkaConsumerOptions<Tk, Tv>> options, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<Tk, Tv>>();

                var builder = new ConsumerBuilder<Tk, Tv>(_options).SetValueDeserializer(new KafkaDeserializer<Tv>());

                var consumer = builder.Build();

                consumer.Subscribe(_options.Topic);

                Task.Run(async () => await Consume(consumer, stoppingToken));
            }

            return Task.CompletedTask;
        }

        private async Task Consume(IConsumer<Tk, Tv> consumer, CancellationToken ctn)
        {
            while (!ctn.IsCancellationRequested)
            {
                var result = consumer.Consume(TimeSpan.FromMilliseconds(1000));

                if (result != null)
                {
                    await _handler.HandleAsync(result.Message.Key, result.Message.Value);

                    consumer.Commit(result);

                    consumer.StoreOffset(result);
                }
            }
        }
    }
}