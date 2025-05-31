namespace Domain.Entities
{
    /// <summary>
    /// Base class for all entities that includes soft delete functionality
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Flag indicating if the entity has been soft-deleted
        /// </summary>
        public bool IsDeleted { get; private set; } = false;

        /// <summary>
        /// Marks the entity as deleted without removing it from the database
        /// </summary>
        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        /// <summary>
        /// Restores a previously deleted entity
        /// </summary>
        public void RestoreDeleted()
        {
            IsDeleted = false;
        }
    }
}