using UnityEngine;

public class CropBlock : MonoBehaviour
{
    private enum CropState
    {
        Empty,
        Plowed,
        Planted,
        ReadyToHarvest
    }

    public Vector2Int Location { get; protected set; }

    [Header("Tilled Soil")]
    [SerializeField] private SpriteRenderer _plowedSoilSR;
    [SerializeField] private Sprite _plowedSoilIcon;

    [Header("Watered Soil")]
    [SerializeField] private SpriteRenderer _wateredSoilSR;
    [SerializeField] private Sprite _wateredSoilIcon;

    [Header("Crop Soil")]
    [SerializeField] private SpriteRenderer _cropSR;

    [Header("FX")]
    [SerializeField] private ParticleSystem _readyParticles;

    private SeedPacket.GrowthStage _currentStage;
    private SeedPacket _currentSeedPacket;

    private bool _isWatered = false;
    private bool _isWild = false;
    private bool _preventUse = false;

    private string _cropName = string.Empty;
    private string _tilemapName = string.Empty;

    private Validator _validator;
    private CropManager _cropController;

    private CropState _currentState = CropState.Empty;

    private void Start()
    {
        _validator = GetComponentInChildren<Validator>();

        if (_readyParticles != null)
        {
            _readyParticles.Stop();
            _readyParticles.Clear();
        }

        // ⭐ IMPORTANT – crops update once per day
        TimeManager.Instance.OnDayPassed.AddListener(NextDay);
    }

    public void Initialize(string tilemapName, Vector2Int location, CropManager cropController)
    {
        Location = location;
        _tilemapName = tilemapName;
        _cropController = cropController;
        _currentState = CropState.Empty;

        name = FormatName();
    }

    public void PreventUse() => _preventUse = true;

    public void PlowSoil()
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState != CropState.Empty) return;
        if (_preventUse) return;

        _currentState = CropState.Plowed;
        _plowedSoilSR.sprite = _plowedSoilIcon;
    }

    public void WaterSoil()
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState == CropState.Empty) return;
        if (_preventUse) return;

        if (_isWatered)
        {
            Debug.Log($"[CropBlock] Already watered {Location}");
            return;
        }

        _wateredSoilSR.sprite = _wateredSoilIcon;
        _isWatered = true;

        Debug.Log($"[CropBlock] WATERED at {Location}");
    }

    public void PlantSeed(SeedPacket seedPacket)
    {
        if (IsMissingRequiredComponents()) return;
        if (_currentState != CropState.Plowed) return;

        CreateCrop(seedPacket);
        UpdateCropImage();
    }

    private void CreateCrop(SeedPacket seedPacket)
    {
        _currentSeedPacket = Instantiate(seedPacket);
        _currentStage = SeedPacket.GrowthStage.Seed;
        _currentState = CropState.Planted;

        _cropName = _currentSeedPacket.CropName;
        name = FormatName();

        _cropController.AddToPlantedCrops(this);
    }

    private void GrowPlants()
    {
        if (_currentState != CropState.Planted) return;
        if (!_isWatered && !_isWild) return;
        if (_currentSeedPacket == null) return;

        // advance stage
        _currentStage++;

        // cap stage at Mature
        if (_currentStage >= SeedPacket.GrowthStage.Mature)
        {
            _currentStage = SeedPacket.GrowthStage.Mature;
            _currentState = CropState.ReadyToHarvest;

            if (_readyParticles != null)
                _readyParticles.Play();

            Debug.Log($"[CropBlock] READY TO HARVEST {Location}");
        }

        UpdateCropImage();
    }

    public void HarvestPlants()
    {
        if (_currentState != CropState.ReadyToHarvest) return;

        PickCrop();
        ResetSoil();
    }

    private void PickCrop()
    {
        if (_readyParticles != null)
        {
            _readyParticles.Stop();
            _readyParticles.Clear();
        }

        var crop = Instantiate(_currentSeedPacket.HarvestPrefab, transform.position, Quaternion.identity);
        crop.transform.SetParent(null);

        _cropController.RemoveFromPlantedCrops(Location);
    }

    private void UpdateCropImage()
    {
        if (_currentSeedPacket == null) return;
        _cropSR.sprite = _currentSeedPacket.GetIconForStage(_currentStage);
    }

    private void ResetSoil()
    {
        _currentState = CropState.Empty;
        _isWatered = false;
        _isWild = false;

        _plowedSoilSR.sprite = null;
        _wateredSoilSR.sprite = null;
        _cropSR.sprite = null;

        if (_readyParticles != null)
        {
            _readyParticles.Stop();
            _readyParticles.Clear();
        }

        name = FormatName();
    }

    public void TurnValidatorOn()
    {
        if (_validator == null || _preventUse) return;
        _validator.TurnValidatorOn();
    }

    public void TurnValidatorOff()
    {
        if (_validator == null) return;
        _validator.TurnValidatorOff();
    }

    private void NextDay()
    {
        if (_currentState == CropState.ReadyToHarvest) return;

        GrowPlants();

        // ⭐ RESET WATER EVERY DAY
        _isWatered = false;
        _wateredSoilSR.sprite = null;
    }

    private string FormatName() =>
        _currentState == CropState.Planted
        ? $"{_tilemapName}-{_cropName} [{Location.x},{Location.y}]"
        : $"{_tilemapName} [{Location.x},{Location.y}]";

    private bool IsMissingRequiredComponents()
    {
        return
            _plowedSoilSR == null ||
            _plowedSoilIcon == null ||
            _wateredSoilSR == null ||
            _wateredSoilIcon == null;
    }
}
