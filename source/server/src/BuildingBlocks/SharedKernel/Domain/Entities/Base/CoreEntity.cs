namespace SharedKernel.Domain;

public class CoreEntity : ICoreEntity
{
    public object this[string propertyName]
    {
        get
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop == null)
            {
                throw new Exception(string.Format("Property {0} does not exists in {1}", propertyName, GetType().Name));
            }
            
            return prop.GetValue(this);
        }

        set
        {
            var prop = GetType().GetProperty(propertyName);
            if (prop == null)
            {
                throw new Exception(string.Format("Property {0} does not exists in {1}", propertyName, GetType().Name));
            }
            
            prop.SetValue(this, value);
        }
    }
}