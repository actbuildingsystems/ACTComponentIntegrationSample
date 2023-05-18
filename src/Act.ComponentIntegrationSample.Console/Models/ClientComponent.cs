namespace Act.ComponentIntegrationSample.Console.Models;

public class ClientComponent
{
    public int? Id { get; set; }
    public List<ColorItem> AvailableColors { get; set; }	
    public List<string> AvailableRegions { get; set; }	
    public string? BendAngleDegrees1Range { get; set; }	
    public string? BendAngleDegrees2Range { get; set; }	
    public double CostPerPunchEach { get; set; }	
    public double CostPerPunchSetup { get; set; }	
    public double CostPerScoreEach { get; set; }	
    public double CostPerUnit { get; set; }	
    public string? DistributorInfo { get; set; }	
    public double MaxLength { get; set; }	
    public double MinLength { get; set; }	
    public double MinLengthShortCuttingCharge { get; set; }	
    public bool PieceMarkAllowed { get; set; }	
    public string? ProductCode { get; set; }	
    public string? ProductCodeSecondary { get; set; }	
    public int ProductId { get; set; }	
    public int PunchingAllowed { get; set; }	
    public bool ScoringAllowed { get; set; }	
    public double ShortCuttingCharge { get; set; }	
    public double StandardLength { get; set; }	
    public int StandardQuantity { get; set; }	
    public string? StockLengths { get; set; }	
    public string? Supplier { get; set; }	
    public string? SupplierDescription { get; set; }	
    public string? SupplierInternal { get; set; }	
    public string? Unit { get; set; }	
    public string? VariationDescription { get; set; }	
    public double WeightPerUnit { get; set; }

    public override string ToString()
    {
        return $"id: {Id} - productId: {ProductId} - productCode: {ProductCode} - costPerUnit: {CostPerUnit}";
    }
}    