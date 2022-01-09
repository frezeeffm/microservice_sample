using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Confluent.Kafka;
using DeliveryUpdater.Data;
using DeliveryUpdater.Ifaces;
using KafkaMessageBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeliveryUpdater.Logic
{
    public class DeliveryUpdaterLogic : IDeliveryUpdaterLogic
    {
        private readonly IKafkaMessageBus<string, MyProduct> _bus;

        public DeliveryUpdaterLogic(IKafkaMessageBus<string, MyProduct> messageBus)
        {
            _bus = messageBus;
        }

        public async Task Update(int id)
        {
            try
            {
                var cl = new HttpClient();
                var req = $"https://api.delivery-club.ru/api1.2/vendor/{id}/menu?data=products";
                var resp = await cl.GetAsync(req);
                if (!resp.IsSuccessStatusCode)
                {
                    return;
                }

                var result = await resp.Content.ReadAsStringAsync();
                var prods = JObject.Parse(result);
                await ParseAndSend(prods);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task ParseAndSend(JObject obj)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<List<Product>>(obj["products"].ToString());
                foreach (var product in res)
                {
                    await _bus.PublishAsync("add", new MyProduct
                    {
                        Name = product.Name,
                        Price = product.Price.Value,
                        StoreName = "Teremok"
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}