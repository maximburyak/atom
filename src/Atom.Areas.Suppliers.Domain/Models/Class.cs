namespace Atom.Areas.Suppliers.Domain.Models
{
	public class ClassCms
	{
		public virtual string Class { get; set; }
		public virtual string Description { get; set; }
		public virtual int Hide { get; set; }
		public virtual string ClassDescription
		{
			get
			{
				return string.Format("{0} - [{1}]", Description, Class);
			}
		}
	}
}