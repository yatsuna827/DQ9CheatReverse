using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using DQ9TreasureMap;


using static System.Console;
using static DQ9TreasureMap.GenerateTreasureMap;

//var map = GenerateTreasureMap.GenerateMetadata(0xF972 - 0x802, 99 + 5 * 5 + 87); //GenerateTreasureMap.Get(0x0E5C, 0xB5);
//WriteLine(map);

var name = "怒れる空の世界";
var level = 53;
var location = 0x1f;
var target = $"{name} Lv.{level} {location:X2}";

for (uint seed = 0x0u; seed < 0x10000; seed++)
{
    var map = GenerateTreasureMap.GenerateMetadata(seed, 99 + 5 * 5 + 1);
    if (map == target)
    {
        WriteLine($"{seed:X4}");
    }
}


//var info = map.CreateDungeonDetails();

//info.PrintFloor();

// info.TakeSnapShot();


static class Debug
{
    public static void PrintFloor(this byte[][] info)
    {
        string[] tile = ["□", "■", "□", "■", "△", "▽", "◇", "■", "□"];
        for (int f = 0; f < 15; f++)
        {
            WriteLine($"{f + 1}F");
            var floor = info[f].FloorMap();
            for (int y = 0; y < info[f][3]; y++)
            {
                var line = floor.Slice(y << 4, info[f][2]).ToArray();
                WriteLine(string.Join("", line.Select(_ => tile[_])));
            }
        }
    }

    public static void TakeSnapShot(this byte[][] info)
    {
        var hash = BitConverter.ToString(SHA256.HashData(info.SelectMany(_ => _).ToArray()));
        WriteLine(hash);
        WriteLine(hash == SNAP_SHOT_Z);
    }

    const string SNAP_SHOT = "5D-0F-48-CD-F0-46-EF-7C-57-8B-6D-C8-AC-E9-5C-AB-16-6C-FA-B2-C4-B0-80-E3-8A-CF-88-CB-21-8A-27-88";
    const string SNAP_SHOT2 = "6F-54-99-4C-F6-D1-F9-68-CE-BA-82-D6-EC-3B-33-77-FB-54-B7-D5-71-53-1C-F2-DB-6B-8C-12-4F-2F-72-A2";
    const string SNAP_SHOT_Z = "76-E8-35-ED-14-28-FB-17-84-B9-7A-DB-24-99-EC-3F-BD-5C-2F-E7-99-AC-FB-8C-7F-9A-D2-D3-2E-73-7D-94";
}
