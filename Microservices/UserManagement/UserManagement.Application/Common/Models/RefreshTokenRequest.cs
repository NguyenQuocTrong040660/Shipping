using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserManagement.Application.Common.Models
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        [Required]
        public string RefreshToken { get; set; }
    }
}
