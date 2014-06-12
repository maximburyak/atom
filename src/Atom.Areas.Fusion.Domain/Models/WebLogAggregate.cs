using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class WebLogAggregate
	{
		public virtual int Id { get; set; }
		public virtual string Target { get; set; }
		public virtual int AccessCount { get; set; }
		public virtual DateTime FirstAccessed { get; set; }
		public virtual DateTime LastAccessed { get; set; }
		public virtual int ProcessingTime { get; set; }
		public virtual int AvgProcessingTime { get; set; }

		public WebLogAggregate()
		{
			
		}

		public WebLogAggregate(int id,
							   string target,
							   int accessCount,
							   DateTime firstAccessed,
							   DateTime lastAccessed, 
							   int processingTime,
							   int avgProcessingTime )
		{
			Id = id;
			Target = target;
			AccessCount = accessCount;
			FirstAccessed = firstAccessed;
			LastAccessed = lastAccessed;
			ProcessingTime = processingTime;
			AvgProcessingTime = avgProcessingTime;
		}	
					
	}		
}			