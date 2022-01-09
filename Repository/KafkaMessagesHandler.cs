using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using FluentValidation;
using KafkaMessageBus;
using Repository.Ifaces;
using Repository.Models;

namespace Repository
{
    public class KafkaMessagesHandler : IKafkaHandler<string, Product>
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<Product> _validator;
        
        public KafkaMessagesHandler(IProductRepository repository, IValidator<Product> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task HandleAsync(string key, Product value)
        {
            try
            {
                var res = await _validator.ValidateAsync(value);
                if (!res.IsValid)
                    Console.WriteLine(res.ToString("~"));
                await _repository.Create(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}