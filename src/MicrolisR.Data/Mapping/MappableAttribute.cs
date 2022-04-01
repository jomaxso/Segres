namespace MicrolisR.Data.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MappableAttribute : Attribute
    {
        public MappableAttribute(Type classType)
        {
            this.ClassType = classType;
        }

        public Type ClassType { get; }
    }
}
