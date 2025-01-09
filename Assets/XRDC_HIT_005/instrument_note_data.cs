using UnityEngine;

[System.Serializable]
public class InstrumentNoteData
{
    public bool hasNote;
    public float volume;
    public int pitchIndex;

    public InstrumentNoteData(bool hasNote = false, float volume = 0f, int pitchIndex = 0)
    {
        this.hasNote = hasNote;
        this.volume = volume;
        this.pitchIndex = pitchIndex;
    }
}

