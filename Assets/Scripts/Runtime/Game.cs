using System;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Runtime
{
    public static class Game
    {
        private static AssetRoot _assetRoot;
        private static Runner _runner;
        
        public static AssetRoot AssetRoot => _assetRoot;
        
        public static void SetAssetRoot(AssetRoot assetRoot)
        {
            _assetRoot = assetRoot;
        }
        
        public static void LoadMarketplace()
        {
            SceneManager.LoadSceneAsync("Marketplace");
        }        
        public static void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public static void LoadGame()
        {
            SceneManager.LoadSceneAsync("Game");
        }
    }
}