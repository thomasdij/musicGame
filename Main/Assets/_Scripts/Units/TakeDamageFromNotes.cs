using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFromNotes : MonoBehaviour
{
    public DamagableCharacter Damagable;
    bool _damageUpdating;
    NoteModel _currentNoteModel;
    float _targetResistance;
    Queue<float> _shield = new Queue<float>();
    [SerializeField] int _shieldLength = 64; // Length of time in 16th notes that the shield takes as its rolling average
    [SerializeField] float _shieldStrength = 4f; // The value that needs to be exceeded to pierce through the shield and cause health damage

    // Initialize character stats
    void Start()
    {
        Damagable = GetComponent<DamagableCharacter>();
        _damageUpdating = false;
        _targetResistance = 1.0f;
        ChangeSpriteColor(0f);
        for (int i = 0; i < _shieldLength - 1; i++)
        {
            _shield.Enqueue(0f);
        }
    }

    void OnEnable()
    {
        ManageMeasureProgress.OnNew16thNote += UpdateShield;
    }

    void OnDisable()
    {
        ManageMeasureProgress.OnNew16thNote -= UpdateShield;
    }

    // Update shield queue to rolling average over past _shieldLength
    void UpdateShield()
    {
        float shieldDamage;
        // if being effected by note, take effect from note
        if (_damageUpdating)
        {
            shieldDamage = _currentNoteModel.CalcDamage(_targetResistance, _currentNoteModel);
            // find how much, if any damage gets through the shield
            float healthDamage = Shield(shieldDamage);
            // apply any damage that gets through shield
            Damagable.TakeDamageFromNotes(healthDamage);
        }
        // else, update shield with value of zero, showing no note occuring on the shield for this 16th note
        else
        {
            shieldDamage = 0f;
            // update shield
            Shield(shieldDamage);
        }

    }

    // Get NoteModel from attacking character   
    void OnTriggerStay2D(Collider2D other)
    {
        // if this object is inside the trigger of an object that is currently generating notes at it
        if (other.gameObject.TryGetComponent(out NoteGenerator othNoteGen))
        {
            _damageUpdating = othNoteGen.MelodyPlaying;
            _currentNoteModel = othNoteGen.CurrentNoteModel;
        }
        else
        {
            _damageUpdating = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        _damageUpdating = false;
    }

    float Shield(float shieldDamage)
    {
        _shield.Enqueue(shieldDamage);
        if (_shield.Count > 48)
        {
            _shield.Dequeue();
        }
        float sum = 0f;
        foreach (float value in _shield)
        {
            sum += value;
        }
        float shieldPierce = sum / _shieldLength;
        UpdateColorFromShield(shieldPierce);
        float healthDamage;
        if (shieldPierce < _shieldStrength)
        {
            healthDamage = 0f;
        }
        else
        {
            healthDamage = shieldPierce - _shieldStrength;
        }
        return healthDamage;
    }

    void UpdateColorFromShield(float shieldPierce)
    {
        float redAmount;
        if (shieldPierce < _shieldStrength)
        {
            redAmount = shieldPierce / _shieldStrength;
        }
        else
        {
            redAmount = 1f;
        }
        ChangeSpriteColor(redAmount);
    }

    void ChangeSpriteColor(float colorValue)
    {
        Color spriteColor = GetComponent<SpriteRenderer>().color;
        spriteColor.r = colorValue;
        GetComponent<SpriteRenderer>().color = spriteColor;
    }
}
