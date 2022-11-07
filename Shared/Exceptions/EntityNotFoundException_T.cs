namespace Shared.Exceptions
{
    /// <summary>
    /// Исключение при невозможности обнаружить объект по переданным параметрам.
    /// </summary>
    public class EntityNotFoundException<T> : EntityNotFoundException
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string key) : base(key, typeof(T).Name)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}