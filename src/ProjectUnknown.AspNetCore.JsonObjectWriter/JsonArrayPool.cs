using System.Buffers;
using Newtonsoft.Json;

namespace ProjectUnknown.AspNetCore.JsonWriter
{
    internal class JsonArrayPool<T> : IArrayPool<T>
    {
        private readonly ArrayPool<T> inner;

        public JsonArrayPool(ArrayPool<T> inner)
        {
            this.inner = inner;
        }

        public T[] Rent(int minimumLength)
        {
            return inner.Rent(minimumLength);
        }

        public void Return(T[] array)
        {
            inner.Return(array);
        }
    }
}
