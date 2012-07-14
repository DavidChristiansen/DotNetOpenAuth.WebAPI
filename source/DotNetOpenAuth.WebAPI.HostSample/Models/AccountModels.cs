using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DotNetOpenAuth.WebAPI.HostSample.Models {
    public class LogOnModel {
		[Required]
		[DisplayName("OpenID")]
		public string UserSuppliedIdentifier { get; set; }

		[DisplayName("Remember me?")]
		public bool RememberMe { get; set; }
	}
}
