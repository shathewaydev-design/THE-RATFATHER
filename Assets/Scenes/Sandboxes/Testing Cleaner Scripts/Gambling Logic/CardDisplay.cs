using UnityEngine;
using UnityEngine.UI;
using static BlackJack;

public class CardDisplay : MonoBehaviour
{

    [SerializeField] RawImage cardIMG;

    public void SetCard(Card card)
    {
        // check if hidden dealer card???
        if (card == BlackJack.Instance.GetHiddenCard())
        {
            cardIMG.texture = Resources.Load<Texture>("cards/BACK");
            return;
        }


        string imageName = card.ToString();
        Texture cardTexture = Resources.Load<Texture>("cards/" + imageName);
        cardIMG.texture = cardTexture;



    }

}
