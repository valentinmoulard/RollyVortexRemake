using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    void Tap(Vector3 cursorPosition);
    void TapBegin(Vector3 cursorPosition);
    void Swipe(Vector3 cursorPosition);
    void Hold(Vector3 cursorPosition);
    void Release(Vector3 cursorPosition);
}
