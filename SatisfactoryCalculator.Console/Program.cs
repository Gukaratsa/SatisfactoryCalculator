Dictionary<string, Infrastructure> _infrastuctures = new()
{
    { "Miner", new Infrastructure("Miner") },
    { "Smelter", new Infrastructure("Smelter") },
    { "Constructor", new Infrastructure("Constructor") },
    { "Manufactor", new Infrastructure("Manufator") }
};
Dictionary<string, Material> _materials = new()
{
    { "IronOre", new ("IronOre", "IOre") },
    { "IronIngot", new ("IronIngot", "IIngot") },
    { "Coal", new ("Coal", "Coal") },
    { "SteelIngot", new ("SteelIngot", "SIngot") },
    { "IronRod", new ("IronRod", "IRod") },
    { "IronPlate", new ("IronPlate", "IPlate") },
    { "Screw", new ("Screw", "Screw") },
    { "RefinedIronPlate", new ("RefinedIronPlate", "RPlate") },
    { "ModularFrame", new ("ModularFrame", "MFrame") },
    { "Rotor", new ("Rotor", "Rotot") },
    { "SmartPlating", new ("SmartPlating", "SPlating") },
    { "Stator", new ("Stator", "Stator")  },
    { "CopperOre", new ("CopperOre", "COre")  },
    { "CopperIngot", new ("CopperIngot", "CIngot")  },
    { "Wire", new ("Wire", "Wire")  },
    { "Cable", new ("Cable", "Cable")  },
    { "SteelBeam", new ("SteelBeam", "SBeam") },
    { "SteelPipe", new ("SteelPipe", "SPipe") },
    { "VersatileFrame", new ("VersatileFrame", "VFrame") },
    { "AutomatedWire", new ("AutomatedWire", "AWire") }
};
List<Recipe> _recipes = new()
{
    new([], "Miner", new(270, "IronOre")),
    new([], "Miner", new(270, "CopperOre")),
    new([new(30, "IronOre")], "Smelter", new(30, "IronIngot")),
    new([new(30, "CopperOre")], "Smelter", new(30, "CopperIngot")),
    new([new(15, "CopperIngot")], "Manufactor", new(30, "Wire")),
    new([new(60, "Wire")], "Manufactor", new(30, "Cable")),
    new([new(15, "IronIngot")], "Smelter", new(15, "IronRod")),
    new([new(30, "IronIngot")], "Smelter", new(20, "IronPlate")),
    new([new(10, "IronRod")], "Smelter", new(40, "Screw")),
    new([new(60, "Screw"), new(30, "IronPlate")], "Manufactor", new(5, "RefinedIronPlate")),
    new([new(3, "RefinedIronPlate"), new(12, "IronRod")], "Manufactor", new(2, "ModularFrame")),
    new([new(100, "Screw"), new(20, "IronRod")], "Manufactor", new(4, "Rotor")),
    new([new(2, "RefinedIronPlate"), new(2, "Rotor")], "Manufactor", new(2, "SmartPlating")),
    new([new(2, "RefinedIronPlate"), new(2, "Rotor")], "Manufactor", new(2, "SteelPipe")),
    new([new(15, "SteelPipe"), new(40, "Wire")], "Manufactor", new(5, "Stator")),
};
List<TransportInfrastructure> _transportInfrastructures = new()
{
    new("Mark1", 60),
    new("Mark2", 120),
    new("Mark3", 270),
};

Dictionary<string, double> summary = [];
Print("SmartPlating", 60, "root", summary);

foreach(var summaryItem in summary)
{
    var material = _materials[summaryItem.Key];
    var recipe = _recipes.First(r => r.OutputMaterial.MaterialName == summaryItem.Key);
    var infrastructure = _infrastuctures[recipe.InfrastructureName];
    var infrastructureAmount = summaryItem.Value / recipe.OutputMaterial.Amount;
    var usedTransporter = GetTransportation(summaryItem.Value);
    Console.WriteLine($"{summaryItem.Key} - {summaryItem.Value} ({infrastructure.Name}:{infrastructureAmount}, {usedTransporter.TransporterName}:{usedTransporter.Amount})");
}

void Print(string product, double amount, string parent, Dictionary<string, double> summary, string indent = "")
{
    var material = _materials[product];
    var recipe = _recipes.First(r => r.OutputMaterial.MaterialName == product);    
    var infrastructure = _infrastuctures[recipe.InfrastructureName];
    var infrastructureAmount = amount / recipe.OutputMaterial.Amount;

    if (summary.ContainsKey(product))
        summary[product] += amount;
    else
        summary.Add(product, amount);

    var usedTransporter = GetTransportation(amount);

    Console.WriteLine($"{indent}{parent} - {product} uses {infrastructureAmount} {infrastructure.Name}s to produce {amount} ({usedTransporter.Amount} x {usedTransporter.TransporterName})");
    if(recipe.InputMaterials.Count > 0)
        indent += "-";
    foreach (var inputMaterial in recipe.InputMaterials)
    {
        Print(inputMaterial.MaterialName, infrastructureAmount * inputMaterial.Amount, material.ShortName, summary, indent);
    }
}

TransportInfrastructureAmount GetTransportation(double amount)
{
    var transporter = _transportInfrastructures.First();
    var transporterAmount = amount / transporter.TransferAmount;
    foreach (var transporterInfrastructure in _transportInfrastructures)
    {
        if (transporterAmount <= 1)
            break;

        transporter = transporterInfrastructure;
        transporterAmount = amount / transporter.TransferAmount;
    }
    return new(transporterAmount, transporter.Name);
}

record MaterialAmount(double Amount, string MaterialName);
record Material(string Name, string ShortName);
record TransportInfrastructure(string Name, double TransferAmount);
record TransportInfrastructureAmount(double Amount, string TransporterName);
record Infrastructure(string Name);

record Recipe(List<MaterialAmount> InputMaterials, string InfrastructureName, MaterialAmount OutputMaterial);
record Transport(Material From, Material To, TransportInfrastructure TransportInfrastructure);