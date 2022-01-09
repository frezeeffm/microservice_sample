using System;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KafkaMessageBus
{
    public static class KafkaRegisterService
    {
        public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton(typeof(IKafkaMessageBus<,>), typeof(KafkaMessageBus.KafkaMessageBus<,>));

        public static IServiceCollection AddKafkaConsumer<Tk, Tv, THandler>(this IServiceCollection services,
            Action<KafkaConsumerOptions<Tk, Tv>> configAction) where THandler : class, IKafkaHandler<Tk, Tv>
        {
            services.AddScoped<IKafkaHandler<Tk, Tv>, THandler>();
            services.AddHostedService<KafkaConsumer<Tk, Tv>>();
            services.Configure(configAction);
            return services;
        }

        public static IServiceCollection AddKafkaProducer<Tk, Tv>(this IServiceCollection services,
            Action<KafkaProducerOptions<Tk, Tv>> configAction)
        {
            services.AddConfluentKafkaProducer<Tk, Tv>();

            services.AddSingleton<KafkaProducer<Tk, Tv>>();

            services.Configure(configAction);

            return services;
        }

        private static IServiceCollection AddConfluentKafkaProducer<Tk, Tv>(this IServiceCollection services)
        {
            services.AddSingleton(
                sp =>
                {
                    var config = sp.GetRequiredService<IOptions<KafkaProducerOptions<Tk, Tv>>>();

                    var builder = new ProducerBuilder<Tk, Tv>(config.Value).SetValueSerializer(new KafkaSerializer<Tv>());

                    return builder.Build();
                });

            return services;
        }
    }
}