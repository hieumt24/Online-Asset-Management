using AssetManagement.Domain.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Domain.Entites
{
    public class BlackListToken : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}