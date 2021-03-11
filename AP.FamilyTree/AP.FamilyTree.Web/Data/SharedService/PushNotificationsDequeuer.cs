using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AP.FamilyTree.Db;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AP.FamilyTree.Web.Data.SharedService
{
    public class PushNotificationsDequeuer : IHostedService, IDisposable
    {
        private Timer _timer;
        private Task _executingTask;
        private readonly IPushNotificationsQueue _messagesQueue;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private EFRepository<LogApplicationError> appRepo { get; set; }
        private readonly IServiceScopeFactory _scopeFactory;
        public PushNotificationsDequeuer(IPushNotificationsQueue messagesQueue, IServiceScopeFactory scopeFactory)
        {
            _messagesQueue = messagesQueue;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(ExecuteTask, null, TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object state)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = ExecuteTaskAsync(_stoppingCts.Token);
        }

        private async Task DequeueMessagesAsync(CancellationToken stoppingToken)
        {
            LogMessageEntry message;
            do
            {
                message = await _messagesQueue.DequeueAsync(stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<FamilyTreeDbContext>();
                            appRepo = new EFRepository<LogApplicationError>(_context);
                            appRepo.Create(new LogApplicationError()
                            {
                                InsertDate = DateTime.Now,
                                ErrorMsg = message.ErrorMsg,
                                ErrorLevel = message.ErrorLevel,
                                UserData = message.UserData,
                                ErrorContext = message.ErrorContext,
                                BrowserInfo = message.BrowserInfo,
                                AppVersion = message.AppVersion
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        /* if (!EventLog.SourceExists(ConstField.LogSource))
                             EventLog.CreateEventSource(ConstField.LogSource, ConstField.LogName);
                         EventLog.WriteEntry(ConstField.LogSource, $"Exception - {e.Message}", EventLogEntryType.Error); */

                    }

                }
            } while (message != null);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            await DequeueMessagesAsync(stoppingToken);
            _timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(-1));
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
            _timer?.Dispose();
        }
    }
}
