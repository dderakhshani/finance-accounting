using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;

namespace Library.Utility
{
    public class CloneObject
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>

        public static T Clone<T>(T source)
        {
            throw new NotImplementedException();
            //if (!typeof(T).IsSerializable)
            //{
            //    throw new ArgumentException("The type must be serializable.", nameof(source));
            //}

            //// Don't serialize a null object, simply return the default for that object
            //if (ReferenceEquals(source, null)) return default;

            //using var stream = new MemoryStream();
            //IFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(stream, source);
            //stream.Seek(0, SeekOrigin.Begin);
            //return (T)formatter.Deserialize(stream);
        }
    }
}