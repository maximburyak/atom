using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Atom.Areas.Fusion.Domain.Models
{
	public class WebLogAnalysis
	{
		public virtual int Id { get; set; }

		[DisplayName("Domain")]
		public virtual string Website { get; set; }

		[DisplayName("Web Path")]
		public virtual string WebPath { get; set; }

		[DisplayName("File Path")]
		public virtual string Path { get; set; }

		[DisplayName("Filename")]
		public virtual string FileName { get; set; }

		[DisplayName("Ext")]
		public virtual string Extension { get; set; }

		[DisplayName("Accesses")]
		public virtual int AccessCount { get; set; }

		[DisplayName("First Accessed")]
		public virtual DateTime FirstAccessed { get; set; }

		[DisplayName("Last Accessed")]
		public virtual DateTime LastAccessed { get; set; }

		[DisplayName("Time")]
		public virtual int ProcessingTime { get; set; }

		[DisplayName("Avg Time")]
		public virtual int AvgProcessingTime { get
			{
				return (AccessCount == 0)
			       		? 0
			       		: ProcessingTime/AccessCount;
			}
			set { }
		}


	}
}
