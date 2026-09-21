// ======================================================================
//  Engine.cs: TEACHER / ENGINE CODE. Students never need to open this.
//
//  Everything that touches Raylib lives in this one file, behind a small set
//  of plain functions (ints, floats and strings only). Main.cs calls these,
//  and Main.cs calls the students' functions. This file uses classes,
//  dictionaries and the rest on purpose, it is outside the course's five tools.
// ======================================================================
using System.Numerics;
using System.Text;
using Raylib_cs;

public class Sprite
{
    public bool Loaded;
    public Texture2D Texture;
    public int Size;          // width and height of one cell, in pixels of the source image
    public int Col;
    public int Row;
    public int Frames = 1;
    public int Gap;           // pixels between cells in a downloaded tileset (often 0 or 1)
    public int Margin;        // pixels around the outside edge of a tileset
    public bool Directional;  // rows are down, left, right, up
}

public class FloatingText
{
    public string Text;
    public float X, Y, Age;
    public Color Color;
}

// Everything the students' code prints with Console.WriteLine ends up here,
// so the game can show it in the message box. It still goes to the terminal too.
public class LogWriter : TextWriter
{
    private readonly TextWriter terminal;
    private readonly StringBuilder current = new StringBuilder();
    public readonly List<string> Lines = new List<string>();

    public LogWriter(TextWriter terminal) { this.terminal = terminal; }
    public override Encoding Encoding { get { return Encoding.UTF8; } }

    public override void Write(char value)
    {
        terminal.Write(value);
        if (value == '\n')
        {
            string line = current.ToString().TrimEnd('\r');
            if (line.Trim().Length > 0) Lines.Add(line);
            if (Lines.Count > 200) Lines.RemoveAt(0);
            current.Clear();
        }
        else current.Append(value);
    }
}

public static class Engine
{
    public const int ScreenW = 800;
    public const int ScreenH = 600;
    public static int CurrentLevel = 1;   // set by Main.cs; lets monster4.png etc. take over on level 4
    public static int Tile = 32;      // Main.cs picks 64, 48 or 32 to suit the size of each level
    public const int HudH = 64;
    public const int MsgH = 72;
    public const int ViewH = ScreenH - HudH - MsgH;

    static string assetsDir = "";
    static readonly Dictionary<string, string[]> settings = new Dictionary<string, string[]>();
    static readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    static readonly Dictionary<string, Sound> sounds = new Dictionary<string, Sound>();
    static readonly HashSet<string> missingSounds = new HashSet<string>();
    static readonly List<FloatingText> floats = new List<FloatingText>();
    static LogWriter log;
    static bool audioReady;
    static bool musicLoaded;
    static Music music;
    static string musicFile = "";
    static bool muted;
    static float clock;

    // ------------------------------------------------------------------ window

    public static void Open(string title)
    {
        log = new LogWriter(Console.Out);
        Console.SetOut(log);

        assetsDir = FindAssetsFolder();
        ReadSettings();

        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.InitWindow(ScreenW, ScreenH, title);
        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        audioReady = true;
    }

    public static bool Running()
    {
        return !Raylib.WindowShouldClose();
    }

    public static float Delta()
    {
        float dt = Raylib.GetFrameTime();
        if (dt > 0.05f) dt = 0.05f;
        clock += dt;
        return dt;
    }

    public static float Clock() { return clock; }

    public static void Close()
    {
        if (audioReady) Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    public static void BeginFrame()
    {
        if (musicLoaded) Raylib.UpdateMusicStream(music);
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Rgb(16, 14, 22, 255));
    }

    public static void EndFrame()
    {
        Raylib.EndDrawing();
    }

    // ------------------------------------------------------------------ input

