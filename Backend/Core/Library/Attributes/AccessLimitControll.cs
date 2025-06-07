using System;

namespace Library.Attributes
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AccessLevelControl : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AccessPermissionControl : Attribute
    {
    }


}