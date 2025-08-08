using System;
using System.Collections.Generic;

namespace GradeApp.Database.Factories
{
    public abstract class BaseFactory<T> where T : class
    {
        protected static readonly Random Random = new Random();

        // Factory()->create() - maakt 1 item
        public abstract T Create();

        // Factory(10)->create() - maakt meerdere items
        public List<T> Create(int count)
        {
            var items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                items.Add(Create());
            }
            return items;
        }

        // Factory()->create(item => item.Name = "Test") - met overrides
        public T Create(Action<T> overrides)
        {
            var item = Create();
            overrides(item);
            return item;
        }
    }
}