using System.Collections;

namespace Meteor
{
	public class UpdatedMessage : Message
	{
		[JsonFx.Json.JsonIgnore]
		public const string updated = "updated";

		public string[] methods = null;
		public UpdatedMessage ()
		{
			msg = updated;
		}
	}
}

