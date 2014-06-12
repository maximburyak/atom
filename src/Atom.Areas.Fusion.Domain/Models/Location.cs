namespace Atom.Areas.Fusion.Domain.Models
{
    public class Location
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Enabled { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var loc=obj as Location;

            if (loc == null)
                return false;

            return loc.GetHashCode() == this.GetHashCode();
        }
        public override int GetHashCode()
        {
            return Id;
        }

    }
}