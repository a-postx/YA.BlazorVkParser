using Microsoft.AspNetCore.Components;

namespace YA.WebClient.Common
{
	public partial class LoadingBackground
	{
		[Parameter]
		public bool ShowContent { get; set; }

		[Parameter]
		public bool ShowBackgroundImage { get; set; }

		[Parameter]
		public bool ShowSpinner { get; set; }

		[Parameter]
		public bool ShowChildContent { get; set; }

		[Parameter]
		public RenderFragment ChildContent { get; set; }
	}
}