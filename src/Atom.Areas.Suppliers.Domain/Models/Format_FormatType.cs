
namespace Atom.Areas.Suppliers.Domain.Models
{
	public class Format_FormatType
	{
		public virtual Format Format { get; set; }
		public virtual FormatType FormatType { get; set; }

		public override bool Equals(object obj)
		{
			return obj.GetHashCode().Equals(GetHashCode());
		}
		public override int GetHashCode()
		{
			return HashCode(this);
		}

		private int HashCode(Format_FormatType obj)
		{

			return (Format.FormatCode + "|" + obj.FormatType.FormatTypeCode).GetHashCode();
		}

	}
}
