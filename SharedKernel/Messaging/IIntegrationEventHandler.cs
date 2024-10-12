using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Messaging;
public interface IIntegrationEventHandler<T> where T : class, IIntegrationEvent
{
    public Task Handle(T integrationEvent, CancellationToken cancellationToken);
}
