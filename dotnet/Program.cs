using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Unicode;
using DQ9TreasureMap;


using static System.Console;
using static DQ9TreasureMap.GenerateTreasureMap;

var mapSeed = 0x0E5Cu;
var rank = 7;
var floors = 15;

var info = CreateDungeonDetails(mapSeed, floors, rank, (int)MapType.洞窟);

// info.PrintFloor();

info.TakeSnapShot();

enum MapType
{
    洞窟 = 1,
    遺跡 = 2,
    氷 = 3,
    水 = 4,
    火山 = 5
}

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
    const string SNAP_SHOT_Z = "E7-61-50-F1-2B-5C-B1-2A-E5-4A-90-81-45-2E-C5-D8-CC-AE-EF-83-0A-CA-CB-56-E0-F7-06-CE-8F-73-51-57";
}
