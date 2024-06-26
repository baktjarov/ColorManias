﻿using GoogleMobileAds.Api;
using System;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class AdsShower_AdMob : IAdsShower
    {
        private string _bannerID = "ca-app-pub-1922629388317265/4922117595";
        private string _interstitialID = "ca-app-pub-1922629388317265/1016013247";
        private string _rewardedID = "ca-app-pub-1922629388317265/1051726162";

        private BannerView _bannerView;
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;

        public void Initialize()
        {
            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
        }

        private void LoadBanner()
        {
            _bannerView = new BannerView(_bannerID, AdSize.Banner, AdPosition.Bottom);
            _bannerView.LoadAd(new AdRequest());
        }

        private void LoadInterstitial()
        {
            InterstitialAd.Load(_interstitialID, new AdRequest(), OnLoaded);

            void OnLoaded(InterstitialAd interstitialAd, LoadAdError loadAdError)
            {
                if (loadAdError != null) { Debug.LogError(loadAdError.GetMessage()); }
                _interstitialAd = interstitialAd;
            }
        }

        private void LoadRewarded()
        {
            RewardedAd.Load(_rewardedID, new AdRequest(), OnLoaded);

            void OnLoaded(RewardedAd rewardedAd, LoadAdError loadAdError)
            {
                if (loadAdError != null) { Debug.LogError(loadAdError.GetMessage()); }
                _rewardedAd = rewardedAd;
            }
        }

        public void ShowBanner()
        {
            _bannerView?.Show();
        }

        public void HideBanner()
        {
            _bannerView?.Hide();
        }

        public void ShowInterstitial()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd() == true)
            {
                _interstitialAd.Show();
            }
        }

        public void ShowRewarded(Action<Reward> onRewarded)
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd() == true)
            {
                _rewardedAd.Show((rewarded) =>
                {
                    onRewarded?.Invoke(rewarded);
                    LoadRewarded();
                });
            }
        }
    }
}