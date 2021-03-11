using System.Threading;
using System.Threading.Tasks;
using AP.FamilyTree.Web.Data.Models;

namespace AP.FamilyTree.Web.Data.SharedService
{
    public interface IPushNotificationsQueue
    {
        void Enqueue(LogMessageEntry message);

        Task<LogMessageEntry> DequeueAsync(CancellationToken cancellationToken);
    }
}
