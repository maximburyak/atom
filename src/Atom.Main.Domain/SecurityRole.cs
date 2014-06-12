namespace Atom.Main.Domain
{
	public class SecurityRole
	{
		public virtual string Id { get; set; }
		public virtual string Description { get; set; }

		public SecurityRole(string Id, string value)
		{
			this.Id = Id;
			this.Description = value;
		}

		/// <summary>
		/// provides uniformity for JQuery Json results to have the Text and Value named 
		/// the same for All drop-downs used by the same JQuery fillSelect function.
		/// </summary>
		#region

		public virtual string Text
		{
			get
			{
				return this.Description;
			}
		}

		public virtual string Value
		{
			get
			{
				return this.Description;
			}
		}

		#endregion
	}
}
