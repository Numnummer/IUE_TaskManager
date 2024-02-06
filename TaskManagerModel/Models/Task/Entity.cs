namespace Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Entity(string? name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
        public Entity() { }

    }
}