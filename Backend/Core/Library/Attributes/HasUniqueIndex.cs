using System;

namespace Library.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HasUniqueIndex : Attribute
    {

    }
}