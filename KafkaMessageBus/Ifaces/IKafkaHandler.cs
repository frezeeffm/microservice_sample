using System.Threading.Tasks;

namespace KafkaMessageBus
{
    public interface IKafkaHandler<Tk, Tv>
    {
        Task HandleAsync(Tk key, Tv value);
    }
}