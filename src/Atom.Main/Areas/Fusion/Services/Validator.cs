using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using xVal.ServerSide;

namespace Atom.Main.Areas.Fusion.Services
{
    public static class Validator<T>
    {
        public static void Validate(T obj)
        {
            var errors = DataAnnotationsValidationRunner.GetErrors(obj).ToList();
            if (errors.Any())
                throw new RulesException(errors);
        }
    }

	public static class DataAnnotationsValidationRunner
	{
		public static IEnumerable<ErrorInfo> GetErrors(object instance)
		{
			return from prop in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>()
				   from attribute in prop.Attributes.OfType<ValidationAttribute>()
				   where !attribute.IsValid(prop.GetValue(instance))
				   select new ErrorInfo(prop.Name, attribute.FormatErrorMessage(string.Empty), instance);
		}
	}
}
