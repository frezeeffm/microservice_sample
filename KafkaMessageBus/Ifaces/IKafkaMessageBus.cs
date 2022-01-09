using System.Threading.Tasks;

namespace KafkaMessageBus
{
    public interface IKafkaMessageBus<Tk, Tv>
    {
        Task PublishAsync(Tk key, Tv message);
    }
}