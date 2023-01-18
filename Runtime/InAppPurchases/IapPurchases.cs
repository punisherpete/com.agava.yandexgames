using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace Agava.YandexGames
{
    public static class InAppPurchases
    {
        // Mutable static fields - absolutely disgusting.
        private static Action s_onSuccessCallback;
        private static Action s_onRewardedCallback;
        private static Action s_onErrorCallback;
        private static Action s_onPaymentFailedCallback;

        public static void InitPayments(Action onOpenCallback = null, Action onErrorCallback = null)
        {
            s_onSuccessCallback = onOpenCallback;
            s_onErrorCallback = onErrorCallback;

            InitializePayments(OnSuccessCallback, OnErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void InitializePayments(Action onSuccessCallback, Action OnCloseCallbak);

        public static void BuyItem(string itemId = "", Action onRewardedCallback = null, Action onPaymentFailedCallback = null)
        {
            s_onRewardedCallback = onRewardedCallback;
            s_onPaymentFailedCallback = onPaymentFailedCallback;

            YandexShowOrderBox(itemId, OnRewardedCallback, OnPaymentFailedCallback);
        }

        [DllImport("__Internal")]
        private static extern void YandexShowOrderBox(string id, Action onRewardedCallback, Action onCloseCallback);

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnSuccessCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnSuccessCallback)} invoked");

            s_onSuccessCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnErrorCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnErrorCallback)} invoked");

            s_onErrorCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void OnRewardedCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnRewardedCallback)} invoked");

            s_onRewardedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action<bool>))]
        private static void OnPaymentFailedCallback()
        {
            if (YandexGamesSdk.CallbackLogging)
                Debug.Log($"{nameof(InterstitialAd)}.{nameof(OnPaymentFailedCallback)} invoked");

            s_onPaymentFailedCallback?.Invoke();
        }
    }
}
