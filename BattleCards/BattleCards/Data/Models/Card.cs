using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static BattleCards.Data.DataConstants;

namespace BattleCards.Data.Models
{
    public class Card
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameCardMaxLength)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Keyword { get; set; }

        [Required]
        public int Attack { get; set; }

        [Required]
        public int Health { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public ICollection<UserCard> Users { get; set; } = new HashSet<UserCard>();
    }
}
