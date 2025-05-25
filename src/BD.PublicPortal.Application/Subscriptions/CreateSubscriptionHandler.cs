using BD.PublicPortal.Core.Entities;
using BD.PublicPortal.Core.DTOs;
using BD.PublicPortal.Core.Entities.Specifications;

namespace BD.PublicPortal.Application.Subscriptions;

public class CreateSubscriptionHandler(IRepository<DonorBloodTransferCenterSubscriptions> _subscriptionRepository)   
    : ICommandHandler<CreateSubscriptionCommand, Result<DonorBloodTransferCenterSubscriptionsDTO>>  
{  
    public async Task<Result<DonorBloodTransferCenterSubscriptionsDTO>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)  
    {
    var spec = new ExistingSubscriptionSpecification(request.ApplicationUserId, request.BloodTansfusionCenterId);
    var existingSubscription = await _subscriptionRepository.FirstOrDefaultAsync(spec, cancellationToken);

    if (existingSubscription != null)
    {
      return Result.Error("Subscription already exists");
    }


    var subscription = new DonorBloodTransferCenterSubscriptions  
        {  
            Id = Guid.NewGuid(),  
            BloodTansfusionCenterId = request.BloodTansfusionCenterId,  
            ApplicationUserId = request.ApplicationUserId  
        };  
  
        
        var savedSubscription = await _subscriptionRepository.AddAsync(subscription, cancellationToken);  
          
        
        var subscriptionDto = savedSubscription.ToDtoWithRelated(1);  
          
        return Result.Success(subscriptionDto);  
    }  
}
