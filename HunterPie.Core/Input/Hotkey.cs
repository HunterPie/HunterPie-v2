using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Input;

// TODO: Refactor this because this is HunterPie v1 code and I hate it
public class Hotkey
{
    private const int WM_HOTKEY = 0x0312;

    private static readonly Dictionary<int, Action> hotkeys = new();
    internal static IntPtr hWnd;

    /// <summary>
    /// All hotkeys registered in HunterPie's environment
    /// </summary>
    public static IReadOnlyDictionary<int, Action> Hotkeys => hotkeys;

    internal static void Unload()
    {
        while (hotkeys.Keys.Count > 0)
        {
            _ = Unregister(hotkeys.Keys.ElementAt(0));
        }
    }

#pragma warning disable IDE0060
    internal static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
#pragma warning restore IDE0060
    {
        if (msg == WM_HOTKEY)
        {
            int hotkeyId = wParam.ToInt32();
            if (Hotkeys.ContainsKey(hotkeyId))
            {
                Action callback = Hotkeys[hotkeyId];
                callback.Invoke();
            }
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// Registers a new global hotkey for HunterPie.
    /// </summary>
    /// <param name="keys">String with the hotkey (e.g: "Ctrl+Alt+O")</param>
    /// <param name="callback">The function HunterPie should invoke when the hotkey is triggered</param>
    /// <returns>The hotkey id, it should be used when unregistering the hotkey</returns>
    public static int Register(string keys, Action callback)
    {
        // Generates a new random Id for this hotkey to avoid collisions
        int newId;
        do
        {
            newId = new Random().Next(1, 1000);
        } while (Hotkeys.ContainsKey(newId));

        int[] hotkey = ParseStringToHotkeyCode(keys);
        if (hotkey is null)
        {
            Log.Error($"{keys} is not a valid hotkey.");
            return -1;
        }
        else
        {
            // Skip hotkeys which value is "None"
            if (hotkey[0] == 0 && hotkey[1] == 0)
            {
                return 0;
            }

            bool success = KeyboardHookHelper.RegisterHotKey(hWnd, newId, hotkey[0], hotkey[1]);

            if (success)
            {
                hotkeys[newId] = callback;
                return newId;
            }
            else
            {
                Log.Error($"Failed to register hotkey. {Marshal.GetLastWin32Error()}");
                return -1;
            }
        }
    }

    /// <summary>
    /// Unregisters a global hotkey from HunterPie.
    /// </summary>
    /// <param name="id">The id you got from <see cref="Register(string, Action)"/></param>
    /// <returns>True if the hotkey was unregistered successfully, false otherwise</returns>
    public static bool Unregister(int id)
    {
        if (!Hotkeys.ContainsKey(id))
        {
            Log.Info($"Failed to unregister hotkey with id: {id}. Hotkey not found!");
            return false;
        }

        bool success = KeyboardHookHelper.UnregisterHotKey(hWnd, id);

        if (success)
        {
            _ = hotkeys.Remove(id);
            return true;
        }
        else
        {
            Log.Error($"Failed to unregister hotkey. {Marshal.GetLastWin32Error()}");
            _ = hotkeys.Remove(id);
            return true;
        }
    }

    /// <summary>
    /// Parses a string from the "Ctrl+Alt+O" format to a array with modifier and key.
    /// </summary>
    /// <param name="keys">String in hotkey format. (e.g: "Ctrl+Alt+O")</param>
    /// <returns>If the parse was succesful, it will return a 2-length array where 0 is the modifier and 1 is the key.
    /// If the hotkey was "None", then it will return a 2-length array where both values are zero.
    /// If the parse fails, it will return null.</returns>
    public static int[] ParseStringToHotkeyCode(string keys)
    {
        if (keys == "None")
        {
            return new int[] { 0, 0 };
        }

        string[] Keys = keys.Split('+');
        // No-repeat
        int Modifier = 0x4000;

        int key = 0x0;
        foreach (string hKey in Keys)
        {
            switch (hKey)
            {
                case "Alt":
                    Modifier |= 0x0001;
                    break;
                case "Ctrl":
                    Modifier |= 0x0002;
                    break;
                case "Shift":
                    Modifier |= 0x0004;
                    break;
                default:
                    try
                    {
                        string parsedKey = CheckAndConvert(hKey);
                        key = (int)Enum.Parse(typeof(KeyboardHookHelper.KeyboardKeys), parsedKey);
                    }
                    catch
                    {
                        return null;
                    }

                    break;
            }
        }

        int[] parsedHotkey = new int[] { Modifier, key };
        return parsedHotkey;
    }

    private static string CheckAndConvert(string key)
    {
        string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        return numbers.Contains(key) ? $"D{key}" : key;
    }
}