using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    using System;
    using SKey = Signals.KeyboardKey;
    using SKeyDown = Signals.KeyboardKeyDown;
    using SKeyUp = Signals.KeyboardKeyUp;

    using SMouse = Signals.MouseKey;
    using SMouseDown = Signals.MouseKeyDown;
    using SMouseUp = Signals.MouseKeyUp;

    /// <summary>
    /// 用于把各种各样的输入转换为事件回调.
    /// </summary>
    public static class InputProcessor
    {
        public static Vector3 mousePosLastFrame;

        /// <summary>
        /// 该函数在这个类第一次被某个函数引用, 初始化该静态类的时候调用.
        /// 此时 Input.mousePosition 应当是可以工作的.
        /// </summary>
        static InputProcessor()
        {
            mousePosLastFrame = Input.mousePosition;
        }

        /// <summary>
        /// 这个函数应当在游戏的逻辑流水线中被调用.
        /// </summary>
        public static void ProcessInputs()
        {
            // 线性检查每个 Key.
            // 因为是输入, 所以不会耗费很长时间.
            // 使用正则表达式生成.
            // 请查看 UnityEngine.KeyCode 来获取所有输入事件.
            // 注意 KeyCode.None 不参与判定.

            CheckKeyDown();
            CheckKeyPressing();
            CheckKeyUp();

            CheckMouseDown();
            CheckMousePressing();
            CheckMouseUp();


            CheckMouseScrollInput();
            CheckMouseMoveInout();
        }
 
        static void CheckKeyPressing()
        {
            if(Input.GetKey(KeyCode.Backspace)) Signal.Emit(new SKey.Backspace());
            if(Input.GetKey(KeyCode.Tab)) Signal.Emit(new SKey.Tab());
            if(Input.GetKey(KeyCode.Clear)) Signal.Emit(new SKey.Clear());
            if(Input.GetKey(KeyCode.Return)) Signal.Emit(new SKey.Return());
            if(Input.GetKey(KeyCode.Pause)) Signal.Emit(new SKey.Pause());
            if(Input.GetKey(KeyCode.Escape)) Signal.Emit(new SKey.Escape());
            if(Input.GetKey(KeyCode.Space)) Signal.Emit(new SKey.Space());
            if(Input.GetKey(KeyCode.Exclaim)) Signal.Emit(new SKey.Exclaim());
            if(Input.GetKey(KeyCode.DoubleQuote)) Signal.Emit(new SKey.DoubleQuote());
            if(Input.GetKey(KeyCode.Hash)) Signal.Emit(new SKey.Hash());
            if(Input.GetKey(KeyCode.Dollar)) Signal.Emit(new SKey.Dollar());
            if(Input.GetKey(KeyCode.Percent)) Signal.Emit(new SKey.Percent());
            if(Input.GetKey(KeyCode.Ampersand)) Signal.Emit(new SKey.Ampersand());
            if(Input.GetKey(KeyCode.Quote)) Signal.Emit(new SKey.Quote());
            if(Input.GetKey(KeyCode.LeftParen)) Signal.Emit(new SKey.LeftParen());
            if(Input.GetKey(KeyCode.RightParen)) Signal.Emit(new SKey.RightParen());
            if(Input.GetKey(KeyCode.Asterisk)) Signal.Emit(new SKey.Asterisk());
            if(Input.GetKey(KeyCode.Plus)) Signal.Emit(new SKey.Plus());
            if(Input.GetKey(KeyCode.Comma)) Signal.Emit(new SKey.Comma());
            if(Input.GetKey(KeyCode.Minus)) Signal.Emit(new SKey.Minus());
            if(Input.GetKey(KeyCode.Period)) Signal.Emit(new SKey.Period());
            if(Input.GetKey(KeyCode.Slash)) Signal.Emit(new SKey.Slash());
            if(Input.GetKey(KeyCode.Alpha0)) Signal.Emit(new SKey.Alpha0());
            if(Input.GetKey(KeyCode.Alpha1)) Signal.Emit(new SKey.Alpha1());
            if(Input.GetKey(KeyCode.Alpha2)) Signal.Emit(new SKey.Alpha2());
            if(Input.GetKey(KeyCode.Alpha3)) Signal.Emit(new SKey.Alpha3());
            if(Input.GetKey(KeyCode.Alpha4)) Signal.Emit(new SKey.Alpha4());
            if(Input.GetKey(KeyCode.Alpha5)) Signal.Emit(new SKey.Alpha5());
            if(Input.GetKey(KeyCode.Alpha6)) Signal.Emit(new SKey.Alpha6());
            if(Input.GetKey(KeyCode.Alpha7)) Signal.Emit(new SKey.Alpha7());
            if(Input.GetKey(KeyCode.Alpha8)) Signal.Emit(new SKey.Alpha8());
            if(Input.GetKey(KeyCode.Alpha9)) Signal.Emit(new SKey.Alpha9());
            if(Input.GetKey(KeyCode.Colon)) Signal.Emit(new SKey.Colon());
            if(Input.GetKey(KeyCode.Semicolon)) Signal.Emit(new SKey.Semicolon());
            if(Input.GetKey(KeyCode.Less)) Signal.Emit(new SKey.Less());
            if(Input.GetKey(KeyCode.Equals)) Signal.Emit(new SKey.Equal());
            if(Input.GetKey(KeyCode.Greater)) Signal.Emit(new SKey.Greater());
            if(Input.GetKey(KeyCode.Question)) Signal.Emit(new SKey.Question());
            if(Input.GetKey(KeyCode.At)) Signal.Emit(new SKey.At());
            if(Input.GetKey(KeyCode.LeftBracket)) Signal.Emit(new SKey.LeftBracket());
            if(Input.GetKey(KeyCode.Backslash)) Signal.Emit(new SKey.Backslash());
            if(Input.GetKey(KeyCode.RightBracket)) Signal.Emit(new SKey.RightBracket());
            if(Input.GetKey(KeyCode.Caret)) Signal.Emit(new SKey.Caret());
            if(Input.GetKey(KeyCode.Underscore)) Signal.Emit(new SKey.Underscore());
            if(Input.GetKey(KeyCode.BackQuote)) Signal.Emit(new SKey.BackQuote());
            if(Input.GetKey(KeyCode.A)) Signal.Emit(new SKey.A());
            if(Input.GetKey(KeyCode.B)) Signal.Emit(new SKey.B());
            if(Input.GetKey(KeyCode.C)) Signal.Emit(new SKey.C());
            if(Input.GetKey(KeyCode.D)) Signal.Emit(new SKey.D());
            if(Input.GetKey(KeyCode.E)) Signal.Emit(new SKey.E());
            if(Input.GetKey(KeyCode.F)) Signal.Emit(new SKey.F());
            if(Input.GetKey(KeyCode.G)) Signal.Emit(new SKey.G());
            if(Input.GetKey(KeyCode.H)) Signal.Emit(new SKey.H());
            if(Input.GetKey(KeyCode.I)) Signal.Emit(new SKey.I());
            if(Input.GetKey(KeyCode.J)) Signal.Emit(new SKey.J());
            if(Input.GetKey(KeyCode.K)) Signal.Emit(new SKey.K());
            if(Input.GetKey(KeyCode.L)) Signal.Emit(new SKey.L());
            if(Input.GetKey(KeyCode.M)) Signal.Emit(new SKey.M());
            if(Input.GetKey(KeyCode.N)) Signal.Emit(new SKey.N());
            if(Input.GetKey(KeyCode.O)) Signal.Emit(new SKey.O());
            if(Input.GetKey(KeyCode.P)) Signal.Emit(new SKey.P());
            if(Input.GetKey(KeyCode.Q)) Signal.Emit(new SKey.Q());
            if(Input.GetKey(KeyCode.R)) Signal.Emit(new SKey.R());
            if(Input.GetKey(KeyCode.S)) Signal.Emit(new SKey.S());
            if(Input.GetKey(KeyCode.T)) Signal.Emit(new SKey.T());
            if(Input.GetKey(KeyCode.U)) Signal.Emit(new SKey.U());
            if(Input.GetKey(KeyCode.V)) Signal.Emit(new SKey.V());
            if(Input.GetKey(KeyCode.W)) Signal.Emit(new SKey.W());
            if(Input.GetKey(KeyCode.X)) Signal.Emit(new SKey.X());
            if(Input.GetKey(KeyCode.Y)) Signal.Emit(new SKey.Y());
            if(Input.GetKey(KeyCode.Z)) Signal.Emit(new SKey.Z());
            if(Input.GetKey(KeyCode.LeftCurlyBracket)) Signal.Emit(new SKey.LeftCurlyBracket());
            if(Input.GetKey(KeyCode.Pipe)) Signal.Emit(new SKey.Pipe());
            if(Input.GetKey(KeyCode.RightCurlyBracket)) Signal.Emit(new SKey.RightCurlyBracket());
            if(Input.GetKey(KeyCode.Tilde)) Signal.Emit(new SKey.Tilde());
            if(Input.GetKey(KeyCode.Delete)) Signal.Emit(new SKey.Delete());
            if(Input.GetKey(KeyCode.Keypad0)) Signal.Emit(new SKey.Keypad0());
            if(Input.GetKey(KeyCode.Keypad1)) Signal.Emit(new SKey.Keypad1());
            if(Input.GetKey(KeyCode.Keypad2)) Signal.Emit(new SKey.Keypad2());
            if(Input.GetKey(KeyCode.Keypad3)) Signal.Emit(new SKey.Keypad3());
            if(Input.GetKey(KeyCode.Keypad4)) Signal.Emit(new SKey.Keypad4());
            if(Input.GetKey(KeyCode.Keypad5)) Signal.Emit(new SKey.Keypad5());
            if(Input.GetKey(KeyCode.Keypad6)) Signal.Emit(new SKey.Keypad6());
            if(Input.GetKey(KeyCode.Keypad7)) Signal.Emit(new SKey.Keypad7());
            if(Input.GetKey(KeyCode.Keypad8)) Signal.Emit(new SKey.Keypad8());
            if(Input.GetKey(KeyCode.Keypad9)) Signal.Emit(new SKey.Keypad9());
            if(Input.GetKey(KeyCode.KeypadPeriod)) Signal.Emit(new SKey.KeypadPeriod());
            if(Input.GetKey(KeyCode.KeypadDivide)) Signal.Emit(new SKey.KeypadDivide());
            if(Input.GetKey(KeyCode.KeypadMultiply)) Signal.Emit(new SKey.KeypadMultiply());
            if(Input.GetKey(KeyCode.KeypadMinus)) Signal.Emit(new SKey.KeypadMinus());
            if(Input.GetKey(KeyCode.KeypadPlus)) Signal.Emit(new SKey.KeypadPlus());
            if(Input.GetKey(KeyCode.KeypadEnter)) Signal.Emit(new SKey.KeypadEnter());
            if(Input.GetKey(KeyCode.KeypadEquals)) Signal.Emit(new SKey.KeypadEquals());
            if(Input.GetKey(KeyCode.UpArrow)) Signal.Emit(new SKey.UpArrow());
            if(Input.GetKey(KeyCode.DownArrow)) Signal.Emit(new SKey.DownArrow());
            if(Input.GetKey(KeyCode.RightArrow)) Signal.Emit(new SKey.RightArrow());
            if(Input.GetKey(KeyCode.LeftArrow)) Signal.Emit(new SKey.LeftArrow());
            if(Input.GetKey(KeyCode.Insert)) Signal.Emit(new SKey.Insert());
            if(Input.GetKey(KeyCode.Home)) Signal.Emit(new SKey.Home());
            if(Input.GetKey(KeyCode.End)) Signal.Emit(new SKey.End());
            if(Input.GetKey(KeyCode.PageUp)) Signal.Emit(new SKey.PageUp());
            if(Input.GetKey(KeyCode.PageDown)) Signal.Emit(new SKey.PageDown());
            if(Input.GetKey(KeyCode.F1)) Signal.Emit(new SKey.F1());
            if(Input.GetKey(KeyCode.F2)) Signal.Emit(new SKey.F2());
            if(Input.GetKey(KeyCode.F3)) Signal.Emit(new SKey.F3());
            if(Input.GetKey(KeyCode.F4)) Signal.Emit(new SKey.F4());
            if(Input.GetKey(KeyCode.F5)) Signal.Emit(new SKey.F5());
            if(Input.GetKey(KeyCode.F6)) Signal.Emit(new SKey.F6());
            if(Input.GetKey(KeyCode.F7)) Signal.Emit(new SKey.F7());
            if(Input.GetKey(KeyCode.F8)) Signal.Emit(new SKey.F8());
            if(Input.GetKey(KeyCode.F9)) Signal.Emit(new SKey.F9());
            if(Input.GetKey(KeyCode.F10)) Signal.Emit(new SKey.F10());
            if(Input.GetKey(KeyCode.F11)) Signal.Emit(new SKey.F11());
            if(Input.GetKey(KeyCode.F12)) Signal.Emit(new SKey.F12());
            if(Input.GetKey(KeyCode.F13)) Signal.Emit(new SKey.F13());
            if(Input.GetKey(KeyCode.F14)) Signal.Emit(new SKey.F14());
            if(Input.GetKey(KeyCode.F15)) Signal.Emit(new SKey.F15());
            if(Input.GetKey(KeyCode.Numlock)) Signal.Emit(new SKey.Numlock());
            if(Input.GetKey(KeyCode.CapsLock)) Signal.Emit(new SKey.CapsLock());
            if(Input.GetKey(KeyCode.ScrollLock)) Signal.Emit(new SKey.ScrollLock());
            if(Input.GetKey(KeyCode.RightShift)) Signal.Emit(new SKey.RightShift());
            if(Input.GetKey(KeyCode.LeftShift)) Signal.Emit(new SKey.LeftShift());
            if(Input.GetKey(KeyCode.RightControl)) Signal.Emit(new SKey.RightControl());
            if(Input.GetKey(KeyCode.LeftControl)) Signal.Emit(new SKey.LeftControl());
            if(Input.GetKey(KeyCode.RightAlt)) Signal.Emit(new SKey.RightAlt());
            if(Input.GetKey(KeyCode.LeftAlt)) Signal.Emit(new SKey.LeftAlt());
            if(Input.GetKey(KeyCode.RightCommand)) Signal.Emit(new SKey.RightCommand());
            if(Input.GetKey(KeyCode.RightApple)) Signal.Emit(new SKey.RightApple());
            if(Input.GetKey(KeyCode.LeftCommand)) Signal.Emit(new SKey.LeftCommand());
            if(Input.GetKey(KeyCode.LeftApple)) Signal.Emit(new SKey.LeftApple());
            if(Input.GetKey(KeyCode.LeftWindows)) Signal.Emit(new SKey.LeftWindows());
            if(Input.GetKey(KeyCode.RightWindows)) Signal.Emit(new SKey.RightWindows());
            if(Input.GetKey(KeyCode.AltGr)) Signal.Emit(new SKey.AltGr());
            if(Input.GetKey(KeyCode.Help)) Signal.Emit(new SKey.Help());
            if(Input.GetKey(KeyCode.Print)) Signal.Emit(new SKey.Print());
            if(Input.GetKey(KeyCode.SysReq)) Signal.Emit(new SKey.SysReq());
            if(Input.GetKey(KeyCode.Break)) Signal.Emit(new SKey.Break());
            if(Input.GetKey(KeyCode.Menu)) Signal.Emit(new SKey.Menu());
        }

        static void CheckKeyDown()
        {
            if(Input.GetKeyDown(KeyCode.Backspace)) Signal.Emit(new SKeyDown.Backspace());
            if(Input.GetKeyDown(KeyCode.Tab)) Signal.Emit(new SKeyDown.Tab());
            if(Input.GetKeyDown(KeyCode.Clear)) Signal.Emit(new SKeyDown.Clear());
            if(Input.GetKeyDown(KeyCode.Return)) Signal.Emit(new SKeyDown.Return());
            if(Input.GetKeyDown(KeyCode.Pause)) Signal.Emit(new SKeyDown.Pause());
            if(Input.GetKeyDown(KeyCode.Escape)) Signal.Emit(new SKeyDown.Escape());
            if(Input.GetKeyDown(KeyCode.Space)) Signal.Emit(new SKeyDown.Space());
            if(Input.GetKeyDown(KeyCode.Exclaim)) Signal.Emit(new SKeyDown.Exclaim());
            if(Input.GetKeyDown(KeyCode.DoubleQuote)) Signal.Emit(new SKeyDown.DoubleQuote());
            if(Input.GetKeyDown(KeyCode.Hash)) Signal.Emit(new SKeyDown.Hash());
            if(Input.GetKeyDown(KeyCode.Dollar)) Signal.Emit(new SKeyDown.Dollar());
            if(Input.GetKeyDown(KeyCode.Percent)) Signal.Emit(new SKeyDown.Percent());
            if(Input.GetKeyDown(KeyCode.Ampersand)) Signal.Emit(new SKeyDown.Ampersand());
            if(Input.GetKeyDown(KeyCode.Quote)) Signal.Emit(new SKeyDown.Quote());
            if(Input.GetKeyDown(KeyCode.LeftParen)) Signal.Emit(new SKeyDown.LeftParen());
            if(Input.GetKeyDown(KeyCode.RightParen)) Signal.Emit(new SKeyDown.RightParen());
            if(Input.GetKeyDown(KeyCode.Asterisk)) Signal.Emit(new SKeyDown.Asterisk());
            if(Input.GetKeyDown(KeyCode.Plus)) Signal.Emit(new SKeyDown.Plus());
            if(Input.GetKeyDown(KeyCode.Comma)) Signal.Emit(new SKeyDown.Comma());
            if(Input.GetKeyDown(KeyCode.Minus)) Signal.Emit(new SKeyDown.Minus());
            if(Input.GetKeyDown(KeyCode.Period)) Signal.Emit(new SKeyDown.Period());
            if(Input.GetKeyDown(KeyCode.Slash)) Signal.Emit(new SKeyDown.Slash());
            if(Input.GetKeyDown(KeyCode.Alpha0)) Signal.Emit(new SKeyDown.Alpha0());
            if(Input.GetKeyDown(KeyCode.Alpha1)) Signal.Emit(new SKeyDown.Alpha1());
            if(Input.GetKeyDown(KeyCode.Alpha2)) Signal.Emit(new SKeyDown.Alpha2());
            if(Input.GetKeyDown(KeyCode.Alpha3)) Signal.Emit(new SKeyDown.Alpha3());
            if(Input.GetKeyDown(KeyCode.Alpha4)) Signal.Emit(new SKeyDown.Alpha4());
            if(Input.GetKeyDown(KeyCode.Alpha5)) Signal.Emit(new SKeyDown.Alpha5());
            if(Input.GetKeyDown(KeyCode.Alpha6)) Signal.Emit(new SKeyDown.Alpha6());
            if(Input.GetKeyDown(KeyCode.Alpha7)) Signal.Emit(new SKeyDown.Alpha7());
            if(Input.GetKeyDown(KeyCode.Alpha8)) Signal.Emit(new SKeyDown.Alpha8());
            if(Input.GetKeyDown(KeyCode.Alpha9)) Signal.Emit(new SKeyDown.Alpha9());
            if(Input.GetKeyDown(KeyCode.Colon)) Signal.Emit(new SKeyDown.Colon());
            if(Input.GetKeyDown(KeyCode.Semicolon)) Signal.Emit(new SKeyDown.Semicolon());
            if(Input.GetKeyDown(KeyCode.Less)) Signal.Emit(new SKeyDown.Less());
            if(Input.GetKeyDown(KeyCode.Equals)) Signal.Emit(new SKeyDown.Equal());
            if(Input.GetKeyDown(KeyCode.Greater)) Signal.Emit(new SKeyDown.Greater());
            if(Input.GetKeyDown(KeyCode.Question)) Signal.Emit(new SKeyDown.Question());
            if(Input.GetKeyDown(KeyCode.At)) Signal.Emit(new SKeyDown.At());
            if(Input.GetKeyDown(KeyCode.LeftBracket)) Signal.Emit(new SKeyDown.LeftBracket());
            if(Input.GetKeyDown(KeyCode.Backslash)) Signal.Emit(new SKeyDown.Backslash());
            if(Input.GetKeyDown(KeyCode.RightBracket)) Signal.Emit(new SKeyDown.RightBracket());
            if(Input.GetKeyDown(KeyCode.Caret)) Signal.Emit(new SKeyDown.Caret());
            if(Input.GetKeyDown(KeyCode.Underscore)) Signal.Emit(new SKeyDown.Underscore());
            if(Input.GetKeyDown(KeyCode.BackQuote)) Signal.Emit(new SKeyDown.BackQuote());
            if(Input.GetKeyDown(KeyCode.A)) Signal.Emit(new SKeyDown.A());
            if(Input.GetKeyDown(KeyCode.B)) Signal.Emit(new SKeyDown.B());
            if(Input.GetKeyDown(KeyCode.C)) Signal.Emit(new SKeyDown.C());
            if(Input.GetKeyDown(KeyCode.D)) Signal.Emit(new SKeyDown.D());
            if(Input.GetKeyDown(KeyCode.E)) Signal.Emit(new SKeyDown.E());
            if(Input.GetKeyDown(KeyCode.F)) Signal.Emit(new SKeyDown.F());
            if(Input.GetKeyDown(KeyCode.G)) Signal.Emit(new SKeyDown.G());
            if(Input.GetKeyDown(KeyCode.H)) Signal.Emit(new SKeyDown.H());
            if(Input.GetKeyDown(KeyCode.I)) Signal.Emit(new SKeyDown.I());
            if(Input.GetKeyDown(KeyCode.J)) Signal.Emit(new SKeyDown.J());
            if(Input.GetKeyDown(KeyCode.K)) Signal.Emit(new SKeyDown.K());
            if(Input.GetKeyDown(KeyCode.L)) Signal.Emit(new SKeyDown.L());
            if(Input.GetKeyDown(KeyCode.M)) Signal.Emit(new SKeyDown.M());
            if(Input.GetKeyDown(KeyCode.N)) Signal.Emit(new SKeyDown.N());
            if(Input.GetKeyDown(KeyCode.O)) Signal.Emit(new SKeyDown.O());
            if(Input.GetKeyDown(KeyCode.P)) Signal.Emit(new SKeyDown.P());
            if(Input.GetKeyDown(KeyCode.Q)) Signal.Emit(new SKeyDown.Q());
            if(Input.GetKeyDown(KeyCode.R)) Signal.Emit(new SKeyDown.R());
            if(Input.GetKeyDown(KeyCode.S)) Signal.Emit(new SKeyDown.S());
            if(Input.GetKeyDown(KeyCode.T)) Signal.Emit(new SKeyDown.T());
            if(Input.GetKeyDown(KeyCode.U)) Signal.Emit(new SKeyDown.U());
            if(Input.GetKeyDown(KeyCode.V)) Signal.Emit(new SKeyDown.V());
            if(Input.GetKeyDown(KeyCode.W)) Signal.Emit(new SKeyDown.W());
            if(Input.GetKeyDown(KeyCode.X)) Signal.Emit(new SKeyDown.X());
            if(Input.GetKeyDown(KeyCode.Y)) Signal.Emit(new SKeyDown.Y());
            if(Input.GetKeyDown(KeyCode.Z)) Signal.Emit(new SKeyDown.Z());
            if(Input.GetKeyDown(KeyCode.LeftCurlyBracket)) Signal.Emit(new SKeyDown.LeftCurlyBracket());
            if(Input.GetKeyDown(KeyCode.Pipe)) Signal.Emit(new SKeyDown.Pipe());
            if(Input.GetKeyDown(KeyCode.RightCurlyBracket)) Signal.Emit(new SKeyDown.RightCurlyBracket());
            if(Input.GetKeyDown(KeyCode.Tilde)) Signal.Emit(new SKeyDown.Tilde());
            if(Input.GetKeyDown(KeyCode.Delete)) Signal.Emit(new SKeyDown.Delete());
            if(Input.GetKeyDown(KeyCode.Keypad0)) Signal.Emit(new SKeyDown.Keypad0());
            if(Input.GetKeyDown(KeyCode.Keypad1)) Signal.Emit(new SKeyDown.Keypad1());
            if(Input.GetKeyDown(KeyCode.Keypad2)) Signal.Emit(new SKeyDown.Keypad2());
            if(Input.GetKeyDown(KeyCode.Keypad3)) Signal.Emit(new SKeyDown.Keypad3());
            if(Input.GetKeyDown(KeyCode.Keypad4)) Signal.Emit(new SKeyDown.Keypad4());
            if(Input.GetKeyDown(KeyCode.Keypad5)) Signal.Emit(new SKeyDown.Keypad5());
            if(Input.GetKeyDown(KeyCode.Keypad6)) Signal.Emit(new SKeyDown.Keypad6());
            if(Input.GetKeyDown(KeyCode.Keypad7)) Signal.Emit(new SKeyDown.Keypad7());
            if(Input.GetKeyDown(KeyCode.Keypad8)) Signal.Emit(new SKeyDown.Keypad8());
            if(Input.GetKeyDown(KeyCode.Keypad9)) Signal.Emit(new SKeyDown.Keypad9());
            if(Input.GetKeyDown(KeyCode.KeypadPeriod)) Signal.Emit(new SKeyDown.KeypadPeriod());
            if(Input.GetKeyDown(KeyCode.KeypadDivide)) Signal.Emit(new SKeyDown.KeypadDivide());
            if(Input.GetKeyDown(KeyCode.KeypadMultiply)) Signal.Emit(new SKeyDown.KeypadMultiply());
            if(Input.GetKeyDown(KeyCode.KeypadMinus)) Signal.Emit(new SKeyDown.KeypadMinus());
            if(Input.GetKeyDown(KeyCode.KeypadPlus)) Signal.Emit(new SKeyDown.KeypadPlus());
            if(Input.GetKeyDown(KeyCode.KeypadEnter)) Signal.Emit(new SKeyDown.KeypadEnter());
            if(Input.GetKeyDown(KeyCode.KeypadEquals)) Signal.Emit(new SKeyDown.KeypadEquals());
            if(Input.GetKeyDown(KeyCode.UpArrow)) Signal.Emit(new SKeyDown.UpArrow());
            if(Input.GetKeyDown(KeyCode.DownArrow)) Signal.Emit(new SKeyDown.DownArrow());
            if(Input.GetKeyDown(KeyCode.RightArrow)) Signal.Emit(new SKeyDown.RightArrow());
            if(Input.GetKeyDown(KeyCode.LeftArrow)) Signal.Emit(new SKeyDown.LeftArrow());
            if(Input.GetKeyDown(KeyCode.Insert)) Signal.Emit(new SKeyDown.Insert());
            if(Input.GetKeyDown(KeyCode.Home)) Signal.Emit(new SKeyDown.Home());
            if(Input.GetKeyDown(KeyCode.End)) Signal.Emit(new SKeyDown.End());
            if(Input.GetKeyDown(KeyCode.PageUp)) Signal.Emit(new SKeyDown.PageUp());
            if(Input.GetKeyDown(KeyCode.PageDown)) Signal.Emit(new SKeyDown.PageDown());
            if(Input.GetKeyDown(KeyCode.F1)) Signal.Emit(new SKeyDown.F1());
            if(Input.GetKeyDown(KeyCode.F2)) Signal.Emit(new SKeyDown.F2());
            if(Input.GetKeyDown(KeyCode.F3)) Signal.Emit(new SKeyDown.F3());
            if(Input.GetKeyDown(KeyCode.F4)) Signal.Emit(new SKeyDown.F4());
            if(Input.GetKeyDown(KeyCode.F5)) Signal.Emit(new SKeyDown.F5());
            if(Input.GetKeyDown(KeyCode.F6)) Signal.Emit(new SKeyDown.F6());
            if(Input.GetKeyDown(KeyCode.F7)) Signal.Emit(new SKeyDown.F7());
            if(Input.GetKeyDown(KeyCode.F8)) Signal.Emit(new SKeyDown.F8());
            if(Input.GetKeyDown(KeyCode.F9)) Signal.Emit(new SKeyDown.F9());
            if(Input.GetKeyDown(KeyCode.F10)) Signal.Emit(new SKeyDown.F10());
            if(Input.GetKeyDown(KeyCode.F11)) Signal.Emit(new SKeyDown.F11());
            if(Input.GetKeyDown(KeyCode.F12)) Signal.Emit(new SKeyDown.F12());
            if(Input.GetKeyDown(KeyCode.F13)) Signal.Emit(new SKeyDown.F13());
            if(Input.GetKeyDown(KeyCode.F14)) Signal.Emit(new SKeyDown.F14());
            if(Input.GetKeyDown(KeyCode.F15)) Signal.Emit(new SKeyDown.F15());
            if(Input.GetKeyDown(KeyCode.Numlock)) Signal.Emit(new SKeyDown.Numlock());
            if(Input.GetKeyDown(KeyCode.CapsLock)) Signal.Emit(new SKeyDown.CapsLock());
            if(Input.GetKeyDown(KeyCode.ScrollLock)) Signal.Emit(new SKeyDown.ScrollLock());
            if(Input.GetKeyDown(KeyCode.RightShift)) Signal.Emit(new SKeyDown.RightShift());
            if(Input.GetKeyDown(KeyCode.LeftShift)) Signal.Emit(new SKeyDown.LeftShift());
            if(Input.GetKeyDown(KeyCode.RightControl)) Signal.Emit(new SKeyDown.RightControl());
            if(Input.GetKeyDown(KeyCode.LeftControl)) Signal.Emit(new SKeyDown.LeftControl());
            if(Input.GetKeyDown(KeyCode.RightAlt)) Signal.Emit(new SKeyDown.RightAlt());
            if(Input.GetKeyDown(KeyCode.LeftAlt)) Signal.Emit(new SKeyDown.LeftAlt());
            if(Input.GetKeyDown(KeyCode.RightCommand)) Signal.Emit(new SKeyDown.RightCommand());
            if(Input.GetKeyDown(KeyCode.RightApple)) Signal.Emit(new SKeyDown.RightApple());
            if(Input.GetKeyDown(KeyCode.LeftCommand)) Signal.Emit(new SKeyDown.LeftCommand());
            if(Input.GetKeyDown(KeyCode.LeftApple)) Signal.Emit(new SKeyDown.LeftApple());
            if(Input.GetKeyDown(KeyCode.LeftWindows)) Signal.Emit(new SKeyDown.LeftWindows());
            if(Input.GetKeyDown(KeyCode.RightWindows)) Signal.Emit(new SKeyDown.RightWindows());
            if(Input.GetKeyDown(KeyCode.AltGr)) Signal.Emit(new SKeyDown.AltGr());
            if(Input.GetKeyDown(KeyCode.Help)) Signal.Emit(new SKeyDown.Help());
            if(Input.GetKeyDown(KeyCode.Print)) Signal.Emit(new SKeyDown.Print());
            if(Input.GetKeyDown(KeyCode.SysReq)) Signal.Emit(new SKeyDown.SysReq());
            if(Input.GetKeyDown(KeyCode.Break)) Signal.Emit(new SKeyDown.Break());
            if(Input.GetKeyDown(KeyCode.Menu)) Signal.Emit(new SKeyDown.Menu());
        }

        static void CheckKeyUp()
        {
            if(Input.GetKeyUp(KeyCode.Backspace)) Signal.Emit(new SKeyUp.Backspace());
            if(Input.GetKeyUp(KeyCode.Tab)) Signal.Emit(new SKeyUp.Tab());
            if(Input.GetKeyUp(KeyCode.Clear)) Signal.Emit(new SKeyUp.Clear());
            if(Input.GetKeyUp(KeyCode.Return)) Signal.Emit(new SKeyUp.Return());
            if(Input.GetKeyUp(KeyCode.Pause)) Signal.Emit(new SKeyUp.Pause());
            if(Input.GetKeyUp(KeyCode.Escape)) Signal.Emit(new SKeyUp.Escape());
            if(Input.GetKeyUp(KeyCode.Space)) Signal.Emit(new SKeyUp.Space());
            if(Input.GetKeyUp(KeyCode.Exclaim)) Signal.Emit(new SKeyUp.Exclaim());
            if(Input.GetKeyUp(KeyCode.DoubleQuote)) Signal.Emit(new SKeyUp.DoubleQuote());
            if(Input.GetKeyUp(KeyCode.Hash)) Signal.Emit(new SKeyUp.Hash());
            if(Input.GetKeyUp(KeyCode.Dollar)) Signal.Emit(new SKeyUp.Dollar());
            if(Input.GetKeyUp(KeyCode.Percent)) Signal.Emit(new SKeyUp.Percent());
            if(Input.GetKeyUp(KeyCode.Ampersand)) Signal.Emit(new SKeyUp.Ampersand());
            if(Input.GetKeyUp(KeyCode.Quote)) Signal.Emit(new SKeyUp.Quote());
            if(Input.GetKeyUp(KeyCode.LeftParen)) Signal.Emit(new SKeyUp.LeftParen());
            if(Input.GetKeyUp(KeyCode.RightParen)) Signal.Emit(new SKeyUp.RightParen());
            if(Input.GetKeyUp(KeyCode.Asterisk)) Signal.Emit(new SKeyUp.Asterisk());
            if(Input.GetKeyUp(KeyCode.Plus)) Signal.Emit(new SKeyUp.Plus());
            if(Input.GetKeyUp(KeyCode.Comma)) Signal.Emit(new SKeyUp.Comma());
            if(Input.GetKeyUp(KeyCode.Minus)) Signal.Emit(new SKeyUp.Minus());
            if(Input.GetKeyUp(KeyCode.Period)) Signal.Emit(new SKeyUp.Period());
            if(Input.GetKeyUp(KeyCode.Slash)) Signal.Emit(new SKeyUp.Slash());
            if(Input.GetKeyUp(KeyCode.Alpha0)) Signal.Emit(new SKeyUp.Alpha0());
            if(Input.GetKeyUp(KeyCode.Alpha1)) Signal.Emit(new SKeyUp.Alpha1());
            if(Input.GetKeyUp(KeyCode.Alpha2)) Signal.Emit(new SKeyUp.Alpha2());
            if(Input.GetKeyUp(KeyCode.Alpha3)) Signal.Emit(new SKeyUp.Alpha3());
            if(Input.GetKeyUp(KeyCode.Alpha4)) Signal.Emit(new SKeyUp.Alpha4());
            if(Input.GetKeyUp(KeyCode.Alpha5)) Signal.Emit(new SKeyUp.Alpha5());
            if(Input.GetKeyUp(KeyCode.Alpha6)) Signal.Emit(new SKeyUp.Alpha6());
            if(Input.GetKeyUp(KeyCode.Alpha7)) Signal.Emit(new SKeyUp.Alpha7());
            if(Input.GetKeyUp(KeyCode.Alpha8)) Signal.Emit(new SKeyUp.Alpha8());
            if(Input.GetKeyUp(KeyCode.Alpha9)) Signal.Emit(new SKeyUp.Alpha9());
            if(Input.GetKeyUp(KeyCode.Colon)) Signal.Emit(new SKeyUp.Colon());
            if(Input.GetKeyUp(KeyCode.Semicolon)) Signal.Emit(new SKeyUp.Semicolon());
            if(Input.GetKeyUp(KeyCode.Less)) Signal.Emit(new SKeyUp.Less());
            if(Input.GetKeyUp(KeyCode.Equals)) Signal.Emit(new SKeyUp.Equal());
            if(Input.GetKeyUp(KeyCode.Greater)) Signal.Emit(new SKeyUp.Greater());
            if(Input.GetKeyUp(KeyCode.Question)) Signal.Emit(new SKeyUp.Question());
            if(Input.GetKeyUp(KeyCode.At)) Signal.Emit(new SKeyUp.At());
            if(Input.GetKeyUp(KeyCode.LeftBracket)) Signal.Emit(new SKeyUp.LeftBracket());
            if(Input.GetKeyUp(KeyCode.Backslash)) Signal.Emit(new SKeyUp.Backslash());
            if(Input.GetKeyUp(KeyCode.RightBracket)) Signal.Emit(new SKeyUp.RightBracket());
            if(Input.GetKeyUp(KeyCode.Caret)) Signal.Emit(new SKeyUp.Caret());
            if(Input.GetKeyUp(KeyCode.Underscore)) Signal.Emit(new SKeyUp.Underscore());
            if(Input.GetKeyUp(KeyCode.BackQuote)) Signal.Emit(new SKeyUp.BackQuote());
            if(Input.GetKeyUp(KeyCode.A)) Signal.Emit(new SKeyUp.A());
            if(Input.GetKeyUp(KeyCode.B)) Signal.Emit(new SKeyUp.B());
            if(Input.GetKeyUp(KeyCode.C)) Signal.Emit(new SKeyUp.C());
            if(Input.GetKeyUp(KeyCode.D)) Signal.Emit(new SKeyUp.D());
            if(Input.GetKeyUp(KeyCode.E)) Signal.Emit(new SKeyUp.E());
            if(Input.GetKeyUp(KeyCode.F)) Signal.Emit(new SKeyUp.F());
            if(Input.GetKeyUp(KeyCode.G)) Signal.Emit(new SKeyUp.G());
            if(Input.GetKeyUp(KeyCode.H)) Signal.Emit(new SKeyUp.H());
            if(Input.GetKeyUp(KeyCode.I)) Signal.Emit(new SKeyUp.I());
            if(Input.GetKeyUp(KeyCode.J)) Signal.Emit(new SKeyUp.J());
            if(Input.GetKeyUp(KeyCode.K)) Signal.Emit(new SKeyUp.K());
            if(Input.GetKeyUp(KeyCode.L)) Signal.Emit(new SKeyUp.L());
            if(Input.GetKeyUp(KeyCode.M)) Signal.Emit(new SKeyUp.M());
            if(Input.GetKeyUp(KeyCode.N)) Signal.Emit(new SKeyUp.N());
            if(Input.GetKeyUp(KeyCode.O)) Signal.Emit(new SKeyUp.O());
            if(Input.GetKeyUp(KeyCode.P)) Signal.Emit(new SKeyUp.P());
            if(Input.GetKeyUp(KeyCode.Q)) Signal.Emit(new SKeyUp.Q());
            if(Input.GetKeyUp(KeyCode.R)) Signal.Emit(new SKeyUp.R());
            if(Input.GetKeyUp(KeyCode.S)) Signal.Emit(new SKeyUp.S());
            if(Input.GetKeyUp(KeyCode.T)) Signal.Emit(new SKeyUp.T());
            if(Input.GetKeyUp(KeyCode.U)) Signal.Emit(new SKeyUp.U());
            if(Input.GetKeyUp(KeyCode.V)) Signal.Emit(new SKeyUp.V());
            if(Input.GetKeyUp(KeyCode.W)) Signal.Emit(new SKeyUp.W());
            if(Input.GetKeyUp(KeyCode.X)) Signal.Emit(new SKeyUp.X());
            if(Input.GetKeyUp(KeyCode.Y)) Signal.Emit(new SKeyUp.Y());
            if(Input.GetKeyUp(KeyCode.Z)) Signal.Emit(new SKeyUp.Z());
            if(Input.GetKeyUp(KeyCode.LeftCurlyBracket)) Signal.Emit(new SKeyUp.LeftCurlyBracket());
            if(Input.GetKeyUp(KeyCode.Pipe)) Signal.Emit(new SKeyUp.Pipe());
            if(Input.GetKeyUp(KeyCode.RightCurlyBracket)) Signal.Emit(new SKeyUp.RightCurlyBracket());
            if(Input.GetKeyUp(KeyCode.Tilde)) Signal.Emit(new SKeyUp.Tilde());
            if(Input.GetKeyUp(KeyCode.Delete)) Signal.Emit(new SKeyUp.Delete());
            if(Input.GetKeyUp(KeyCode.Keypad0)) Signal.Emit(new SKeyUp.Keypad0());
            if(Input.GetKeyUp(KeyCode.Keypad1)) Signal.Emit(new SKeyUp.Keypad1());
            if(Input.GetKeyUp(KeyCode.Keypad2)) Signal.Emit(new SKeyUp.Keypad2());
            if(Input.GetKeyUp(KeyCode.Keypad3)) Signal.Emit(new SKeyUp.Keypad3());
            if(Input.GetKeyUp(KeyCode.Keypad4)) Signal.Emit(new SKeyUp.Keypad4());
            if(Input.GetKeyUp(KeyCode.Keypad5)) Signal.Emit(new SKeyUp.Keypad5());
            if(Input.GetKeyUp(KeyCode.Keypad6)) Signal.Emit(new SKeyUp.Keypad6());
            if(Input.GetKeyUp(KeyCode.Keypad7)) Signal.Emit(new SKeyUp.Keypad7());
            if(Input.GetKeyUp(KeyCode.Keypad8)) Signal.Emit(new SKeyUp.Keypad8());
            if(Input.GetKeyUp(KeyCode.Keypad9)) Signal.Emit(new SKeyUp.Keypad9());
            if(Input.GetKeyUp(KeyCode.KeypadPeriod)) Signal.Emit(new SKeyUp.KeypadPeriod());
            if(Input.GetKeyUp(KeyCode.KeypadDivide)) Signal.Emit(new SKeyUp.KeypadDivide());
            if(Input.GetKeyUp(KeyCode.KeypadMultiply)) Signal.Emit(new SKeyUp.KeypadMultiply());
            if(Input.GetKeyUp(KeyCode.KeypadMinus)) Signal.Emit(new SKeyUp.KeypadMinus());
            if(Input.GetKeyUp(KeyCode.KeypadPlus)) Signal.Emit(new SKeyUp.KeypadPlus());
            if(Input.GetKeyUp(KeyCode.KeypadEnter)) Signal.Emit(new SKeyUp.KeypadEnter());
            if(Input.GetKeyUp(KeyCode.KeypadEquals)) Signal.Emit(new SKeyUp.KeypadEquals());
            if(Input.GetKeyUp(KeyCode.UpArrow)) Signal.Emit(new SKeyUp.UpArrow());
            if(Input.GetKeyUp(KeyCode.DownArrow)) Signal.Emit(new SKeyUp.DownArrow());
            if(Input.GetKeyUp(KeyCode.RightArrow)) Signal.Emit(new SKeyUp.RightArrow());
            if(Input.GetKeyUp(KeyCode.LeftArrow)) Signal.Emit(new SKeyUp.LeftArrow());
            if(Input.GetKeyUp(KeyCode.Insert)) Signal.Emit(new SKeyUp.Insert());
            if(Input.GetKeyUp(KeyCode.Home)) Signal.Emit(new SKeyUp.Home());
            if(Input.GetKeyUp(KeyCode.End)) Signal.Emit(new SKeyUp.End());
            if(Input.GetKeyUp(KeyCode.PageUp)) Signal.Emit(new SKeyUp.PageUp());
            if(Input.GetKeyUp(KeyCode.PageDown)) Signal.Emit(new SKeyUp.PageDown());
            if(Input.GetKeyUp(KeyCode.F1)) Signal.Emit(new SKeyUp.F1());
            if(Input.GetKeyUp(KeyCode.F2)) Signal.Emit(new SKeyUp.F2());
            if(Input.GetKeyUp(KeyCode.F3)) Signal.Emit(new SKeyUp.F3());
            if(Input.GetKeyUp(KeyCode.F4)) Signal.Emit(new SKeyUp.F4());
            if(Input.GetKeyUp(KeyCode.F5)) Signal.Emit(new SKeyUp.F5());
            if(Input.GetKeyUp(KeyCode.F6)) Signal.Emit(new SKeyUp.F6());
            if(Input.GetKeyUp(KeyCode.F7)) Signal.Emit(new SKeyUp.F7());
            if(Input.GetKeyUp(KeyCode.F8)) Signal.Emit(new SKeyUp.F8());
            if(Input.GetKeyUp(KeyCode.F9)) Signal.Emit(new SKeyUp.F9());
            if(Input.GetKeyUp(KeyCode.F10)) Signal.Emit(new SKeyUp.F10());
            if(Input.GetKeyUp(KeyCode.F11)) Signal.Emit(new SKeyUp.F11());
            if(Input.GetKeyUp(KeyCode.F12)) Signal.Emit(new SKeyUp.F12());
            if(Input.GetKeyUp(KeyCode.F13)) Signal.Emit(new SKeyUp.F13());
            if(Input.GetKeyUp(KeyCode.F14)) Signal.Emit(new SKeyUp.F14());
            if(Input.GetKeyUp(KeyCode.F15)) Signal.Emit(new SKeyUp.F15());
            if(Input.GetKeyUp(KeyCode.Numlock)) Signal.Emit(new SKeyUp.Numlock());
            if(Input.GetKeyUp(KeyCode.CapsLock)) Signal.Emit(new SKeyUp.CapsLock());
            if(Input.GetKeyUp(KeyCode.ScrollLock)) Signal.Emit(new SKeyUp.ScrollLock());
            if(Input.GetKeyUp(KeyCode.RightShift)) Signal.Emit(new SKeyUp.RightShift());
            if(Input.GetKeyUp(KeyCode.LeftShift)) Signal.Emit(new SKeyUp.LeftShift());
            if(Input.GetKeyUp(KeyCode.RightControl)) Signal.Emit(new SKeyUp.RightControl());
            if(Input.GetKeyUp(KeyCode.LeftControl)) Signal.Emit(new SKeyUp.LeftControl());
            if(Input.GetKeyUp(KeyCode.RightAlt)) Signal.Emit(new SKeyUp.RightAlt());
            if(Input.GetKeyUp(KeyCode.LeftAlt)) Signal.Emit(new SKeyUp.LeftAlt());
            if(Input.GetKeyUp(KeyCode.RightCommand)) Signal.Emit(new SKeyUp.RightCommand());
            if(Input.GetKeyUp(KeyCode.RightApple)) Signal.Emit(new SKeyUp.RightApple());
            if(Input.GetKeyUp(KeyCode.LeftCommand)) Signal.Emit(new SKeyUp.LeftCommand());
            if(Input.GetKeyUp(KeyCode.LeftApple)) Signal.Emit(new SKeyUp.LeftApple());
            if(Input.GetKeyUp(KeyCode.LeftWindows)) Signal.Emit(new SKeyUp.LeftWindows());
            if(Input.GetKeyUp(KeyCode.RightWindows)) Signal.Emit(new SKeyUp.RightWindows());
            if(Input.GetKeyUp(KeyCode.AltGr)) Signal.Emit(new SKeyUp.AltGr());
            if(Input.GetKeyUp(KeyCode.Help)) Signal.Emit(new SKeyUp.Help());
            if(Input.GetKeyUp(KeyCode.Print)) Signal.Emit(new SKeyUp.Print());
            if(Input.GetKeyUp(KeyCode.SysReq)) Signal.Emit(new SKeyUp.SysReq());
            if(Input.GetKeyUp(KeyCode.Break)) Signal.Emit(new SKeyUp.Break());
            if(Input.GetKeyUp(KeyCode.Menu)) Signal.Emit(new SKeyUp.Menu());
        }

        static void CheckMouseDown()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0)) Signal.Emit(new SMouseDown.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse1)) Signal.Emit(new SMouseDown.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse2)) Signal.Emit(new SMouseDown.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse3)) Signal.Emit(new SMouseDown.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse4)) Signal.Emit(new SMouseDown.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse5)) Signal.Emit(new SMouseDown.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse6)) Signal.Emit(new SMouseDown.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMousePressing()
        {
            if(Input.GetKey(KeyCode.Mouse0)) Signal.Emit(new SMouse.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse1)) Signal.Emit(new SMouse.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse2)) Signal.Emit(new SMouse.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse3)) Signal.Emit(new SMouse.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse4)) Signal.Emit(new SMouse.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse5)) Signal.Emit(new SMouse.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse6)) Signal.Emit(new SMouse.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMouseUp()
        {
            if(Input.GetKeyUp(KeyCode.Mouse0)) Signal.Emit(new SMouseUp.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse1)) Signal.Emit(new SMouseUp.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse2)) Signal.Emit(new SMouseUp.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse3)) Signal.Emit(new SMouseUp.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse4)) Signal.Emit(new SMouseUp.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse5)) Signal.Emit(new SMouseUp.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse6)) Signal.Emit(new SMouseUp.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMouseMoveInout()
        {
            var curPos = Input.mousePosition;
            if(curPos != mousePosLastFrame)
            {
                Signal.Emit(new Signals.MouseMove() { delta = curPos - mousePosLastFrame });
            }

            mousePosLastFrame = curPos;
        }

        static void CheckMouseScrollInput()
        {
            if(Input.mouseScrollDelta.y != 0f)
            {
                Signal.Emit(new Signals.MouseScroll() { delta = Input.mouseScrollDelta.y });
            }
        }

    }
}