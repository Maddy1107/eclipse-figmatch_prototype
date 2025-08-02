using UnityEngine;

public interface ICard
{
    int CardID { get; }
    void Setup(int id, Sprite frontSprite);
    void Flip();
    void FlipBack();
    void SetMatched();
    bool IsFlipped { get; }
    bool IsMatched { get; }
}
