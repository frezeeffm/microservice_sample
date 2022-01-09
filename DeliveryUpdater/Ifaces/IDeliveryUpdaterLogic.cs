using System.Threading.Tasks;

namespace DeliveryUpdater.Ifaces
{
    public interface IDeliveryUpdaterLogic
    {
        public Task Update(int id);
    }
}