    public static int HeldX()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) return -1;
        if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) return 1;
        return 0;
    }

    public static int HeldY()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W)) return -1;
        if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S)) return 1;
        return 0;
    }

    public static bool Pressed(string action)
    {
        if (action == "potion") return Raylib.IsKeyPressed(KeyboardKey.P);
        if (action == "leader") return Raylib.IsKeyPressed(KeyboardKey.Tab);
        if (action == "mute") return Raylib.IsKeyPressed(KeyboardKey.M);
        if (action == "restart") return Raylib.IsKeyPressed(KeyboardKey.R);
        if (action == "confirm") return Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space);
        if (action == "fire") return Raylib.IsKeyPressed(KeyboardKey.F);
        if (action == "throw") return Raylib.IsKeyPressed(KeyboardKey.T);
        if (action == "spell1" || action == "choice1") return Raylib.IsKeyPressed(KeyboardKey.One);
        if (action == "spell2" || action == "choice2") return Raylib.IsKeyPressed(KeyboardKey.Two);
        if (action == "spell3") return Raylib.IsKeyPressed(KeyboardKey.Three);
        if (action == "spell4") return Raylib.IsKeyPressed(KeyboardKey.Four);
        return false;
    }

    // ------------------------------------------------------------------ assets

    static string FindAssetsFolder()
    {
        string[] places =
        {
            Path.Combine(Directory.GetCurrentDirectory(), "..", "assets"),
            Path.Combine(Directory.GetCurrentDirectory(), "assets"),
            Path.Combine(AppContext.BaseDirectory, "assets"),
        };
        foreach (string place in places)
        {
            if (Directory.Exists(place)) return Path.GetFullPath(place);
        }
        return Path.GetFullPath(places[0]);
    }

    static void ReadSettings()
    {
        string path = Path.Combine(assetsDir, "assets.txt");
        if (!File.Exists(path)) return;
        foreach (string raw in File.ReadAllLines(path))
        {
            string line = raw.Trim();
            if (line.Length == 0 || line.StartsWith("#")) continue;
            int eq = line.IndexOf('=');
            if (eq < 1) continue;
            string name = line.Substring(0, eq).Trim().ToLowerInvariant();
            string[] words = line.Substring(eq + 1).Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0) settings[name] = words;
        }
    }

    static int Option(string[] words, string key, int fallback)
    {
        for (int i = 1; i + 1 < words.Length; i++)
        {
            int value;
            if (words[i].ToLowerInvariant() == key && int.TryParse(words[i + 1], out value)) return value;
        }
        return fallback;
    }

    static string FindFile(string name, string[] extensions)
    {
        if (settings.ContainsKey(name))
        {
            string chosen = Path.Combine(assetsDir, settings[name][0]);
            if (File.Exists(chosen)) return chosen;
            Console.Error.WriteLine("assets.txt: cannot find " + settings[name][0] + " for '" + name + "'");
        }
        foreach (string ext in extensions)
        {
            string path = Path.Combine(assetsDir, name + ext);
            if (File.Exists(path)) return path;
        }
        return null;
    }

    static Sprite GetSprite(string name)
    {
        name = name.ToLowerInvariant();
        if (sprites.ContainsKey(name)) return sprites[name];

        Sprite sprite = new Sprite();
        sprites[name] = sprite;
        string path = FindFile(name, new[] { ".png", ".jpg", ".jpeg", ".bmp" });
        if (path == null) return sprite;

        if (!textures.ContainsKey(path))
        {
            Texture2D loaded = Raylib.LoadTexture(path);
            Raylib.SetTextureFilter(loaded, TextureFilter.Point);
            textures[path] = loaded;
        }
        Texture2D texture = textures[path];
        if (texture.Width <= 0 || texture.Height <= 0) return sprite;

        sprite.Texture = texture;
        sprite.Loaded = true;
        int w = texture.Width;
        int h = texture.Height;
        string[] words = settings.ContainsKey(name) ? settings[name] : new[] { "" };
        bool described = words.Length > 1;

        if (described)
        {
            int directions = Option(words, "directions", 1);
            sprite.Directional = directions == 4;
            sprite.Size = Option(words, "size", sprite.Directional ? h / 4 : h);
            if (sprite.Size < 1) sprite.Size = h;
            sprite.Gap = Option(words, "gap", 0);
            sprite.Margin = Option(words, "margin", 0);
            sprite.Col = Option(words, "col", 0);
            sprite.Row = Option(words, "row", 0);
            bool wholeFile = Option(words, "col", -1) == -1 && Option(words, "row", -1) == -1;
            sprite.Frames = Option(words, "frames", wholeFile ? Math.Max(1, (w - 2 * sprite.Margin + sprite.Gap) / (sprite.Size + sprite.Gap)) : 1);
        }
        else if (w == h) { sprite.Size = w; }
        else if (w > h && w % h == 0) { sprite.Size = h; sprite.Frames = w / h; }
        else if (h % 4 == 0 && w % (h / 4) == 0) { sprite.Size = h / 4; sprite.Frames = w / (h / 4); sprite.Directional = true; }
        else { sprite.Size = 0; }   // odd shape: stretch the whole picture onto the tile
        if (sprite.Frames < 1) sprite.Frames = 1;
        return sprite;
    }

    // First choice first:  Pick("wall3", "wall")
    static Sprite Pick(string first, string second)
    {
        Sprite sprite = GetSprite(first);
        if (sprite.Loaded) return sprite;
        return GetSprite(second);
    }

    // This level's pictures win:  monster_ogre4, monster4, then monster_ogre, monster
    static Sprite PickMonster(string name)
    {
        string[] choices = { "monster_" + name + CurrentLevel, "monster" + CurrentLevel, "monster_" + name };
        foreach (string choice in choices)
        {
            Sprite sprite = GetSprite(choice);
            if (sprite.Loaded) return sprite;
        }
        return GetSprite("monster");
    }

    // ------------------------------------------------------------------ drawing

    static Color Rgb(int r, int g, int b, int a)
    {
        return new Color((byte)Math.Clamp(r, 0, 255), (byte)Math.Clamp(g, 0, 255), (byte)Math.Clamp(b, 0, 255), (byte)Math.Clamp(a, 0, 255));
    }

    static void DrawSpriteAt(Sprite sprite, float x, float y, int frame, int dir, float rotation, Color tint)
    {
        Rectangle source;
        if (sprite.Size <= 0) source = new Rectangle(0, 0, sprite.Texture.Width, sprite.Texture.Height);
        else
        {
            int col = sprite.Col + (frame % sprite.Frames);
            int row = sprite.Row + (sprite.Directional ? dir : 0);
            source = new Rectangle(sprite.Margin + col * (sprite.Size + sprite.Gap), sprite.Margin + row * (sprite.Size + sprite.Gap), sprite.Size, sprite.Size);
        }
        if (!sprite.Directional && dir == 1) source.Width = -source.Width;   // face left by mirroring
        Rectangle dest = new Rectangle(x + Tile / 2f, y + Tile / 2f, Tile, Tile);
        Raylib.DrawTexturePro(sprite.Texture, source, dest, new Vector2(Tile / 2f, Tile / 2f), rotation, tint);
    }

    // The boss: boss_warden3, boss3, boss_warden, boss. If none of those exist it is drawn as a monster.
    static Sprite PickBoss(string name)
    {
        string lower = name.ToLowerInvariant().Replace(" ", "");
        string[] choices = { "boss_" + lower + CurrentLevel, "boss" + CurrentLevel, "boss_" + lower, "boss" };
        foreach (string choice in choices)
        {
            Sprite sprite = GetSprite(choice);
            if (sprite.Loaded) return sprite;
        }
        return PickMonster(name);
    }

    // kind is "wall", "floor", "door", "loot", "npc" or "powerup". levelNumber lets wall2.png etc. take over.
    public static void DrawTile(string kind, int levelNumber, float x, float y)
    {
        Sprite sprite = Pick(kind + levelNumber, kind);
        if (sprite.Loaded) { DrawSpriteAt(sprite, x, y, (int)(clock * 4), 0, 0, Color.White); return; }

        int ix = (int)x, iy = (int)y;
        if (kind == "wall")
        {
            Raylib.DrawRectangle(ix, iy, Tile, Tile, Rgb(96, 90, 112, 255));
            Raylib.DrawRectangleLines(ix, iy, Tile, Tile, Rgb(40, 36, 48, 255));
            Raylib.DrawRectangle(ix, iy + Tile / 2, Tile, 2, Rgb(40, 36, 48, 255));
        }
        else if (kind == "floor")
        {
            Raylib.DrawRectangle(ix, iy, Tile, Tile, Rgb(92, 84, 74, 255));
            Raylib.DrawRectangleLines(ix, iy, Tile, Tile, Rgb(78, 71, 62, 255));
        }
        else if (kind == "door")
        {
            Raylib.DrawRectangle(ix + 4, iy + 2, Tile - 8, Tile - 4, Rgb(24, 20, 30, 255));
            Raylib.DrawRectangleLines(ix + 4, iy + 2, Tile - 8, Tile - 4, Rgb(240, 200, 90, 255));
        }
        else if (kind == "loot")
        {
            Raylib.DrawRectangle(ix + 6, iy + 10, Tile - 12, Tile - 16, Rgb(186, 122, 58, 255));
            Raylib.DrawRectangle(ix + 6, iy + 16, Tile - 12, 3, Rgb(246, 206, 84, 255));
        }
        else if (kind == "npc")
        {
            Raylib.DrawCircle(ix + Tile / 2, iy + Tile / 2, Tile / 2 - 4, Rgb(158, 104, 204, 255));
            Raylib.DrawCircle(ix + Tile / 2, iy + Tile / 2 - 3, Tile / 6, Rgb(244, 206, 170, 255));
        }
        else if (kind == "powerup")
        {
            int pulse = (int)(clock * 4) % 2;
            Raylib.DrawCircle(ix + Tile / 2, iy + Tile / 2, Tile / 4 + pulse * 2, Rgb(250, 214, 80, 255));
            Raylib.DrawCircle(ix + Tile / 2, iy + Tile / 2, Tile / 8, Color.White);
        }
    }

    // The boss is drawn one and a quarter tiles big, so it looks like a boss.
    public static void DrawBoss(string name, float x, float y, int frame, float flash)
    {
        Sprite sprite = PickBoss(name);
        Color tint = flash > 0 && ((int)(clock * 30) % 2 == 0) ? Rgb(255, 90, 90, 255) : Color.White;
        if (sprite.Loaded)
        {
            int keep = Tile;
            float grow = Tile * 0.125f;
            Tile = (int)(Tile * 1.25f);
            DrawSpriteAt(sprite, x - grow, y - grow * 2, frame, 0, 0, tint);
            Tile = keep;
            return;
        }
        Raylib.DrawCircle((int)x + Tile / 2, (int)y + Tile / 2, Tile / 2, flash > 0 ? tint : Rgb(176, 58, 66, 255));
        Raylib.DrawCircle((int)x + Tile / 2 - 5, (int)y + Tile / 2 - 3, 3, Rgb(255, 230, 90, 255));
        Raylib.DrawCircle((int)x + Tile / 2 + 5, (int)y + Tile / 2 - 3, 3, Rgb(255, 230, 90, 255));
    }

    // kind is "arrow", "stone" or "spell_spark" and so on. A spell with no picture of its own uses bolt.png.
    // angle 0 points right, 90 points down.
    public static void DrawProjectile(string kind, float x, float y, float angle)
    {
        Sprite sprite = Pick(kind + CurrentLevel, kind);
        if (!sprite.Loaded && kind.StartsWith("spell_")) sprite = Pick("bolt" + CurrentLevel, "bolt");
        if (sprite.Loaded) { DrawSpriteAt(sprite, x, y, (int)(clock * 10), 0, angle, Color.White); return; }
        Color color = kind == "arrow" ? Rgb(230, 234, 244, 255) : kind == "stone" ? Rgb(150, 150, 164, 255) : Rgb(120, 190, 255, 255);
        Raylib.DrawCircle((int)x + Tile / 2, (int)y + Tile / 2, Tile / 8 + 1, color);
    }

    public static void DrawBossBar(string name, int hp, int maxHp, int phase)
    {
        if (maxHp < 1) maxHp = 1;
        int width = 360, x = (ScreenW - width) / 2, y = HudH + 30;
        Raylib.DrawRectangle(x - 4, y - 4, width + 8, 30, Rgb(12, 10, 18, 210));
        Raylib.DrawRectangle(x, y + 12, width, 10, Rgb(50, 40, 60, 255));
        Color fill = phase == 3 ? Rgb(255, 120, 50, 255) : phase == 2 ? Rgb(236, 170, 60, 255) : Rgb(210, 60, 70, 255);
        Raylib.DrawRectangle(x, y + 12, width * Math.Clamp(hp, 0, maxHp) / maxHp, 10, fill);
        Raylib.DrawRectangle(x + width / 3, y + 12, 1, 10, Rgb(10, 10, 14, 255));
        Raylib.DrawRectangle(x + width * 2 / 3, y + 12, 1, 10, Rgb(10, 10, 14, 255));
        Raylib.DrawRectangleLines(x, y + 12, width, 10, Rgb(10, 10, 14, 255));
        string label = name.ToUpperInvariant() + (phase == 3 ? "   ENRAGED" : phase == 2 ? "   phase 2" : "");
        Raylib.DrawText(label, x, y - 2, 12, Rgb(255, 226, 120, 255));
    }

    static List<string> Wrap(string text, int size, int width)
    {
        List<string> lines = new List<string>();
        string line = "";
        foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            string attempt = line == "" ? word : line + " " + word;
            if (line != "" && Raylib.MeasureText(attempt, size) > width) { lines.Add(line); line = word; }
            else line = attempt;
        }
        if (line != "") lines.Add(line);
        return lines;
    }

    // The talking box. choiceA "" means there is nothing to choose: ENTER moves on.
    public static void DrawDialogue(string name, string text, string choiceA, string choiceB)
    {
        int x = 40, width = ScreenW - 80, height = 150, y = ScreenH - MsgH - height - 8;
        Raylib.DrawRectangle(x, y, width, height, Rgb(16, 14, 26, 240));
        Raylib.DrawRectangleLines(x, y, width, height, Rgb(240, 200, 90, 255));
        Raylib.DrawRectangle(x + 12, y - 12, Raylib.MeasureText(name, 16) + 16, 24, Rgb(240, 200, 90, 255));
        Raylib.DrawText(name, x + 20, y - 8, 16, Rgb(20, 16, 30, 255));
        int row = y + 22;
        foreach (string line in Wrap(text, 16, width - 32))
        {
            if (row > y + 78) break;
            Raylib.DrawText(line, x + 16, row, 16, Color.White);
            row += 20;
        }
        if (choiceA == "")
        {
            if ((int)(clock * 2) % 2 == 0) Raylib.DrawText("ENTER", x + width - 70, y + height - 24, 14, Rgb(240, 200, 90, 255));
        }
        else
        {
            Raylib.DrawText("1   " + choiceA, x + 16, y + height - 50, 16, Rgb(170, 220, 255, 255));
            if (choiceB != "") Raylib.DrawText("2   " + choiceB, x + 16, y + height - 28, 16, Rgb(170, 220, 255, 255));
        }
    }

    // Mana, and the powerups that are switched on. They live at the right of the message box.
    public static void DrawMana(int mana, int maxMana)
    {
        if (maxMana < 1) maxMana = 1;
        int x = ScreenW - 214, y = ScreenH - MsgH + 8;
        Raylib.DrawText("MANA", x, y, 12, Rgb(150, 200, 255, 255));
        for (int i = 0; i < maxMana && i < 12; i++)
            Raylib.DrawRectangle(x + 44 + i * 13, y, 10, 12, i < mana ? Rgb(90, 170, 255, 255) : Rgb(40, 50, 80, 255));
        Raylib.DrawText(mana + "/" + maxMana, x + 44 + Math.Min(maxMana, 12) * 13 + 4, y, 12, Color.White);
    }

    public static void DrawPowerups(List<string> names, List<int> turns)
    {
        int x = ScreenW - 214, y = ScreenH - MsgH + 26;
        for (int i = 0; i < names.Count && i < 3; i++)
            Raylib.DrawText("* " + names[i] + " " + turns[i], x, y + i * 13, 12, Rgb(250, 214, 80, 255));
    }

    // One line under the map: which number key casts what.
    public static void DrawSpellKeys(List<string> names, List<int> costs, int mana)
    {
        int x = 12, y = HudH + 6;
        for (int i = 0; i < names.Count && i < 4; i++)
        {
            string label = (i + 1) + " " + names[i] + " (" + costs[i] + ")";
            Color color = mana >= costs[i] ? Rgb(170, 220, 255, 255) : Rgb(90, 96, 120, 255);
            Raylib.DrawRectangle(x - 3, y - 2, Raylib.MeasureText(label, 12) + 6, 16, Rgb(12, 10, 18, 180));
            Raylib.DrawText(label, x, y, 12, color);
            x += Raylib.MeasureText(label, 12) + 14;
        }
    }

    // name is "player", or a monster's name such as "Ogre". flash is 0..1 (white-hot when hit).
    public static void DrawActor(string name, float x, float y, int frame, int dir, float flash)
    {
        bool isPlayer = name == "player";
        Sprite sprite = isPlayer ? Pick("player" + CurrentLevel, "player") : PickMonster(name);
        Color tint = flash > 0 && ((int)(clock * 30) % 2 == 0) ? Rgb(255, 90, 90, 255) : Color.White;
        if (sprite.Loaded) { DrawSpriteAt(sprite, x, y, frame, dir, 0, tint); return; }

        Color body = isPlayer ? Rgb(92, 136, 230, 255) : Rgb(110, 190, 70, 255);
        if (flash > 0) body = tint;
        Raylib.DrawCircle((int)x + Tile / 2, (int)y + Tile / 2 + (frame % 2), Tile / 2 - 4, body);
        Raylib.DrawCircle((int)x + Tile / 2 - 4, (int)y + Tile / 2 - 2, 2, Color.Black);
        Raylib.DrawCircle((int)x + Tile / 2 + 4, (int)y + Tile / 2 - 2, 2, Color.Black);
    }

    // angle 0 points right, 90 points down.
    public static void DrawSword(float x, float y, float angle)
    {
        Sprite sprite = Pick("sword" + CurrentLevel, "sword");
        if (sprite.Loaded) { DrawSpriteAt(sprite, x, y, 0, 0, angle, Color.White); return; }
        float rad = angle * MathF.PI / 180f;
        float cx = x + Tile / 2f + MathF.Cos(rad) * 8, cy = y + Tile / 2f + MathF.Sin(rad) * 8;
        Raylib.DrawCircle((int)cx, (int)cy, 4, Color.White);
        Raylib.DrawCircle((int)(cx - MathF.Cos(rad) * 8), (int)(cy - MathF.Sin(rad) * 8), 3, Color.White);
    }

    public static void DrawSmallBar(float x, float y, int value, int max)
    {
        if (max < 1) max = 1;
        int width = Tile - 6;
        Raylib.DrawRectangle((int)x + 3, (int)y - 5, width, 4, Rgb(20, 20, 20, 220));
        Raylib.DrawRectangle((int)x + 3, (int)y - 5, width * Math.Clamp(value, 0, max) / max, 4, Rgb(230, 70, 70, 255));
    }

    public static void Shade(int alpha)
    {
        Raylib.DrawRectangle(0, 0, ScreenW, ScreenH, Rgb(0, 0, 0, alpha));
    }

    public static void Text(string text, int x, int y, int size, int r, int g, int b)
    {
        Raylib.DrawText(text, x, y, size, Rgb(r, g, b, 255));
    }

    public static void TextCentered(string text, int y, int size, int r, int g, int b)
    {
        Raylib.DrawText(text, (ScreenW - Raylib.MeasureText(text, size)) / 2, y, size, Rgb(r, g, b, 255));
    }

    // ------------------------------------------------------------------ HUD

    public static void DrawHudBackground()
    {
        Raylib.DrawRectangle(0, 0, ScreenW, HudH, Rgb(24, 20, 34, 255));
        Raylib.DrawRectangle(0, HudH - 2, ScreenW, 2, Rgb(90, 80, 120, 255));
        Raylib.DrawRectangle(0, ScreenH - MsgH, ScreenW, MsgH, Rgb(24, 20, 34, 255));
        Raylib.DrawRectangle(0, ScreenH - MsgH, ScreenW, 2, Rgb(90, 80, 120, 255));
    }

    public static void DrawPartyMember(int slot, string name, int hp, int maxHp, bool isLeader)
    {
        int x = 10 + slot * 132;
        if (x > ScreenW - 270) return;      // no room for more; the rest are still in the party
        if (name.Length > 10) name = name.Substring(0, 10);
        Color nameColor = hp <= 0 ? Rgb(110, 100, 120, 255) : isLeader ? Rgb(255, 226, 120, 255) : Rgb(220, 220, 235, 255);
        Raylib.DrawText((isLeader ? "> " : "  ") + name, x, 10, 16, nameColor);
        if (maxHp < 1) maxHp = 1;
        Raylib.DrawRectangle(x, 34, 120, 12, Rgb(50, 40, 60, 255));
        Raylib.DrawRectangle(x, 34, 120 * Math.Clamp(hp, 0, maxHp) / maxHp, 12, hp * 3 <= maxHp ? Rgb(230, 70, 70, 255) : Rgb(96, 200, 110, 255));
        Raylib.DrawRectangleLines(x, 34, 120, 12, Rgb(10, 10, 14, 255));
        Raylib.DrawText(hp + "/" + maxHp, x + 44, 35, 10, Color.White);
    }

    public static void DrawCounters(int levelNumber, int potions, int shields, int gold, int arrows, int stones)
    {
        int x = ScreenW - 250;
        Raylib.DrawText("LEVEL " + levelNumber, x, 8, 16, Rgb(180, 170, 210, 255));
        Raylib.DrawText("Potions " + potions + "   Shields " + shields + "   Gold " + gold, x, 28, 14, Rgb(240, 226, 170, 255));
        Raylib.DrawText("Arrows " + arrows + "   Stones " + stones, x, 45, 14, Rgb(200, 210, 230, 255));
    }

    public static void DrawMessages()
    {
        int shown = Math.Min(3, log.Lines.Count);
        for (int i = 0; i < shown; i++)
        {
            string line = log.Lines[log.Lines.Count - shown + i];
            int shade = i == shown - 1 ? 255 : 150;
            Raylib.DrawText(line, 12, ScreenH - MsgH + 8 + i * 16, 14, Rgb(shade, shade, shade, 255));
        }
        Raylib.DrawText("Arrows/WASD move   walk into monsters or people   F arrow   T stone   1-4 spells   P potion   TAB leader   M music",
            12, ScreenH - 16, 10, Rgb(130, 120, 160, 255));
    }

    // ------------------------------------------------------------------ floating numbers

    public static void Float(string text, float x, float y, int r, int g, int b)
    {
        floats.Add(new FloatingText { Text = text, X = x, Y = y, Color = Rgb(r, g, b, 255) });
    }

    public static void DrawFloats(float dt, float offsetX, float offsetY)
    {
        for (int i = floats.Count - 1; i >= 0; i--)
        {
            FloatingText f = floats[i];
            f.Age += dt;
            if (f.Age > 0.9f) { floats.RemoveAt(i); continue; }
            int x = (int)(f.X + offsetX), y = (int)(f.Y + offsetY - f.Age * 36);
            Raylib.DrawText(f.Text, x + 1, y + 1, 16, Color.Black);
            Raylib.DrawText(f.Text, x, y, 16, f.Color);
        }
    }

    // ------------------------------------------------------------------ sound

    public static void Sfx(string name)
    {
        if (!audioReady || muted || missingSounds.Contains(name)) return;
        if (!sounds.ContainsKey(name))
        {
            string path = FindFile(name, new[] { ".wav", ".ogg", ".mp3" });
            if (path == null) { missingSounds.Add(name); return; }
            sounds[name] = Raylib.LoadSound(path);
        }
        Raylib.PlaySound(sounds[name]);
    }

    public static void PlayMusic(int levelNumber)
    {
        if (!audioReady) return;
        string[] extensions = { ".ogg", ".mp3", ".wav" };
        string path = FindFile("music" + levelNumber, extensions);
        if (path == null) path = FindFile("music", extensions);
        if (path == null || path == musicFile) return;

        if (musicLoaded) Raylib.StopMusicStream(music);
        music = Raylib.LoadMusicStream(path);
        musicFile = path;
        musicLoaded = true;
        Raylib.SetMusicVolume(music, muted ? 0f : 0.6f);
        Raylib.PlayMusicStream(music);
    }

    public static void ToggleMute()
    {
        muted = !muted;
        if (musicLoaded) Raylib.SetMusicVolume(music, muted ? 0f : 0.6f);
    }
}
