using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameJam2
{
	class MainClass
	{
        //Jauges de Gestion
        static int money = 50;
        static int students = 50;
        static int mood = 50;

        //Get random event
        public static Events GetRandomEvent()
        {
            Events currentEvent;
            //get random event from list
            Random rnd = new Random();
            int rand = rnd.Next(0, 3);
            currentEvent = new Events(String.Format("{0}",rand));
            return currentEvent;
        }

        //Colored arrows + parse to check wich ones to show
        public static void ColoredArrowsDisplay(Events currentEvent, bool yes)
        {
            //▲▼
            string up1 = " ", up2 = " ", up3 = " ", down1 = " ", down2 = " ", down3 = " ";
            if(yes)
            {
                if (currentEvent.ImpactYesMoney > 0) { up1 = "▲"; } else if (currentEvent.ImpactYesMoney < 0) { down1 = "▼"; }
                if (currentEvent.ImpactYesStudents > 10) { up2 = "▲"; } else if (currentEvent.ImpactYesStudents < 0) { down2 = "▼"; }
                if (currentEvent.ImpactYesMood > 0) { up3 = "▲"; } else if (currentEvent.ImpactYesMood < 0) { down3 = "▼"; }
            }
            else if(!yes)
            {
                if (currentEvent.ImpactNoMoney > 0) { up1 = "▲"; } else if (currentEvent.ImpactNoMoney < 0) { down1 = "▼"; }
                if (currentEvent.ImpactNoStudents > 10) { up2 = "▲"; } else if (currentEvent.ImpactNoStudents < 0) { down2 = "▼"; }
                if (currentEvent.ImpactNoMood > 0) { up3 = "▲"; } else if (currentEvent.ImpactNoMood < 0) { down3 = "▼"; }
            }
            TraitementConsole.ChangeCouleur(ConsoleColor.Black, ConsoleColor.Green, false);
            Console.WriteLine("        {0}           {1}          {2}", up1, up2, up3);
            Console.ResetColor();
            Console.WriteLine("");
            TraitementConsole.ChangeCouleur(ConsoleColor.Black, ConsoleColor.Red, false);
            Console.WriteLine("        {0}           {1}          {2}", down1, down2, down3);
            Console.ResetColor();
        }

        //Afficher UI et infos
        public static void DisplayGame(Events currentEvent)
        {
            TraitementConsole.NettoieConsole();
            //Art contien yes no et le dessin au millieu
            TraitementConsole.AfficheAsciiArtFile(currentEvent.Art);

            //Texte de contexte explication
            Console.WriteLine(currentEvent.Text);

            Console.WriteLine("");

            //Ca c'est les stats 
            TraitementConsole.ChangeCouleur(ConsoleColor.Black, ConsoleColor.White, false);
            Console.WriteLine("Argent: {0}  Eleves: {1}  Moral: {2}", money, students, mood);
        }

        //Appliquer les cartes/events
        public static void ApplyEvent(Events currentEvent, bool yes)
        {
            //blabalalalala
            if (yes)
            {
                money += currentEvent.ImpactYesMoney;
                students += currentEvent.ImpactYesStudents;
                mood += currentEvent.ImpactYesMood;
            }
            else
            {
                money += currentEvent.ImpactNoMoney;
                students += currentEvent.ImpactNoStudents;
                mood += currentEvent.ImpactNoMood;
            }
        }

		public static void Main (string[] args)
		{
            Events currentEvent;

			//TraitementConsole.ChangeCouleur(ConsoleColor.DarkBlue, ConsoleColor.White, true);


            var handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Win32Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Win32Exception(); }

            var record = new NativeMethods.INPUT_RECORD();
            uint recordLen = 0;
            currentEvent = GetRandomEvent();
            DisplayGame(currentEvent);
            while (true)
            {
                if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Win32Exception(); }
                Console.SetCursorPosition(0, 46);
                // DEBUG TO SHOW MOUSE POS AND MOUSE STATE
                /*switch (record.EventType) {
                    case NativeMethods.MOUSE_EVENT: {
                            Console.WriteLine("Mouse event");
                            Console.WriteLine(string.Format("    X ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.X));
                            Console.WriteLine(string.Format("    Y ...............:   {0,4:0}  ", record.MouseEvent.dwMousePosition.Y));
                            Console.WriteLine(string.Format("    dwButtonState ...:   {0}  ", record.MouseEvent.dwButtonState));
                            Console.WriteLine(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.MouseEvent.dwControlKeyState));
                            Console.WriteLine(string.Format("    dwEventFlags ....: 0x{0:X4}  ", record.MouseEvent.dwEventFlags));
                        } break;

                    case NativeMethods.KEY_EVENT: {
                            Console.WriteLine("Key event  ");
                            Console.WriteLine(string.Format("    bKeyDown  .......:  {0,5}  ", record.KeyEvent.bKeyDown));
                            Console.WriteLine(string.Format("    wRepeatCount ....:   {0,4:0}  ", record.KeyEvent.wRepeatCount));
                            Console.WriteLine(string.Format("    wVirtualKeyCode .:   {0,4:0}  ", record.KeyEvent.wVirtualKeyCode));
                            Console.WriteLine(string.Format("    uChar ...........:      {0}  ", record.KeyEvent.UnicodeChar));
                            Console.WriteLine(string.Format("    dwControlKeyState: 0x{0:X4}  ", record.KeyEvent.dwControlKeyState));

                            if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
                        } break;
                }*/
                int temp = record.MouseEvent.dwButtonState;
                int X = record.MouseEvent.dwMousePosition.X;
                int Y = record.MouseEvent.dwMousePosition.Y;
                //Yes boutton
                if(X > 10 && X < 60 && Y > 2 && Y < 40)
                {
                    ColoredArrowsDisplay(currentEvent, true);
                    if (temp == 1)
                    {
                        ApplyEvent(currentEvent, true);
                        currentEvent = GetRandomEvent();
                        DisplayGame(currentEvent);
                    }
                }
                if (X > 130 && X < 190 && Y > 2 && Y < 40)
                {
                    ColoredArrowsDisplay(currentEvent, false);
                    if (temp == 1)
                    {
                        ApplyEvent(currentEvent, false);
                        currentEvent = GetRandomEvent();
                        DisplayGame(currentEvent);
                    }
                }
                if (record.KeyEvent.wVirtualKeyCode == (int)ConsoleKey.Escape) { return; }
            }
		}
	}

    //Class NativeMethods
    class NativeMethods 
    {
            public const Int32 STD_INPUT_HANDLE = -10;

            public const Int32 ENABLE_MOUSE_INPUT = 0x0010;
            public const Int32 ENABLE_QUICK_EDIT_MODE = 0x0040;
            public const Int32 ENABLE_EXTENDED_FLAGS = 0x0080;

            public const Int32 KEY_EVENT = 1;
            public const Int32 MOUSE_EVENT = 2;


            [DebuggerDisplay("EventType: {EventType}")]
            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT_RECORD
            {
                [FieldOffset(0)]
                public Int16 EventType;
                [FieldOffset(4)]
                public KEY_EVENT_RECORD KeyEvent;
                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;
            }

            [DebuggerDisplay("{dwMousePosition.X}, {dwMousePosition.Y}")]
            public struct MOUSE_EVENT_RECORD
            {
                public COORD dwMousePosition;
                public Int32 dwButtonState;
                public Int32 dwControlKeyState;
                public Int32 dwEventFlags;
            }

            [DebuggerDisplay("{X}, {Y}")]
            public struct COORD
            {
                public UInt16 X;
                public UInt16 Y;
            }

            [DebuggerDisplay("KeyCode: {wVirtualKeyCode}")]
            [StructLayout(LayoutKind.Explicit)]
            public struct KEY_EVENT_RECORD {
                [FieldOffset(0)]
                [MarshalAsAttribute(UnmanagedType.Bool)]
                public Boolean bKeyDown;
                [FieldOffset(4)]
                public UInt16 wRepeatCount;
                [FieldOffset(6)]
                public UInt16 wVirtualKeyCode;
                [FieldOffset(8)]
                public UInt16 wVirtualScanCode;
                [FieldOffset(10)]
                public Char UnicodeChar;
                [FieldOffset(10)]
                public Byte AsciiChar;
                [FieldOffset(12)]
                public Int32 dwControlKeyState;
            };

            public class ConsoleHandle : SafeHandleMinusOneIsInvalid {
                public ConsoleHandle() : base(false) { }

                protected override bool ReleaseHandle() {
                    return true; //releasing console handle is not our business
                }
            }

            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref Int32 lpMode);

            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

            [DllImportAttribute("kernel32.dll", SetLastError = true)]
            [return: MarshalAsAttribute(UnmanagedType.Bool)]
            public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, Int32 dwMode);
    }
 }