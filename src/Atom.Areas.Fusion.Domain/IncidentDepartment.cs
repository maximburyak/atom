using Atom.Areas.Fusion.Domain.Models;

namespace Atom.Areas.Fusion.Domain
{
    public class IncidentDepartment
    {
        public virtual Area Area { get; set; }
        public virtual Location Location { get; set; }
        public virtual HandlingDepartment HandlingDepartment { get; set; }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode().Equals(GetHashCode());
        }
        public override int GetHashCode()
        {
            return HashCode(this);
        }

        private int HashCode(IncidentDepartment obj)
        {
            return obj.Area.GetHashCode() ^  obj.HandlingDepartment.GetHashCode();
        }
    }
}