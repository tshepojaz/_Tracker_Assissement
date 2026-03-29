namespace beetobee.order.domain.Abstractions;

public abstract class Entity<T> :IEntity
{
   public required T Id { get; set; }
   public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

}
