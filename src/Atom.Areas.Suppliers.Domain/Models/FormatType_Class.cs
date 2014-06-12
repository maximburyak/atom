namespace Atom.Areas.Suppliers.Domain.Models
{
	public class FormatType_Class
	{
		public virtual FormatType FormatType { get; set; }
		public virtual ClassCms Class { get; set; }
		public override bool Equals(object obj)
		{
			return obj.GetHashCode().Equals(GetHashCode());
		}
		public override int GetHashCode()
		{
			return HashCode(this);
		}

		private int HashCode(FormatType_Class obj)
		{

			return (obj.Class.Class + "|" + obj.FormatType.FormatTypeCode).GetHashCode();
		}
	}
}