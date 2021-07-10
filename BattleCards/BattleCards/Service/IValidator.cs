using BattleCards.Models.Cards;
using BattleCards.Models.Users;
using System.Collections.Generic;

namespace BattleCards.Service
{
    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateCard(AddCardFormModel model);
    }
}
