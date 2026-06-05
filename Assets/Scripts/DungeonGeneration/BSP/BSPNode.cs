using UnityEngine;

/// <summary>
/// Repräsentiert einen Knoten im BSP-Baum.
/// Jeder Knoten speichert einen rechteckigen Bereich
/// des Dungeons sowie seine möglichen Kindknoten.
/// </summary>
public class BSPNode
{
    // Der Bereich des Dungeons, den dieser Knoten repräsentiert.
    public RectInt Area;

    // Linker und rechter Teilbereich nach einer Aufteilung.
    public BSPNode LeftChild;
    public BSPNode RightChild;

    // Der Raum, der in diesem Knoten erstellt wird (null, wenn noch keiner erstellt wurde).
    public RectInt? Room;

    //Erstellt einen neuen BSP-Knoten für den angegebenen Bereich.
    public BSPNode(RectInt area)
    {
        Area = area;
    }
    // Überprüft, ob dieser Knoten ein Blatt ist (d.h. keine Kinder hat).
    public bool IsLeaf()
    {
        return LeftChild == null && RightChild == null;
    }
}
