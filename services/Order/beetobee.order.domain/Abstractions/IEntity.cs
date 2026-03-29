namespace beetobee.order.domain.Abstractions;

public interface IEntity<T> : IEntity
{
    public T Id { get; set; }
}

public interface IEntity 
{
    public DateTime? CreatedAt { get; set; }
}
