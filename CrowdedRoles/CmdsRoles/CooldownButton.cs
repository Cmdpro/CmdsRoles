// Credits to https://gist.github.com/gabriel-nsiqueira/827dea0a1cdc2210db6f9a045ec4ce0a and https://gist.github.com/naturecodevoid/1c61786e6a95d7d093f495b6e67aad29 for the original code.

using HarmonyLib;
using Reactor.Extensions;
using Reactor.Unstrip;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reactor.Button
{
    public class CooldownButton
    {
        public static List<CooldownButton> buttons = new List<CooldownButton>();
        public KillButtonManager KillButtonManager;
        public Vector2 PositionOffset = Vector2.zero;
        public float MaxTimer = 0f;
        public float Timer = 0f;
        public float EffectDuration = 0f;
        public bool IsEffectActive;
        public bool HasEffectDuration;
        public bool Enabled = true;
        public bool Useable = true;
        public Func<bool> UseTester;
        private Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();
        private byte[] Image_;
        public bool CanTime = true;
        public byte[] Image
        {
            get { return Image_; }
            set
            {
                Image_ = value;
                string sha = GetHashSHA1(value);
                if (cache.ContainsKey(sha))
                {
                    Sprite_ = GUIExtensions.CreateSprite(cache[sha]);
                    return;
                }
                Texture2D tex = GUIExtensions.CreateEmptyTexture();
                ImageConversion.LoadImage(tex, value, false);
                tex = ScaleTexture(tex, 125, 125);
                cache[sha] = tex;
                Sprite_ = GUIExtensions.CreateSprite(tex);
            }
        }
        private Sprite Sprite_;
        public Action OnClick;
        public Action OnEffectEnd;
        public HudManager HudManager;
        public bool CanUse_;

        public CooldownButton(Action onClick, float cooldown, byte[] image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager, float effectDuration, Action onEffectEnd)
        {
            SetVars(onClick, cooldown, image, positionOffset, useTester, hudManager);
            this.HasEffectDuration = true;
            this.IsEffectActive = false;
            this.OnEffectEnd = onEffectEnd;
            this.EffectDuration = effectDuration;
        }

        public CooldownButton(Action onClick, float cooldown, byte[] image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager)
        {
            SetVars(onClick, cooldown, image, positionOffset, useTester, hudManager);
            this.HasEffectDuration = false;
            Update();
        }
        private void SetVars(Action onClick, float cooldown, byte[] image, Vector2 positionOffset, Func<bool> useTester, HudManager hudManager)
        {
            buttons.Add(this);
            this.HudManager = hudManager;
            this.OnClick = onClick;
            this.PositionOffset = positionOffset;
            this.UseTester = useTester;
            this.MaxTimer = cooldown;
            this.Timer = MaxTimer;
            this.Image = image;
            this.KillButtonManager = UnityEngine.Object.Instantiate(HudManager.KillButton, HudManager.transform);
            Update();
        }
        internal bool CanUse()
        {
            if (PlayerControl.LocalPlayer == null || PlayerControl.LocalPlayer.Data == null) return false;
            CanUse_ = UseTester();
            return true;
        }
        internal static void HudUpdate()
        {
            buttons.RemoveAll(item => item.KillButtonManager == null);
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].CanUse())
                    buttons[i].Update();
            }
        }
        private void Update()
        {
            KillButtonManager.killText.text = "";
            if (KillButtonManager.transform.localPosition.x > 0f)
                KillButtonManager.transform.localPosition = new Vector3((KillButtonManager.transform.localPosition.x + 1.3f) * -1, KillButtonManager.transform.localPosition.y, KillButtonManager.transform.localPosition.z) + new Vector3(PositionOffset.x, PositionOffset.y);
            PassiveButton button = KillButtonManager.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)listener);
            void listener()
            {
                if (Timer < 0f && CanUse_ && Enabled)
                {
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
                    if (HasEffectDuration)
                    {
                        IsEffectActive = true;
                        Timer = EffectDuration;
                        KillButtonManager.TimerText.color = new Color(0, 255, 0);
                    }
                    else
                    {
                        Timer = MaxTimer;
                    }
                    if (Useable == true)
                    {
                        OnClick();
                    }
                }
                

            }
            KillButtonManager.renderer.sprite = this.Sprite_;
            if (Timer > 999)
            {
                
            }
            if (Timer < 0f)
            {
                if (PlayerControl.LocalPlayer.CanMove == false)
                {
                    KillButtonManager.SetCoolDown(MaxTimer, MaxTimer);
                    Timer = 0;
                }

                if (Enabled)
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 1f);
                else
                    KillButtonManager.renderer.color = new Color(1f, 1f, 1f, .3f);
                if (IsEffectActive)
                {
                    KillButtonManager.TimerText.color = new Color(255, 255, 255);
                    Timer = MaxTimer;
                    //if (CanEnd == true)
                    //{
                        IsEffectActive = false;
                        OnEffectEnd();
                    //}
                }
            }
            else
            {
                if (CanUse_ && (PlayerControl.LocalPlayer.CanMove || IsEffectActive))
                    if (CanTime == true) {
                        Timer -= Time.deltaTime;
                    }
                KillButtonManager.renderer.color = new Color(1f, 1f, 1f, 0.3f);
            }
            KillButtonManager.gameObject.SetActive(CanUse_);
            KillButtonManager.renderer.enabled = CanUse_;
            if (CanUse_)
            {
                KillButtonManager.renderer.material.SetFloat("_Desat", 0f);
                KillButtonManager.SetCoolDown(Timer, MaxTimer);
            }
        }
        public static string GetHashSHA1(byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                return string.Concat(sha1.ComputeHash(data).Join(x => x.ToString("X2")));
            }
        }
        // Credit: http://jon-martin.com/?p=114
        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class ButtonUpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            CooldownButton.HudUpdate();
        }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class ButtonResetPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            CooldownButton.buttons.RemoveAll(item => item.KillButtonManager == null);
            for (int i = 0; i < CooldownButton.buttons.Count; i++)
            {
                CooldownButton.buttons[i].KillButtonManager.TimerText.color = new Color(255, 255, 255);
                CooldownButton.buttons[i].Timer = CooldownButton.buttons[i].MaxTimer;
                CooldownButton.buttons[i].IsEffectActive = false;
                CooldownButton.buttons[i].OnEffectEnd();
                CooldownButton.buttons[i].KillButtonManager.gameObject.SetActive(false);
            }
        }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class HudClose
    {
        public static void Postfix(MeetingHud __instance)
        {
            for (int i = 0; i < CooldownButton.buttons.Count; i++)
            {
                CooldownButton.buttons[i].KillButtonManager.gameObject.SetActive(true);
            }
        }
    }
}