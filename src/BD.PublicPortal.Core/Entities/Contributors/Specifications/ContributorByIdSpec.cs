namespace BD.PublicPortal.Core.Entities.Contributors.Specifications;

public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(int contributorId) =>
    Query
        .Where(contributor => contributor.Id == contributorId);

  
}


