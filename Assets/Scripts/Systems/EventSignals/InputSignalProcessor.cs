using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower.Systems
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
            if(Input.GetKey(KeyCode.Backspace)) SignalSystem.Emit(new SKey.Backspace());
            if(Input.GetKey(KeyCode.Tab)) SignalSystem.Emit(new SKey.Tab());
            if(Input.GetKey(KeyCode.Clear)) SignalSystem.Emit(new SKey.Clear());
            if(Input.GetKey(KeyCode.Return)) SignalSystem.Emit(new SKey.Return());
            if(Input.GetKey(KeyCode.Pause)) SignalSystem.Emit(new SKey.Pause());
            if(Input.GetKey(KeyCode.Escape)) SignalSystem.Emit(new SKey.Escape());
            if(Input.GetKey(KeyCode.Space)) SignalSystem.Emit(new SKey.Space());
            if(Input.GetKey(KeyCode.Exclaim)) SignalSystem.Emit(new SKey.Exclaim());
            if(Input.GetKey(KeyCode.DoubleQuote)) SignalSystem.Emit(new SKey.DoubleQuote());
            if(Input.GetKey(KeyCode.Hash)) SignalSystem.Emit(new SKey.Hash());
            if(Input.GetKey(KeyCode.Dollar)) SignalSystem.Emit(new SKey.Dollar());
            if(Input.GetKey(KeyCode.Percent)) SignalSystem.Emit(new SKey.Percent());
            if(Input.GetKey(KeyCode.Ampersand)) SignalSystem.Emit(new SKey.Ampersand());
            if(Input.GetKey(KeyCode.Quote)) SignalSystem.Emit(new SKey.Quote());
            if(Input.GetKey(KeyCode.LeftParen)) SignalSystem.Emit(new SKey.LeftParen());
            if(Input.GetKey(KeyCode.RightParen)) SignalSystem.Emit(new SKey.RightParen());
            if(Input.GetKey(KeyCode.Asterisk)) SignalSystem.Emit(new SKey.Asterisk());
            if(Input.GetKey(KeyCode.Plus)) SignalSystem.Emit(new SKey.Plus());
            if(Input.GetKey(KeyCode.Comma)) SignalSystem.Emit(new SKey.Comma());
            if(Input.GetKey(KeyCode.Minus)) SignalSystem.Emit(new SKey.Minus());
            if(Input.GetKey(KeyCode.Period)) SignalSystem.Emit(new SKey.Period());
            if(Input.GetKey(KeyCode.Slash)) SignalSystem.Emit(new SKey.Slash());
            if(Input.GetKey(KeyCode.Alpha0)) SignalSystem.Emit(new SKey.Alpha0());
            if(Input.GetKey(KeyCode.Alpha1)) SignalSystem.Emit(new SKey.Alpha1());
            if(Input.GetKey(KeyCode.Alpha2)) SignalSystem.Emit(new SKey.Alpha2());
            if(Input.GetKey(KeyCode.Alpha3)) SignalSystem.Emit(new SKey.Alpha3());
            if(Input.GetKey(KeyCode.Alpha4)) SignalSystem.Emit(new SKey.Alpha4());
            if(Input.GetKey(KeyCode.Alpha5)) SignalSystem.Emit(new SKey.Alpha5());
            if(Input.GetKey(KeyCode.Alpha6)) SignalSystem.Emit(new SKey.Alpha6());
            if(Input.GetKey(KeyCode.Alpha7)) SignalSystem.Emit(new SKey.Alpha7());
            if(Input.GetKey(KeyCode.Alpha8)) SignalSystem.Emit(new SKey.Alpha8());
            if(Input.GetKey(KeyCode.Alpha9)) SignalSystem.Emit(new SKey.Alpha9());
            if(Input.GetKey(KeyCode.Colon)) SignalSystem.Emit(new SKey.Colon());
            if(Input.GetKey(KeyCode.Semicolon)) SignalSystem.Emit(new SKey.Semicolon());
            if(Input.GetKey(KeyCode.Less)) SignalSystem.Emit(new SKey.Less());
            if(Input.GetKey(KeyCode.Equals)) SignalSystem.Emit(new SKey.Equal());
            if(Input.GetKey(KeyCode.Greater)) SignalSystem.Emit(new SKey.Greater());
            if(Input.GetKey(KeyCode.Question)) SignalSystem.Emit(new SKey.Question());
            if(Input.GetKey(KeyCode.At)) SignalSystem.Emit(new SKey.At());
            if(Input.GetKey(KeyCode.LeftBracket)) SignalSystem.Emit(new SKey.LeftBracket());
            if(Input.GetKey(KeyCode.Backslash)) SignalSystem.Emit(new SKey.Backslash());
            if(Input.GetKey(KeyCode.RightBracket)) SignalSystem.Emit(new SKey.RightBracket());
            if(Input.GetKey(KeyCode.Caret)) SignalSystem.Emit(new SKey.Caret());
            if(Input.GetKey(KeyCode.Underscore)) SignalSystem.Emit(new SKey.Underscore());
            if(Input.GetKey(KeyCode.BackQuote)) SignalSystem.Emit(new SKey.BackQuote());
            if(Input.GetKey(KeyCode.A)) SignalSystem.Emit(new SKey.A());
            if(Input.GetKey(KeyCode.B)) SignalSystem.Emit(new SKey.B());
            if(Input.GetKey(KeyCode.C)) SignalSystem.Emit(new SKey.C());
            if(Input.GetKey(KeyCode.D)) SignalSystem.Emit(new SKey.D());
            if(Input.GetKey(KeyCode.E)) SignalSystem.Emit(new SKey.E());
            if(Input.GetKey(KeyCode.F)) SignalSystem.Emit(new SKey.F());
            if(Input.GetKey(KeyCode.G)) SignalSystem.Emit(new SKey.G());
            if(Input.GetKey(KeyCode.H)) SignalSystem.Emit(new SKey.H());
            if(Input.GetKey(KeyCode.I)) SignalSystem.Emit(new SKey.I());
            if(Input.GetKey(KeyCode.J)) SignalSystem.Emit(new SKey.J());
            if(Input.GetKey(KeyCode.K)) SignalSystem.Emit(new SKey.K());
            if(Input.GetKey(KeyCode.L)) SignalSystem.Emit(new SKey.L());
            if(Input.GetKey(KeyCode.M)) SignalSystem.Emit(new SKey.M());
            if(Input.GetKey(KeyCode.N)) SignalSystem.Emit(new SKey.N());
            if(Input.GetKey(KeyCode.O)) SignalSystem.Emit(new SKey.O());
            if(Input.GetKey(KeyCode.P)) SignalSystem.Emit(new SKey.P());
            if(Input.GetKey(KeyCode.Q)) SignalSystem.Emit(new SKey.Q());
            if(Input.GetKey(KeyCode.R)) SignalSystem.Emit(new SKey.R());
            if(Input.GetKey(KeyCode.S)) SignalSystem.Emit(new SKey.S());
            if(Input.GetKey(KeyCode.T)) SignalSystem.Emit(new SKey.T());
            if(Input.GetKey(KeyCode.U)) SignalSystem.Emit(new SKey.U());
            if(Input.GetKey(KeyCode.V)) SignalSystem.Emit(new SKey.V());
            if(Input.GetKey(KeyCode.W)) SignalSystem.Emit(new SKey.W());
            if(Input.GetKey(KeyCode.X)) SignalSystem.Emit(new SKey.X());
            if(Input.GetKey(KeyCode.Y)) SignalSystem.Emit(new SKey.Y());
            if(Input.GetKey(KeyCode.Z)) SignalSystem.Emit(new SKey.Z());
            if(Input.GetKey(KeyCode.LeftCurlyBracket)) SignalSystem.Emit(new SKey.LeftCurlyBracket());
            if(Input.GetKey(KeyCode.Pipe)) SignalSystem.Emit(new SKey.Pipe());
            if(Input.GetKey(KeyCode.RightCurlyBracket)) SignalSystem.Emit(new SKey.RightCurlyBracket());
            if(Input.GetKey(KeyCode.Tilde)) SignalSystem.Emit(new SKey.Tilde());
            if(Input.GetKey(KeyCode.Delete)) SignalSystem.Emit(new SKey.Delete());
            if(Input.GetKey(KeyCode.Keypad0)) SignalSystem.Emit(new SKey.Keypad0());
            if(Input.GetKey(KeyCode.Keypad1)) SignalSystem.Emit(new SKey.Keypad1());
            if(Input.GetKey(KeyCode.Keypad2)) SignalSystem.Emit(new SKey.Keypad2());
            if(Input.GetKey(KeyCode.Keypad3)) SignalSystem.Emit(new SKey.Keypad3());
            if(Input.GetKey(KeyCode.Keypad4)) SignalSystem.Emit(new SKey.Keypad4());
            if(Input.GetKey(KeyCode.Keypad5)) SignalSystem.Emit(new SKey.Keypad5());
            if(Input.GetKey(KeyCode.Keypad6)) SignalSystem.Emit(new SKey.Keypad6());
            if(Input.GetKey(KeyCode.Keypad7)) SignalSystem.Emit(new SKey.Keypad7());
            if(Input.GetKey(KeyCode.Keypad8)) SignalSystem.Emit(new SKey.Keypad8());
            if(Input.GetKey(KeyCode.Keypad9)) SignalSystem.Emit(new SKey.Keypad9());
            if(Input.GetKey(KeyCode.KeypadPeriod)) SignalSystem.Emit(new SKey.KeypadPeriod());
            if(Input.GetKey(KeyCode.KeypadDivide)) SignalSystem.Emit(new SKey.KeypadDivide());
            if(Input.GetKey(KeyCode.KeypadMultiply)) SignalSystem.Emit(new SKey.KeypadMultiply());
            if(Input.GetKey(KeyCode.KeypadMinus)) SignalSystem.Emit(new SKey.KeypadMinus());
            if(Input.GetKey(KeyCode.KeypadPlus)) SignalSystem.Emit(new SKey.KeypadPlus());
            if(Input.GetKey(KeyCode.KeypadEnter)) SignalSystem.Emit(new SKey.KeypadEnter());
            if(Input.GetKey(KeyCode.KeypadEquals)) SignalSystem.Emit(new SKey.KeypadEquals());
            if(Input.GetKey(KeyCode.UpArrow)) SignalSystem.Emit(new SKey.UpArrow());
            if(Input.GetKey(KeyCode.DownArrow)) SignalSystem.Emit(new SKey.DownArrow());
            if(Input.GetKey(KeyCode.RightArrow)) SignalSystem.Emit(new SKey.RightArrow());
            if(Input.GetKey(KeyCode.LeftArrow)) SignalSystem.Emit(new SKey.LeftArrow());
            if(Input.GetKey(KeyCode.Insert)) SignalSystem.Emit(new SKey.Insert());
            if(Input.GetKey(KeyCode.Home)) SignalSystem.Emit(new SKey.Home());
            if(Input.GetKey(KeyCode.End)) SignalSystem.Emit(new SKey.End());
            if(Input.GetKey(KeyCode.PageUp)) SignalSystem.Emit(new SKey.PageUp());
            if(Input.GetKey(KeyCode.PageDown)) SignalSystem.Emit(new SKey.PageDown());
            if(Input.GetKey(KeyCode.F1)) SignalSystem.Emit(new SKey.F1());
            if(Input.GetKey(KeyCode.F2)) SignalSystem.Emit(new SKey.F2());
            if(Input.GetKey(KeyCode.F3)) SignalSystem.Emit(new SKey.F3());
            if(Input.GetKey(KeyCode.F4)) SignalSystem.Emit(new SKey.F4());
            if(Input.GetKey(KeyCode.F5)) SignalSystem.Emit(new SKey.F5());
            if(Input.GetKey(KeyCode.F6)) SignalSystem.Emit(new SKey.F6());
            if(Input.GetKey(KeyCode.F7)) SignalSystem.Emit(new SKey.F7());
            if(Input.GetKey(KeyCode.F8)) SignalSystem.Emit(new SKey.F8());
            if(Input.GetKey(KeyCode.F9)) SignalSystem.Emit(new SKey.F9());
            if(Input.GetKey(KeyCode.F10)) SignalSystem.Emit(new SKey.F10());
            if(Input.GetKey(KeyCode.F11)) SignalSystem.Emit(new SKey.F11());
            if(Input.GetKey(KeyCode.F12)) SignalSystem.Emit(new SKey.F12());
            if(Input.GetKey(KeyCode.F13)) SignalSystem.Emit(new SKey.F13());
            if(Input.GetKey(KeyCode.F14)) SignalSystem.Emit(new SKey.F14());
            if(Input.GetKey(KeyCode.F15)) SignalSystem.Emit(new SKey.F15());
            if(Input.GetKey(KeyCode.Numlock)) SignalSystem.Emit(new SKey.Numlock());
            if(Input.GetKey(KeyCode.CapsLock)) SignalSystem.Emit(new SKey.CapsLock());
            if(Input.GetKey(KeyCode.ScrollLock)) SignalSystem.Emit(new SKey.ScrollLock());
            if(Input.GetKey(KeyCode.RightShift)) SignalSystem.Emit(new SKey.RightShift());
            if(Input.GetKey(KeyCode.LeftShift)) SignalSystem.Emit(new SKey.LeftShift());
            if(Input.GetKey(KeyCode.RightControl)) SignalSystem.Emit(new SKey.RightControl());
            if(Input.GetKey(KeyCode.LeftControl)) SignalSystem.Emit(new SKey.LeftControl());
            if(Input.GetKey(KeyCode.RightAlt)) SignalSystem.Emit(new SKey.RightAlt());
            if(Input.GetKey(KeyCode.LeftAlt)) SignalSystem.Emit(new SKey.LeftAlt());
            if(Input.GetKey(KeyCode.RightCommand)) SignalSystem.Emit(new SKey.RightCommand());
            if(Input.GetKey(KeyCode.RightApple)) SignalSystem.Emit(new SKey.RightApple());
            if(Input.GetKey(KeyCode.LeftCommand)) SignalSystem.Emit(new SKey.LeftCommand());
            if(Input.GetKey(KeyCode.LeftApple)) SignalSystem.Emit(new SKey.LeftApple());
            if(Input.GetKey(KeyCode.LeftWindows)) SignalSystem.Emit(new SKey.LeftWindows());
            if(Input.GetKey(KeyCode.RightWindows)) SignalSystem.Emit(new SKey.RightWindows());
            if(Input.GetKey(KeyCode.AltGr)) SignalSystem.Emit(new SKey.AltGr());
            if(Input.GetKey(KeyCode.Help)) SignalSystem.Emit(new SKey.Help());
            if(Input.GetKey(KeyCode.Print)) SignalSystem.Emit(new SKey.Print());
            if(Input.GetKey(KeyCode.SysReq)) SignalSystem.Emit(new SKey.SysReq());
            if(Input.GetKey(KeyCode.Break)) SignalSystem.Emit(new SKey.Break());
            if(Input.GetKey(KeyCode.Menu)) SignalSystem.Emit(new SKey.Menu());
        }

        static void CheckKeyDown()
        {
            if(Input.GetKeyDown(KeyCode.Backspace)) SignalSystem.Emit(new SKeyDown.Backspace());
            if(Input.GetKeyDown(KeyCode.Tab)) SignalSystem.Emit(new SKeyDown.Tab());
            if(Input.GetKeyDown(KeyCode.Clear)) SignalSystem.Emit(new SKeyDown.Clear());
            if(Input.GetKeyDown(KeyCode.Return)) SignalSystem.Emit(new SKeyDown.Return());
            if(Input.GetKeyDown(KeyCode.Pause)) SignalSystem.Emit(new SKeyDown.Pause());
            if(Input.GetKeyDown(KeyCode.Escape)) SignalSystem.Emit(new SKeyDown.Escape());
            if(Input.GetKeyDown(KeyCode.Space)) SignalSystem.Emit(new SKeyDown.Space());
            if(Input.GetKeyDown(KeyCode.Exclaim)) SignalSystem.Emit(new SKeyDown.Exclaim());
            if(Input.GetKeyDown(KeyCode.DoubleQuote)) SignalSystem.Emit(new SKeyDown.DoubleQuote());
            if(Input.GetKeyDown(KeyCode.Hash)) SignalSystem.Emit(new SKeyDown.Hash());
            if(Input.GetKeyDown(KeyCode.Dollar)) SignalSystem.Emit(new SKeyDown.Dollar());
            if(Input.GetKeyDown(KeyCode.Percent)) SignalSystem.Emit(new SKeyDown.Percent());
            if(Input.GetKeyDown(KeyCode.Ampersand)) SignalSystem.Emit(new SKeyDown.Ampersand());
            if(Input.GetKeyDown(KeyCode.Quote)) SignalSystem.Emit(new SKeyDown.Quote());
            if(Input.GetKeyDown(KeyCode.LeftParen)) SignalSystem.Emit(new SKeyDown.LeftParen());
            if(Input.GetKeyDown(KeyCode.RightParen)) SignalSystem.Emit(new SKeyDown.RightParen());
            if(Input.GetKeyDown(KeyCode.Asterisk)) SignalSystem.Emit(new SKeyDown.Asterisk());
            if(Input.GetKeyDown(KeyCode.Plus)) SignalSystem.Emit(new SKeyDown.Plus());
            if(Input.GetKeyDown(KeyCode.Comma)) SignalSystem.Emit(new SKeyDown.Comma());
            if(Input.GetKeyDown(KeyCode.Minus)) SignalSystem.Emit(new SKeyDown.Minus());
            if(Input.GetKeyDown(KeyCode.Period)) SignalSystem.Emit(new SKeyDown.Period());
            if(Input.GetKeyDown(KeyCode.Slash)) SignalSystem.Emit(new SKeyDown.Slash());
            if(Input.GetKeyDown(KeyCode.Alpha0)) SignalSystem.Emit(new SKeyDown.Alpha0());
            if(Input.GetKeyDown(KeyCode.Alpha1)) SignalSystem.Emit(new SKeyDown.Alpha1());
            if(Input.GetKeyDown(KeyCode.Alpha2)) SignalSystem.Emit(new SKeyDown.Alpha2());
            if(Input.GetKeyDown(KeyCode.Alpha3)) SignalSystem.Emit(new SKeyDown.Alpha3());
            if(Input.GetKeyDown(KeyCode.Alpha4)) SignalSystem.Emit(new SKeyDown.Alpha4());
            if(Input.GetKeyDown(KeyCode.Alpha5)) SignalSystem.Emit(new SKeyDown.Alpha5());
            if(Input.GetKeyDown(KeyCode.Alpha6)) SignalSystem.Emit(new SKeyDown.Alpha6());
            if(Input.GetKeyDown(KeyCode.Alpha7)) SignalSystem.Emit(new SKeyDown.Alpha7());
            if(Input.GetKeyDown(KeyCode.Alpha8)) SignalSystem.Emit(new SKeyDown.Alpha8());
            if(Input.GetKeyDown(KeyCode.Alpha9)) SignalSystem.Emit(new SKeyDown.Alpha9());
            if(Input.GetKeyDown(KeyCode.Colon)) SignalSystem.Emit(new SKeyDown.Colon());
            if(Input.GetKeyDown(KeyCode.Semicolon)) SignalSystem.Emit(new SKeyDown.Semicolon());
            if(Input.GetKeyDown(KeyCode.Less)) SignalSystem.Emit(new SKeyDown.Less());
            if(Input.GetKeyDown(KeyCode.Equals)) SignalSystem.Emit(new SKeyDown.Equal());
            if(Input.GetKeyDown(KeyCode.Greater)) SignalSystem.Emit(new SKeyDown.Greater());
            if(Input.GetKeyDown(KeyCode.Question)) SignalSystem.Emit(new SKeyDown.Question());
            if(Input.GetKeyDown(KeyCode.At)) SignalSystem.Emit(new SKeyDown.At());
            if(Input.GetKeyDown(KeyCode.LeftBracket)) SignalSystem.Emit(new SKeyDown.LeftBracket());
            if(Input.GetKeyDown(KeyCode.Backslash)) SignalSystem.Emit(new SKeyDown.Backslash());
            if(Input.GetKeyDown(KeyCode.RightBracket)) SignalSystem.Emit(new SKeyDown.RightBracket());
            if(Input.GetKeyDown(KeyCode.Caret)) SignalSystem.Emit(new SKeyDown.Caret());
            if(Input.GetKeyDown(KeyCode.Underscore)) SignalSystem.Emit(new SKeyDown.Underscore());
            if(Input.GetKeyDown(KeyCode.BackQuote)) SignalSystem.Emit(new SKeyDown.BackQuote());
            if(Input.GetKeyDown(KeyCode.A)) SignalSystem.Emit(new SKeyDown.A());
            if(Input.GetKeyDown(KeyCode.B)) SignalSystem.Emit(new SKeyDown.B());
            if(Input.GetKeyDown(KeyCode.C)) SignalSystem.Emit(new SKeyDown.C());
            if(Input.GetKeyDown(KeyCode.D)) SignalSystem.Emit(new SKeyDown.D());
            if(Input.GetKeyDown(KeyCode.E)) SignalSystem.Emit(new SKeyDown.E());
            if(Input.GetKeyDown(KeyCode.F)) SignalSystem.Emit(new SKeyDown.F());
            if(Input.GetKeyDown(KeyCode.G)) SignalSystem.Emit(new SKeyDown.G());
            if(Input.GetKeyDown(KeyCode.H)) SignalSystem.Emit(new SKeyDown.H());
            if(Input.GetKeyDown(KeyCode.I)) SignalSystem.Emit(new SKeyDown.I());
            if(Input.GetKeyDown(KeyCode.J)) SignalSystem.Emit(new SKeyDown.J());
            if(Input.GetKeyDown(KeyCode.K)) SignalSystem.Emit(new SKeyDown.K());
            if(Input.GetKeyDown(KeyCode.L)) SignalSystem.Emit(new SKeyDown.L());
            if(Input.GetKeyDown(KeyCode.M)) SignalSystem.Emit(new SKeyDown.M());
            if(Input.GetKeyDown(KeyCode.N)) SignalSystem.Emit(new SKeyDown.N());
            if(Input.GetKeyDown(KeyCode.O)) SignalSystem.Emit(new SKeyDown.O());
            if(Input.GetKeyDown(KeyCode.P)) SignalSystem.Emit(new SKeyDown.P());
            if(Input.GetKeyDown(KeyCode.Q)) SignalSystem.Emit(new SKeyDown.Q());
            if(Input.GetKeyDown(KeyCode.R)) SignalSystem.Emit(new SKeyDown.R());
            if(Input.GetKeyDown(KeyCode.S)) SignalSystem.Emit(new SKeyDown.S());
            if(Input.GetKeyDown(KeyCode.T)) SignalSystem.Emit(new SKeyDown.T());
            if(Input.GetKeyDown(KeyCode.U)) SignalSystem.Emit(new SKeyDown.U());
            if(Input.GetKeyDown(KeyCode.V)) SignalSystem.Emit(new SKeyDown.V());
            if(Input.GetKeyDown(KeyCode.W)) SignalSystem.Emit(new SKeyDown.W());
            if(Input.GetKeyDown(KeyCode.X)) SignalSystem.Emit(new SKeyDown.X());
            if(Input.GetKeyDown(KeyCode.Y)) SignalSystem.Emit(new SKeyDown.Y());
            if(Input.GetKeyDown(KeyCode.Z)) SignalSystem.Emit(new SKeyDown.Z());
            if(Input.GetKeyDown(KeyCode.LeftCurlyBracket)) SignalSystem.Emit(new SKeyDown.LeftCurlyBracket());
            if(Input.GetKeyDown(KeyCode.Pipe)) SignalSystem.Emit(new SKeyDown.Pipe());
            if(Input.GetKeyDown(KeyCode.RightCurlyBracket)) SignalSystem.Emit(new SKeyDown.RightCurlyBracket());
            if(Input.GetKeyDown(KeyCode.Tilde)) SignalSystem.Emit(new SKeyDown.Tilde());
            if(Input.GetKeyDown(KeyCode.Delete)) SignalSystem.Emit(new SKeyDown.Delete());
            if(Input.GetKeyDown(KeyCode.Keypad0)) SignalSystem.Emit(new SKeyDown.Keypad0());
            if(Input.GetKeyDown(KeyCode.Keypad1)) SignalSystem.Emit(new SKeyDown.Keypad1());
            if(Input.GetKeyDown(KeyCode.Keypad2)) SignalSystem.Emit(new SKeyDown.Keypad2());
            if(Input.GetKeyDown(KeyCode.Keypad3)) SignalSystem.Emit(new SKeyDown.Keypad3());
            if(Input.GetKeyDown(KeyCode.Keypad4)) SignalSystem.Emit(new SKeyDown.Keypad4());
            if(Input.GetKeyDown(KeyCode.Keypad5)) SignalSystem.Emit(new SKeyDown.Keypad5());
            if(Input.GetKeyDown(KeyCode.Keypad6)) SignalSystem.Emit(new SKeyDown.Keypad6());
            if(Input.GetKeyDown(KeyCode.Keypad7)) SignalSystem.Emit(new SKeyDown.Keypad7());
            if(Input.GetKeyDown(KeyCode.Keypad8)) SignalSystem.Emit(new SKeyDown.Keypad8());
            if(Input.GetKeyDown(KeyCode.Keypad9)) SignalSystem.Emit(new SKeyDown.Keypad9());
            if(Input.GetKeyDown(KeyCode.KeypadPeriod)) SignalSystem.Emit(new SKeyDown.KeypadPeriod());
            if(Input.GetKeyDown(KeyCode.KeypadDivide)) SignalSystem.Emit(new SKeyDown.KeypadDivide());
            if(Input.GetKeyDown(KeyCode.KeypadMultiply)) SignalSystem.Emit(new SKeyDown.KeypadMultiply());
            if(Input.GetKeyDown(KeyCode.KeypadMinus)) SignalSystem.Emit(new SKeyDown.KeypadMinus());
            if(Input.GetKeyDown(KeyCode.KeypadPlus)) SignalSystem.Emit(new SKeyDown.KeypadPlus());
            if(Input.GetKeyDown(KeyCode.KeypadEnter)) SignalSystem.Emit(new SKeyDown.KeypadEnter());
            if(Input.GetKeyDown(KeyCode.KeypadEquals)) SignalSystem.Emit(new SKeyDown.KeypadEquals());
            if(Input.GetKeyDown(KeyCode.UpArrow)) SignalSystem.Emit(new SKeyDown.UpArrow());
            if(Input.GetKeyDown(KeyCode.DownArrow)) SignalSystem.Emit(new SKeyDown.DownArrow());
            if(Input.GetKeyDown(KeyCode.RightArrow)) SignalSystem.Emit(new SKeyDown.RightArrow());
            if(Input.GetKeyDown(KeyCode.LeftArrow)) SignalSystem.Emit(new SKeyDown.LeftArrow());
            if(Input.GetKeyDown(KeyCode.Insert)) SignalSystem.Emit(new SKeyDown.Insert());
            if(Input.GetKeyDown(KeyCode.Home)) SignalSystem.Emit(new SKeyDown.Home());
            if(Input.GetKeyDown(KeyCode.End)) SignalSystem.Emit(new SKeyDown.End());
            if(Input.GetKeyDown(KeyCode.PageUp)) SignalSystem.Emit(new SKeyDown.PageUp());
            if(Input.GetKeyDown(KeyCode.PageDown)) SignalSystem.Emit(new SKeyDown.PageDown());
            if(Input.GetKeyDown(KeyCode.F1)) SignalSystem.Emit(new SKeyDown.F1());
            if(Input.GetKeyDown(KeyCode.F2)) SignalSystem.Emit(new SKeyDown.F2());
            if(Input.GetKeyDown(KeyCode.F3)) SignalSystem.Emit(new SKeyDown.F3());
            if(Input.GetKeyDown(KeyCode.F4)) SignalSystem.Emit(new SKeyDown.F4());
            if(Input.GetKeyDown(KeyCode.F5)) SignalSystem.Emit(new SKeyDown.F5());
            if(Input.GetKeyDown(KeyCode.F6)) SignalSystem.Emit(new SKeyDown.F6());
            if(Input.GetKeyDown(KeyCode.F7)) SignalSystem.Emit(new SKeyDown.F7());
            if(Input.GetKeyDown(KeyCode.F8)) SignalSystem.Emit(new SKeyDown.F8());
            if(Input.GetKeyDown(KeyCode.F9)) SignalSystem.Emit(new SKeyDown.F9());
            if(Input.GetKeyDown(KeyCode.F10)) SignalSystem.Emit(new SKeyDown.F10());
            if(Input.GetKeyDown(KeyCode.F11)) SignalSystem.Emit(new SKeyDown.F11());
            if(Input.GetKeyDown(KeyCode.F12)) SignalSystem.Emit(new SKeyDown.F12());
            if(Input.GetKeyDown(KeyCode.F13)) SignalSystem.Emit(new SKeyDown.F13());
            if(Input.GetKeyDown(KeyCode.F14)) SignalSystem.Emit(new SKeyDown.F14());
            if(Input.GetKeyDown(KeyCode.F15)) SignalSystem.Emit(new SKeyDown.F15());
            if(Input.GetKeyDown(KeyCode.Numlock)) SignalSystem.Emit(new SKeyDown.Numlock());
            if(Input.GetKeyDown(KeyCode.CapsLock)) SignalSystem.Emit(new SKeyDown.CapsLock());
            if(Input.GetKeyDown(KeyCode.ScrollLock)) SignalSystem.Emit(new SKeyDown.ScrollLock());
            if(Input.GetKeyDown(KeyCode.RightShift)) SignalSystem.Emit(new SKeyDown.RightShift());
            if(Input.GetKeyDown(KeyCode.LeftShift)) SignalSystem.Emit(new SKeyDown.LeftShift());
            if(Input.GetKeyDown(KeyCode.RightControl)) SignalSystem.Emit(new SKeyDown.RightControl());
            if(Input.GetKeyDown(KeyCode.LeftControl)) SignalSystem.Emit(new SKeyDown.LeftControl());
            if(Input.GetKeyDown(KeyCode.RightAlt)) SignalSystem.Emit(new SKeyDown.RightAlt());
            if(Input.GetKeyDown(KeyCode.LeftAlt)) SignalSystem.Emit(new SKeyDown.LeftAlt());
            if(Input.GetKeyDown(KeyCode.RightCommand)) SignalSystem.Emit(new SKeyDown.RightCommand());
            if(Input.GetKeyDown(KeyCode.RightApple)) SignalSystem.Emit(new SKeyDown.RightApple());
            if(Input.GetKeyDown(KeyCode.LeftCommand)) SignalSystem.Emit(new SKeyDown.LeftCommand());
            if(Input.GetKeyDown(KeyCode.LeftApple)) SignalSystem.Emit(new SKeyDown.LeftApple());
            if(Input.GetKeyDown(KeyCode.LeftWindows)) SignalSystem.Emit(new SKeyDown.LeftWindows());
            if(Input.GetKeyDown(KeyCode.RightWindows)) SignalSystem.Emit(new SKeyDown.RightWindows());
            if(Input.GetKeyDown(KeyCode.AltGr)) SignalSystem.Emit(new SKeyDown.AltGr());
            if(Input.GetKeyDown(KeyCode.Help)) SignalSystem.Emit(new SKeyDown.Help());
            if(Input.GetKeyDown(KeyCode.Print)) SignalSystem.Emit(new SKeyDown.Print());
            if(Input.GetKeyDown(KeyCode.SysReq)) SignalSystem.Emit(new SKeyDown.SysReq());
            if(Input.GetKeyDown(KeyCode.Break)) SignalSystem.Emit(new SKeyDown.Break());
            if(Input.GetKeyDown(KeyCode.Menu)) SignalSystem.Emit(new SKeyDown.Menu());
        }

        static void CheckKeyUp()
        {
            if(Input.GetKeyUp(KeyCode.Backspace)) SignalSystem.Emit(new SKeyUp.Backspace());
            if(Input.GetKeyUp(KeyCode.Tab)) SignalSystem.Emit(new SKeyUp.Tab());
            if(Input.GetKeyUp(KeyCode.Clear)) SignalSystem.Emit(new SKeyUp.Clear());
            if(Input.GetKeyUp(KeyCode.Return)) SignalSystem.Emit(new SKeyUp.Return());
            if(Input.GetKeyUp(KeyCode.Pause)) SignalSystem.Emit(new SKeyUp.Pause());
            if(Input.GetKeyUp(KeyCode.Escape)) SignalSystem.Emit(new SKeyUp.Escape());
            if(Input.GetKeyUp(KeyCode.Space)) SignalSystem.Emit(new SKeyUp.Space());
            if(Input.GetKeyUp(KeyCode.Exclaim)) SignalSystem.Emit(new SKeyUp.Exclaim());
            if(Input.GetKeyUp(KeyCode.DoubleQuote)) SignalSystem.Emit(new SKeyUp.DoubleQuote());
            if(Input.GetKeyUp(KeyCode.Hash)) SignalSystem.Emit(new SKeyUp.Hash());
            if(Input.GetKeyUp(KeyCode.Dollar)) SignalSystem.Emit(new SKeyUp.Dollar());
            if(Input.GetKeyUp(KeyCode.Percent)) SignalSystem.Emit(new SKeyUp.Percent());
            if(Input.GetKeyUp(KeyCode.Ampersand)) SignalSystem.Emit(new SKeyUp.Ampersand());
            if(Input.GetKeyUp(KeyCode.Quote)) SignalSystem.Emit(new SKeyUp.Quote());
            if(Input.GetKeyUp(KeyCode.LeftParen)) SignalSystem.Emit(new SKeyUp.LeftParen());
            if(Input.GetKeyUp(KeyCode.RightParen)) SignalSystem.Emit(new SKeyUp.RightParen());
            if(Input.GetKeyUp(KeyCode.Asterisk)) SignalSystem.Emit(new SKeyUp.Asterisk());
            if(Input.GetKeyUp(KeyCode.Plus)) SignalSystem.Emit(new SKeyUp.Plus());
            if(Input.GetKeyUp(KeyCode.Comma)) SignalSystem.Emit(new SKeyUp.Comma());
            if(Input.GetKeyUp(KeyCode.Minus)) SignalSystem.Emit(new SKeyUp.Minus());
            if(Input.GetKeyUp(KeyCode.Period)) SignalSystem.Emit(new SKeyUp.Period());
            if(Input.GetKeyUp(KeyCode.Slash)) SignalSystem.Emit(new SKeyUp.Slash());
            if(Input.GetKeyUp(KeyCode.Alpha0)) SignalSystem.Emit(new SKeyUp.Alpha0());
            if(Input.GetKeyUp(KeyCode.Alpha1)) SignalSystem.Emit(new SKeyUp.Alpha1());
            if(Input.GetKeyUp(KeyCode.Alpha2)) SignalSystem.Emit(new SKeyUp.Alpha2());
            if(Input.GetKeyUp(KeyCode.Alpha3)) SignalSystem.Emit(new SKeyUp.Alpha3());
            if(Input.GetKeyUp(KeyCode.Alpha4)) SignalSystem.Emit(new SKeyUp.Alpha4());
            if(Input.GetKeyUp(KeyCode.Alpha5)) SignalSystem.Emit(new SKeyUp.Alpha5());
            if(Input.GetKeyUp(KeyCode.Alpha6)) SignalSystem.Emit(new SKeyUp.Alpha6());
            if(Input.GetKeyUp(KeyCode.Alpha7)) SignalSystem.Emit(new SKeyUp.Alpha7());
            if(Input.GetKeyUp(KeyCode.Alpha8)) SignalSystem.Emit(new SKeyUp.Alpha8());
            if(Input.GetKeyUp(KeyCode.Alpha9)) SignalSystem.Emit(new SKeyUp.Alpha9());
            if(Input.GetKeyUp(KeyCode.Colon)) SignalSystem.Emit(new SKeyUp.Colon());
            if(Input.GetKeyUp(KeyCode.Semicolon)) SignalSystem.Emit(new SKeyUp.Semicolon());
            if(Input.GetKeyUp(KeyCode.Less)) SignalSystem.Emit(new SKeyUp.Less());
            if(Input.GetKeyUp(KeyCode.Equals)) SignalSystem.Emit(new SKeyUp.Equal());
            if(Input.GetKeyUp(KeyCode.Greater)) SignalSystem.Emit(new SKeyUp.Greater());
            if(Input.GetKeyUp(KeyCode.Question)) SignalSystem.Emit(new SKeyUp.Question());
            if(Input.GetKeyUp(KeyCode.At)) SignalSystem.Emit(new SKeyUp.At());
            if(Input.GetKeyUp(KeyCode.LeftBracket)) SignalSystem.Emit(new SKeyUp.LeftBracket());
            if(Input.GetKeyUp(KeyCode.Backslash)) SignalSystem.Emit(new SKeyUp.Backslash());
            if(Input.GetKeyUp(KeyCode.RightBracket)) SignalSystem.Emit(new SKeyUp.RightBracket());
            if(Input.GetKeyUp(KeyCode.Caret)) SignalSystem.Emit(new SKeyUp.Caret());
            if(Input.GetKeyUp(KeyCode.Underscore)) SignalSystem.Emit(new SKeyUp.Underscore());
            if(Input.GetKeyUp(KeyCode.BackQuote)) SignalSystem.Emit(new SKeyUp.BackQuote());
            if(Input.GetKeyUp(KeyCode.A)) SignalSystem.Emit(new SKeyUp.A());
            if(Input.GetKeyUp(KeyCode.B)) SignalSystem.Emit(new SKeyUp.B());
            if(Input.GetKeyUp(KeyCode.C)) SignalSystem.Emit(new SKeyUp.C());
            if(Input.GetKeyUp(KeyCode.D)) SignalSystem.Emit(new SKeyUp.D());
            if(Input.GetKeyUp(KeyCode.E)) SignalSystem.Emit(new SKeyUp.E());
            if(Input.GetKeyUp(KeyCode.F)) SignalSystem.Emit(new SKeyUp.F());
            if(Input.GetKeyUp(KeyCode.G)) SignalSystem.Emit(new SKeyUp.G());
            if(Input.GetKeyUp(KeyCode.H)) SignalSystem.Emit(new SKeyUp.H());
            if(Input.GetKeyUp(KeyCode.I)) SignalSystem.Emit(new SKeyUp.I());
            if(Input.GetKeyUp(KeyCode.J)) SignalSystem.Emit(new SKeyUp.J());
            if(Input.GetKeyUp(KeyCode.K)) SignalSystem.Emit(new SKeyUp.K());
            if(Input.GetKeyUp(KeyCode.L)) SignalSystem.Emit(new SKeyUp.L());
            if(Input.GetKeyUp(KeyCode.M)) SignalSystem.Emit(new SKeyUp.M());
            if(Input.GetKeyUp(KeyCode.N)) SignalSystem.Emit(new SKeyUp.N());
            if(Input.GetKeyUp(KeyCode.O)) SignalSystem.Emit(new SKeyUp.O());
            if(Input.GetKeyUp(KeyCode.P)) SignalSystem.Emit(new SKeyUp.P());
            if(Input.GetKeyUp(KeyCode.Q)) SignalSystem.Emit(new SKeyUp.Q());
            if(Input.GetKeyUp(KeyCode.R)) SignalSystem.Emit(new SKeyUp.R());
            if(Input.GetKeyUp(KeyCode.S)) SignalSystem.Emit(new SKeyUp.S());
            if(Input.GetKeyUp(KeyCode.T)) SignalSystem.Emit(new SKeyUp.T());
            if(Input.GetKeyUp(KeyCode.U)) SignalSystem.Emit(new SKeyUp.U());
            if(Input.GetKeyUp(KeyCode.V)) SignalSystem.Emit(new SKeyUp.V());
            if(Input.GetKeyUp(KeyCode.W)) SignalSystem.Emit(new SKeyUp.W());
            if(Input.GetKeyUp(KeyCode.X)) SignalSystem.Emit(new SKeyUp.X());
            if(Input.GetKeyUp(KeyCode.Y)) SignalSystem.Emit(new SKeyUp.Y());
            if(Input.GetKeyUp(KeyCode.Z)) SignalSystem.Emit(new SKeyUp.Z());
            if(Input.GetKeyUp(KeyCode.LeftCurlyBracket)) SignalSystem.Emit(new SKeyUp.LeftCurlyBracket());
            if(Input.GetKeyUp(KeyCode.Pipe)) SignalSystem.Emit(new SKeyUp.Pipe());
            if(Input.GetKeyUp(KeyCode.RightCurlyBracket)) SignalSystem.Emit(new SKeyUp.RightCurlyBracket());
            if(Input.GetKeyUp(KeyCode.Tilde)) SignalSystem.Emit(new SKeyUp.Tilde());
            if(Input.GetKeyUp(KeyCode.Delete)) SignalSystem.Emit(new SKeyUp.Delete());
            if(Input.GetKeyUp(KeyCode.Keypad0)) SignalSystem.Emit(new SKeyUp.Keypad0());
            if(Input.GetKeyUp(KeyCode.Keypad1)) SignalSystem.Emit(new SKeyUp.Keypad1());
            if(Input.GetKeyUp(KeyCode.Keypad2)) SignalSystem.Emit(new SKeyUp.Keypad2());
            if(Input.GetKeyUp(KeyCode.Keypad3)) SignalSystem.Emit(new SKeyUp.Keypad3());
            if(Input.GetKeyUp(KeyCode.Keypad4)) SignalSystem.Emit(new SKeyUp.Keypad4());
            if(Input.GetKeyUp(KeyCode.Keypad5)) SignalSystem.Emit(new SKeyUp.Keypad5());
            if(Input.GetKeyUp(KeyCode.Keypad6)) SignalSystem.Emit(new SKeyUp.Keypad6());
            if(Input.GetKeyUp(KeyCode.Keypad7)) SignalSystem.Emit(new SKeyUp.Keypad7());
            if(Input.GetKeyUp(KeyCode.Keypad8)) SignalSystem.Emit(new SKeyUp.Keypad8());
            if(Input.GetKeyUp(KeyCode.Keypad9)) SignalSystem.Emit(new SKeyUp.Keypad9());
            if(Input.GetKeyUp(KeyCode.KeypadPeriod)) SignalSystem.Emit(new SKeyUp.KeypadPeriod());
            if(Input.GetKeyUp(KeyCode.KeypadDivide)) SignalSystem.Emit(new SKeyUp.KeypadDivide());
            if(Input.GetKeyUp(KeyCode.KeypadMultiply)) SignalSystem.Emit(new SKeyUp.KeypadMultiply());
            if(Input.GetKeyUp(KeyCode.KeypadMinus)) SignalSystem.Emit(new SKeyUp.KeypadMinus());
            if(Input.GetKeyUp(KeyCode.KeypadPlus)) SignalSystem.Emit(new SKeyUp.KeypadPlus());
            if(Input.GetKeyUp(KeyCode.KeypadEnter)) SignalSystem.Emit(new SKeyUp.KeypadEnter());
            if(Input.GetKeyUp(KeyCode.KeypadEquals)) SignalSystem.Emit(new SKeyUp.KeypadEquals());
            if(Input.GetKeyUp(KeyCode.UpArrow)) SignalSystem.Emit(new SKeyUp.UpArrow());
            if(Input.GetKeyUp(KeyCode.DownArrow)) SignalSystem.Emit(new SKeyUp.DownArrow());
            if(Input.GetKeyUp(KeyCode.RightArrow)) SignalSystem.Emit(new SKeyUp.RightArrow());
            if(Input.GetKeyUp(KeyCode.LeftArrow)) SignalSystem.Emit(new SKeyUp.LeftArrow());
            if(Input.GetKeyUp(KeyCode.Insert)) SignalSystem.Emit(new SKeyUp.Insert());
            if(Input.GetKeyUp(KeyCode.Home)) SignalSystem.Emit(new SKeyUp.Home());
            if(Input.GetKeyUp(KeyCode.End)) SignalSystem.Emit(new SKeyUp.End());
            if(Input.GetKeyUp(KeyCode.PageUp)) SignalSystem.Emit(new SKeyUp.PageUp());
            if(Input.GetKeyUp(KeyCode.PageDown)) SignalSystem.Emit(new SKeyUp.PageDown());
            if(Input.GetKeyUp(KeyCode.F1)) SignalSystem.Emit(new SKeyUp.F1());
            if(Input.GetKeyUp(KeyCode.F2)) SignalSystem.Emit(new SKeyUp.F2());
            if(Input.GetKeyUp(KeyCode.F3)) SignalSystem.Emit(new SKeyUp.F3());
            if(Input.GetKeyUp(KeyCode.F4)) SignalSystem.Emit(new SKeyUp.F4());
            if(Input.GetKeyUp(KeyCode.F5)) SignalSystem.Emit(new SKeyUp.F5());
            if(Input.GetKeyUp(KeyCode.F6)) SignalSystem.Emit(new SKeyUp.F6());
            if(Input.GetKeyUp(KeyCode.F7)) SignalSystem.Emit(new SKeyUp.F7());
            if(Input.GetKeyUp(KeyCode.F8)) SignalSystem.Emit(new SKeyUp.F8());
            if(Input.GetKeyUp(KeyCode.F9)) SignalSystem.Emit(new SKeyUp.F9());
            if(Input.GetKeyUp(KeyCode.F10)) SignalSystem.Emit(new SKeyUp.F10());
            if(Input.GetKeyUp(KeyCode.F11)) SignalSystem.Emit(new SKeyUp.F11());
            if(Input.GetKeyUp(KeyCode.F12)) SignalSystem.Emit(new SKeyUp.F12());
            if(Input.GetKeyUp(KeyCode.F13)) SignalSystem.Emit(new SKeyUp.F13());
            if(Input.GetKeyUp(KeyCode.F14)) SignalSystem.Emit(new SKeyUp.F14());
            if(Input.GetKeyUp(KeyCode.F15)) SignalSystem.Emit(new SKeyUp.F15());
            if(Input.GetKeyUp(KeyCode.Numlock)) SignalSystem.Emit(new SKeyUp.Numlock());
            if(Input.GetKeyUp(KeyCode.CapsLock)) SignalSystem.Emit(new SKeyUp.CapsLock());
            if(Input.GetKeyUp(KeyCode.ScrollLock)) SignalSystem.Emit(new SKeyUp.ScrollLock());
            if(Input.GetKeyUp(KeyCode.RightShift)) SignalSystem.Emit(new SKeyUp.RightShift());
            if(Input.GetKeyUp(KeyCode.LeftShift)) SignalSystem.Emit(new SKeyUp.LeftShift());
            if(Input.GetKeyUp(KeyCode.RightControl)) SignalSystem.Emit(new SKeyUp.RightControl());
            if(Input.GetKeyUp(KeyCode.LeftControl)) SignalSystem.Emit(new SKeyUp.LeftControl());
            if(Input.GetKeyUp(KeyCode.RightAlt)) SignalSystem.Emit(new SKeyUp.RightAlt());
            if(Input.GetKeyUp(KeyCode.LeftAlt)) SignalSystem.Emit(new SKeyUp.LeftAlt());
            if(Input.GetKeyUp(KeyCode.RightCommand)) SignalSystem.Emit(new SKeyUp.RightCommand());
            if(Input.GetKeyUp(KeyCode.RightApple)) SignalSystem.Emit(new SKeyUp.RightApple());
            if(Input.GetKeyUp(KeyCode.LeftCommand)) SignalSystem.Emit(new SKeyUp.LeftCommand());
            if(Input.GetKeyUp(KeyCode.LeftApple)) SignalSystem.Emit(new SKeyUp.LeftApple());
            if(Input.GetKeyUp(KeyCode.LeftWindows)) SignalSystem.Emit(new SKeyUp.LeftWindows());
            if(Input.GetKeyUp(KeyCode.RightWindows)) SignalSystem.Emit(new SKeyUp.RightWindows());
            if(Input.GetKeyUp(KeyCode.AltGr)) SignalSystem.Emit(new SKeyUp.AltGr());
            if(Input.GetKeyUp(KeyCode.Help)) SignalSystem.Emit(new SKeyUp.Help());
            if(Input.GetKeyUp(KeyCode.Print)) SignalSystem.Emit(new SKeyUp.Print());
            if(Input.GetKeyUp(KeyCode.SysReq)) SignalSystem.Emit(new SKeyUp.SysReq());
            if(Input.GetKeyUp(KeyCode.Break)) SignalSystem.Emit(new SKeyUp.Break());
            if(Input.GetKeyUp(KeyCode.Menu)) SignalSystem.Emit(new SKeyUp.Menu());
        }

        static void CheckMouseDown()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0)) SignalSystem.Emit(new SMouseDown.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse1)) SignalSystem.Emit(new SMouseDown.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse2)) SignalSystem.Emit(new SMouseDown.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse3)) SignalSystem.Emit(new SMouseDown.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse4)) SignalSystem.Emit(new SMouseDown.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse5)) SignalSystem.Emit(new SMouseDown.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKeyDown(KeyCode.Mouse6)) SignalSystem.Emit(new SMouseDown.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMousePressing()
        {
            if(Input.GetKey(KeyCode.Mouse0)) SignalSystem.Emit(new SMouse.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse1)) SignalSystem.Emit(new SMouse.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse2)) SignalSystem.Emit(new SMouse.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse3)) SignalSystem.Emit(new SMouse.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse4)) SignalSystem.Emit(new SMouse.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse5)) SignalSystem.Emit(new SMouse.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKey(KeyCode.Mouse6)) SignalSystem.Emit(new SMouse.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMouseUp()
        {
            if(Input.GetKeyUp(KeyCode.Mouse0)) SignalSystem.Emit(new SMouseUp.Mouse0() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse1)) SignalSystem.Emit(new SMouseUp.Mouse1() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse2)) SignalSystem.Emit(new SMouseUp.Mouse2() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse3)) SignalSystem.Emit(new SMouseUp.Mouse3() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse4)) SignalSystem.Emit(new SMouseUp.Mouse4() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse5)) SignalSystem.Emit(new SMouseUp.Mouse5() { pos = Input.mousePosition });
            if(Input.GetKeyUp(KeyCode.Mouse6)) SignalSystem.Emit(new SMouseUp.Mouse6() { pos = Input.mousePosition });
        }

        static void CheckMouseMoveInout()
        {
            var curPos = Input.mousePosition;
            if(curPos != mousePosLastFrame)
            {
                SignalSystem.Emit(new Signals.MouseMove() { delta = curPos - mousePosLastFrame });
            }

            mousePosLastFrame = curPos;
        }

        static void CheckMouseScrollInput()
        {
            if(Input.mouseScrollDelta.y != 0f)
            {
                SignalSystem.Emit(new Signals.MouseScroll() { delta = Input.mouseScrollDelta.y });
            }
        }

    }
}