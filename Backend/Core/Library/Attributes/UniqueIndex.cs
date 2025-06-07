using System;

namespace Library.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueIndex : Attribute
    {

    }
}