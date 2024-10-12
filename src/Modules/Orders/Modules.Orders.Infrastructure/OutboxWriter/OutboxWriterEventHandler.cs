namespace Modules.Orders.Infrastructure.OutboxWriter;

//public class OutboxWriterEventHandler(IServiceScopeFactory serviceScopeFactory, IDateTimeProvider dateTimeProvider) :
//INotificationHandler<UserCreatedDomainEvent>,
//INotificationHandler<RoleDeletedDomainEvent>
//{
//    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
//    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

//    public async Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
//    {
//        var user = domainEvent.User;

//        IIntegrationEvent integrationEvent = new UserCreatedIntegrationEvent(
//            user.Id.Value,
//            _dateTimeProvider.UtcNow,
//            user.Email,
//            user.Name.FirstName,
//            user.Name.LastName);

//        await AddOutboxIntegrationEventAsync(integrationEvent, cancellationToken);
//    }

//    public async Task Handle(RoleDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
//    {
//        // not needed for now 
//    }


//    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
//    {
//        using var scope = _serviceScopeFactory.CreateScope();
//        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

//        string name = integrationEvent.GetType().Name;
//        string content = JsonConvert.SerializeObject(integrationEvent);

//        await dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
//            EventName: name,
//            EventContent: content,
//            CreatedOn: DateTime.UtcNow));

//        // TODO: currently it is separate transaction
//        // setup to use same DB transaction via middleware or research
//        // check if dbContext is set in 1 transaction, it will trigger SaveChangesInterceptor -
//        // if still triggers it, try to use Postgre Connection directly... Remove useage of dbCOntext here
//        await dbContext.SaveChangesAsync(cancellationToken);
//    }
//}