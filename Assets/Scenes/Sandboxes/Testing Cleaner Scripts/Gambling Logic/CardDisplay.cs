using UnityEngine;
using UnityEngine.UI;
using static BlackJack;

public class CardDisplay : MonoBehaviour
{
    // CARDS SHOULD HAVE 1/1.4 RATIO
    [SerializeField] RawImage cardIMG;

    public void SetCard(Card card)
    {
        string imageName = card.ToString();
        Texture cardTexture = Resources.Load<Texture>("cards/" + imageName);

        // check if hidden dealer card???
        if (card == BlackJack.Instance.GetHiddenCard())
        {
            if (!BlackJack.Instance.stayButton.enabled)
            {
                cardIMG.texture = cardTexture;
                return;
            }
            cardIMG.texture = Resources.Load<Texture>("cards/BACK");
            return;
        }

        //string imageName = card.ToString();
        //Texture cardTexture = Resources.Load<Texture>("cards/" + imageName);
        cardIMG.texture = cardTexture;

    }

    public void SetPokerCard(Poker.Card card, bool isOpponentCard, bool showDown = false)
    {
        string imageName = card.ToString();
        Texture cardTexture = Resources.Load<Texture>("cards/" + imageName);

        if (isOpponentCard)
        {
            // check if hand is over
            if (showDown)
            {
                cardIMG.texture = cardTexture;
                return;

            }

            cardIMG.texture = Resources.Load<Texture>("cards/BACK");
            return;
        }

        cardIMG.texture = cardTexture;

    }

}
