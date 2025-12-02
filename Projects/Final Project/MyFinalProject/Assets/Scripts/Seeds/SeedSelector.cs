using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedSelector : MonoBehaviour
{
    [Header("Available Seeds")]
    [SerializeField] private SeedPacket[] _availableSeeds;

    [Header("UI References")]
    [SerializeField] private Image _plantIcon;              // Icon child on Button_Plant
    [SerializeField] private TextMeshProUGUI _plantLabel;   // Text on Button_Plant

    [Header("Input")]
    [SerializeField] private KeyCode _nextSeedKey = KeyCode.E;
    [SerializeField] private KeyCode _prevSeedKey = KeyCode.Q;

    private int _currentIndex = 0;

    private void Start()
    {
        // Safety: no seeds, do nothing
        if (_availableSeeds == null || _availableSeeds.Length == 0)
        {
            Debug.LogWarning("[SeedSelector] No seeds assigned.");
            return;
        }

        ApplySelection();
    }

    private void Update()
    {
        // Simple cycling with Q / E (you can change keys)
        if (Input.GetKeyDown(_nextSeedKey))
        {
            NextSeed();
        }
        else if (Input.GetKeyDown(_prevSeedKey))
        {
            PreviousSeed();
        }
    }

    private void NextSeed()
    {
        if (_availableSeeds == null || _availableSeeds.Length == 0) return;

        _currentIndex++;
        if (_currentIndex >= _availableSeeds.Length)
            _currentIndex = 0;

        ApplySelection();
    }

    private void PreviousSeed()
    {
        if (_availableSeeds == null || _availableSeeds.Length == 0) return;

        _currentIndex--;
        if (_currentIndex < 0)
            _currentIndex = _availableSeeds.Length - 1;

        ApplySelection();
    }

    private void ApplySelection()
    {
        SeedPacket seed = _availableSeeds[_currentIndex];

        // 1) Tell the FarmingController which seed is active
        if (FarmingController.Instance != null)
        {
            FarmingController.Instance.SetSeed(seed);
        }
        else
        {
            Debug.LogWarning("[SeedSelector] No FarmingController.Instance found.");
        }

        // 2) Update the Plant button UI
        if (_plantIcon != null)
        {
            _plantIcon.sprite = seed.CoverImage;
            _plantIcon.enabled = (seed.CoverImage != null);
        }

        if (_plantLabel != null)
        {
            // e.g. "3 Plant (Onion)"
            _plantLabel.text = $"3 {seed.CropName}";
        }

        Debug.Log("[SeedSelector] Selected seed: " + seed.CropName);
    }
}
