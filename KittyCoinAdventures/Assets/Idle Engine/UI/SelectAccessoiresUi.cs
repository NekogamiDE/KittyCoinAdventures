using System;
using System.Collections;
using System.Collections.Generic;
using IdleEngine.UserInterface;
using UnityEngine;
using UnityEngine.UI;

public class SelectAccessoiresUi : MonoBehaviour
{
    public Image AccessoireImage;
    public Image Border;
    private int index;
    private bool selected;

    public int GetIndex()
    {
        return this.index;
    }

    private Action<int> selectionChange;

    public void CreateAccessoire(Action<int> OnAcessoireSelect, Sprite image, int index)
    {
        selectionChange = OnAcessoireSelect;
        AccessoireImage.sprite = image;
        this.index = index;
        ResetSelection();
    }

    public void ResetSelection()
    {
        this.selected = false;
        Border.color = Color.white;
    }

    public void ToggleSelection()
    {
        this.selected = !this.selected;
        if (this.selected)
        {
            Border.color = Color.green;
            selectionChange(this.index);
        }
        else
        {
            Border.color = Color.white;
            selectionChange(-1);
        }
    }
}
